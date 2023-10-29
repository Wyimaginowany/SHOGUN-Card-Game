using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public abstract class EnemyCombat
public class EnemyCombat : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _damage;

    [Header("To Attach")]
    [SerializeField] private AnimatorOverrideController _overrideController;


    private Animator _animator;
    private CombatManager _combatManager;
    private PlayerHealth _playerHealth;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = _overrideController;

        _playerHealth = (PlayerHealth)FindObjectOfType(typeof(PlayerHealth));
        _combatManager = (CombatManager)FindObjectOfType(typeof(CombatManager));
        //_playerEffects = ...
    }

    
    //protected void HandleTurn()


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
