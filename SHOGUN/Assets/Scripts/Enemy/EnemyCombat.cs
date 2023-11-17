using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public abstract class EnemyCombat : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected int _damage;

    [Header("To Attach")]
    [SerializeField] private AnimatorOverrideController _overrideController;
    

    protected Animator _animator;
    private CombatManager _combatManager;
    protected PlayerHealth playerHealth;

    protected virtual void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = _overrideController;

        playerHealth = (PlayerHealth)FindObjectOfType(typeof(PlayerHealth));
        _combatManager = (CombatManager)FindObjectOfType(typeof(CombatManager));
        //_playerEffects = ...
    }
    
    public virtual void HandleTurn()
    {
        
    }

    private void AttackAnimationEvent()
    {
        playerHealth.TakeDamage(_damage);
    }

    private void EndAttackAnimationEvent()
    {
        _combatManager.NextEnemyTurn();
    }
}
