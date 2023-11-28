using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    private List<EnemyAttack> _attacksPool = new List<EnemyAttack>();
    private EnemyAttack _chosenAttack;

    [System.Serializable]
    public class EnemyAttack
    {
        public String AttackName = "";
        public OniAttackTypes AttackType;
        public int AttackPriority = 0;
        public int AttackCooldown = 0;
        public int AttackMaxCooldown = 2;
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
        switch (_chosenAttack.AttackType)
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

        _turnTimeAmount = _chosenAttack.AttackDuration;
        _animator.SetTrigger(_chosenAttack.AnimatorTrigger);
        _chosenAttack.AttackCooldown = _chosenAttack.AttackMaxCooldown;
        if (_chosenAttack.RemoveAfterUsage) _oniPossibleAttacks.Remove(_chosenAttack);

        base.HandleTurn();
    }

    public override void PrepareAttack()
    {
        GetTurnAttack();
        _attackIntentionText.text = GetChosenAttackName();
    }

    public String GetChosenAttackName()
    {
        return _chosenAttack.AttackName;
    } 

    private void GetTurnAttack()
    {
        _attacksPool = new List<EnemyAttack>();

        foreach (EnemyAttack enemyAttack in _oniPossibleAttacks)
        {
            if (enemyAttack.AttackCooldown > 0)
            {
                enemyAttack.AttackCooldown--;
                continue;
            }

            for (int i = 0; i < enemyAttack.AttackPriority; i++)
            {
                _attacksPool.Add(enemyAttack);
            }
        }

        _chosenAttack = _attacksPool.ElementAt(UnityEngine.Random.Range(0, _attacksPool.Count));
        _chosenAttack.AttackCooldown++;
    }

    private void DealDamage(int damage)
    {
        damage *= Mathf.RoundToInt((100 + _currentBerserkMuliplier) / 100);
        _combatManager.DealDamageToPlayer(damage);
    }

    private void StrongHit()
    {
        int damage = UnityEngine.Random.Range(_strongHitMinDmg, _strongHitMaxDmg);
        DealDamage(damage);

        Debug.Log("strongHit");
    }

    private void StunAttack()
    {
        int damage = UnityEngine.Random.Range(_stunAttackMinDmg, _stunAttackMaxDmg);
        DealDamage(damage);

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