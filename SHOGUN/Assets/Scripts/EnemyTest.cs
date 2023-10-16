using System;
using TMPro;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _damage;

    [Header("To Attach")]
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _damageText;
    [SerializeField] private Animator _popupAnimator;
    [SerializeField] private AnimatorOverrideController _overrideController;

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
        _healthText.text = _currentHealth.ToString();
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

        _healthText.text = _currentHealth.ToString();
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
