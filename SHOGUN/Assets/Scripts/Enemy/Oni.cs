﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class Oni : EnemyCombat
{
    [Header("Attacks")]
    [SerializeField] private List<EnemyAttack> _oniPossibleAttacks = new List<EnemyAttack>();
    [Space(5)]
    [Header("Strong Attack")]
    [SerializeField] private int _strongHitMinDmg = 12;
    [SerializeField] private int _strongHitMaxDmg = 20;
    [Space(5)]
    [Header("Stun Attack")]
    [SerializeField] private int _minShieldToBlockStun = 6;
    [SerializeField] private int _stunAttackMinDmg = 6;
    [SerializeField] private int _stunAttackMaxDmg = 9;
    [Space(5)]
    [Header("Buff Attack")]
    [SerializeField] private int _damageBuffPerUse = 2;
    [Space(5)]
    [Header("Berserk Attack")]
    [SerializeField] private int _berserkDamageBuffPercentage = 30;
    [SerializeField] private int _berserkHealthLossPercentage = 50;
      
    private EnemyHealth _enemyHealth;
    private int _currentBerserkMuliplier = 0;
    private int _currentDamageBuff = 0;

    [System.Serializable]
    public class EnemyAttack
    {
        public OniAttackTypes AttackType;
        public int AttackPriority = 0;
        public int AttackCooldown = 2;
        public int AttackCooldownAtStart = 2;
        public bool RemoveAfterUsage = false;
        public string AnimatorTrigger = "";
        public float AttackDuration = 2f;
    }


    protected override void Start()
    {
        base.Start();
        _enemyHealth = GetComponent<EnemyHealth>();
        
    }

    public override void HandleTurn()
    {
        EnemyAttack oniAttackSelected = GetTurnAttack();

        switch (oniAttackSelected.AttackType)
        {
            case OniAttackTypes.StrongAttack:
                StrongHit();
                break;
            case OniAttackTypes.StunAttack:
                StunAttack();
                break;
            case OniAttackTypes.BuffAttack:
                BuffDamage();
                break;
            case OniAttackTypes.Berserk:
                Berserk();
                break;           
        }

        _turnTimeAmount = oniAttackSelected.AttackDuration;
        _animator.SetTrigger(oniAttackSelected.AnimatorTrigger);
        oniAttackSelected.AttackCooldown = oniAttackSelected.AttackCooldownAtStart;
        if (oniAttackSelected.RemoveAfterUsage) _oniPossibleAttacks.Remove(oniAttackSelected);

        base.HandleTurn();
    }

    private EnemyAttack GetTurnAttack()
    {
        EnemyAttack chosenAttack = null;
        int enemyPriority = -1;

        foreach (EnemyAttack enemyAttack in _oniPossibleAttacks)
        {
            if (enemyAttack.AttackCooldown > 0)
            {
                enemyAttack.AttackCooldown--;
                continue;
            }
                

            if (enemyAttack.AttackPriority > enemyPriority)
            {
                enemyPriority = enemyAttack.AttackPriority;
                chosenAttack = enemyAttack;
            }
        }

        chosenAttack.AttackCooldown++;
        return chosenAttack;
    }

    private void DealDamageToPlayer(int damage)
    {
        damage *= Mathf.RoundToInt((100 + _currentBerserkMuliplier) / 100);
        playerHealth.TakeDamage(damage + _currentDamageBuff);
    }

    private void StrongHit()
    {
        int damage = UnityEngine.Random.Range(_strongHitMinDmg, _strongHitMaxDmg);
        DealDamageToPlayer(damage);

        Debug.Log("strongHit");
    }

    private void StunAttack()
    {
        int damage = UnityEngine.Random.Range(_stunAttackMinDmg, _stunAttackMaxDmg);
        DealDamageToPlayer(damage);

        if (playerHealth.GetPlayerShield() < _minShieldToBlockStun)
        {
            //TODO: Stun player
            Debug.Log("stunPlayer");
        }
        
        Debug.Log("stunAttack");
    }

    private void BuffDamage()
    {
        //TODO: buff dmg by 2 for the rest of the combat (effect stacks)
        _currentDamageBuff += _damageBuffPerUse;
        Debug.Log("buffDamage");
    }

    private void Berserk()
    {
        //TODO: decrease currentHealth by 50%, but increase damage by 30% for the rest of the combat (one time use)
        _enemyHealth.SetCurrentHealth(_enemyHealth.GetCurrentHealth()/(100 / _berserkHealthLossPercentage));
        _currentBerserkMuliplier = _berserkDamageBuffPercentage;
        Debug.Log("berserk");
    }
}

public enum OniAttackTypes { StrongAttack, StunAttack, BuffAttack, Berserk }