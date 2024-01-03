using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IBleedable
{
    [Header("Settings")]
    [SerializeField] private int _maxHealth;

    [Header("To Attach")]
    [SerializeField] private TMP_Text _damageText;
    [SerializeField] private Animator _enemyDamagePopupAnimator;
    [SerializeField] private TMP_Text _healthbarAmountText;
    [SerializeField] private Slider _healthbarSlider;
    [SerializeField] private float _timeAfterDeath=2f;
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
        _healthbarAmountText.text = _currentHealth.ToString() + "/" + _maxHealth.ToString();
        _healthbarSlider.value = 1;
        _untargetableCanvas.SetActive(false);
        
        CombatManager.OnPlayerTurnEnd += ResetShield;
        CombatManager.OnPlayerTurnEnd += TakeBleedDamage;
        CombatManager.OnPlayerTurnEnd += MakeTargetable;
    }
    
    private void ResetShield()
    {
        _currentShield = 0;
    }
    
    public void GiveShield(int shieldAmount)
    {
        _currentShield += shieldAmount;
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
        _currentShield = Mathf.Clamp(_currentShield - damage, 0, 10000);

        if (damageToEnemy > 0)
        {
            _currentHealth -= damageToEnemy;

        }
        
        if(_currentHealth > 0)  
        {
            _animator.SetTrigger("Take-damage");
        }
        else
        {
            _currentHealth = 0;
            _animator.SetTrigger("Death");
            StartCoroutine(RunTimer());
        }

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

    public void TakeBleedDamage()
    {
        if (_currentBleedStacks <= 0) return;

        TakeDamage(_currentBleedStacks);
        _currentBleedStacks--;
    }

}
