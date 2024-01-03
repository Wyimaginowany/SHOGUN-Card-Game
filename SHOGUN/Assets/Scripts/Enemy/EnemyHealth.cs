using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
public class EnemyHealth : MonoBehaviour, IBleedable
{
    [Header("Settings")]
    [SerializeField] private int _maxHealth;

    [Header("To Attach")]
    [SerializeField] private TMP_Text _damageText;
    [SerializeField] private Animator _enemyDamagePopupAnimator;
    [SerializeField] private TMP_Text _healthbarAmountText;
    [SerializeField] private TMP_Text _shieldAmountText;
    [SerializeField] private Slider _healthbarSlider;
    [SerializeField] private Slider _shieldSlider;
    [SerializeField] private float _timeAfterDeath=2f;
    [SerializeField] private Transform _arrowEndPoint;
    [SerializeField] private GameObject _untargetableCanvas;

    public static event Action<EnemyHealth> OnEnemyDeath;

    private int _currentHealth;
    private int _currentShield = 0;
    private int _currentBleedStacks = 0;
    public bool isTargetable = true;
    protected Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _currentHealth = _maxHealth;
        UpdateHealthbarVisual();
        UpdateShieldVisual();

        _healthbarAmountText.text = _currentHealth.ToString() + "/" + _maxHealth.ToString();
        _healthbarSlider.value = 1;
        _untargetableCanvas.SetActive(false);
        
        CombatManager.OnPlayerTurnEnd += ResetShield;
        CombatManager.OnPlayerTurnEnd += TakeBleedDamage;
        CombatManager.OnPlayerTurnEnd += MakeTargetable;
    }

    private void OnDestroy()
    {
        CombatManager.OnPlayerTurnEnd -= ResetShield;
    }

    private void ResetShield()
    {
        _currentShield = 0;
        _shieldSlider.gameObject.SetActive(false);
    }
    
    public void GiveShield(int shieldAmount)
    {
        _currentShield += shieldAmount;
        UpdateShieldVisual();
    }
    
    public int GetEnemyShield()
    {
        return _currentShield;
    }

    public void MakeTargetable()
    {
        isTargetable = true;
        _untargetableCanvas.SetActive(false);
    }
    
    public void MakeUntargetable()
    {
        isTargetable = false;
        _untargetableCanvas.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        _damageText.text = "-" + damage.ToString();
        _enemyDamagePopupAnimator.SetTrigger("damage");

        int damageToEnemy = damage - _currentShield;
        _currentShield -= damage;
        if (_currentShield < 0) _currentShield = 0;

        if (damageToEnemy > 0)
        {
            _currentHealth -= damageToEnemy;

        }

        if (_currentHealth > 0)
        {
            _animator.SetTrigger("Take-damage");
        }
        else
        {
            _currentHealth = 0;
            _animator.SetTrigger("Death");
            StartCoroutine(RunTimer());
        }

        UpdateHealthbarVisual();
        UpdateShieldVisual();
    }

    private void UpdateShieldVisual()
    {
        _shieldAmountText.text = _currentShield.ToString();
        _shieldSlider.value = _currentShield;

        if (_currentShield > 0)
        {
            _shieldSlider.gameObject.SetActive(true);
            return;
        }

        _shieldSlider.gameObject.SetActive(false);
    }

    private void UpdateHealthbarVisual()
    {
        _healthbarAmountText.text = _currentHealth.ToString() + "/" + _maxHealth.ToString();
        _healthbarSlider.value = ((float)_currentHealth) / ((float)_maxHealth);
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }

    public void SetCurrentHealth(int newHealth)
    {
        _currentHealth = newHealth;
        UpdateHealthbarVisual();
    }

    IEnumerator RunTimer()
    {
        yield return new WaitForSeconds(_timeAfterDeath);
        EnemyDeath();
    }

    public void EnemyDeath(){
        OnEnemyDeath?.Invoke(this);
        Destroy(gameObject);
    }

    public Vector3 GetArrowEndPoint()
    {
        return _arrowEndPoint.position;
    public void TakeBleedDamage()
    {
        if (_currentBleedStacks <= 0) return;

        TakeDamage(_currentBleedStacks);
        _currentBleedStacks--;
    }

}
