using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTest : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _damage;

    [Header("To Attach")]
    [SerializeField] private TMP_Text _damageText;
    [SerializeField] private Animator _popupAnimator;
    [SerializeField] private AnimatorOverrideController _overrideController;
    
    [SerializeField] private TMP_Text _healthbarAmountText;
    [SerializeField] private Slider _healthbarSlider;

    public static event Action<EnemyTest> OnEnemyDeath;

    private Animator _animator;
    private CombatManager _combatManager;
    private PlayerHealth _playerHealth;
    private int _currentHealth;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = _overrideController;

        _playerHealth = (PlayerHealth)FindObjectOfType(typeof(PlayerHealth));
        _combatManager = (CombatManager)FindObjectOfType(typeof(CombatManager));

        _currentHealth = _maxHealth;
        _healthbarAmountText.text = _currentHealth.ToString() + "/" + _maxHealth.ToString();
        _healthbarSlider.value=1;
    }

    public void TakeDamage(int damage)
    {
        _damageText.text = "-"+damage.ToString();
        _popupAnimator.SetTrigger("Take-damage");
        
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            OnEnemyDeath?.Invoke(this);
            Destroy(gameObject);
        }

        _healthbarAmountText.text = _currentHealth.ToString() + "/" + _maxHealth.ToString();
        _healthbarSlider.value= ((float) _currentHealth) / ((float)_maxHealth);
    }

    public void HandleTurn()
    {
        _animator.SetTrigger("attack");
    }

    private void AttackAnimationEvent()
    {
        _playerHealth.TakeDamage(_damage);
    }

    private void EndAttackAnimationEvent()
    {
        _combatManager.NextEnemyTurn();
    }
}
