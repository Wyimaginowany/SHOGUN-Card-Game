using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public abstract class EnemyCombat
public abstract class EnemyCombat : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _damage;

    [Header("To Attach")]
    [SerializeField] private AnimatorOverrideController _overrideController;
    

    private Animator _animator;
    private CombatManager _combatManager;
    protected PlayerHealth playerHealth;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = _overrideController;

        playerHealth = (PlayerHealth)FindObjectOfType(typeof(PlayerHealth));
        _combatManager = (CombatManager)FindObjectOfType(typeof(CombatManager));
        //_playerEffects = ...
    }


    protected virtual void HandleTurn()
    {
        _animator.SetTrigger("attack");
    }
    // public void HandleTurn()
    // {
    //     _animator.SetTrigger("attack");
    // }

    private void AttackAnimationEvent()
    {
        playerHealth.TakeDamage(_damage);
    }

    private void EndAttackAnimationEvent()
    {
        _combatManager.NextEnemyTurn();
    }
}
