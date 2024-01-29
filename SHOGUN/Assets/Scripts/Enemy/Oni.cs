using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Oni : EnemyCombat
{
    #region Fields
    [Header("Attacks")]
    [SerializeField] private List<EnemyAttack<OniAttacks>> _oniPossibleAttacks = new List<EnemyAttack<OniAttacks>>();
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
    private List<EnemyAttack<OniAttacks>> _attacksPool = new List<EnemyAttack<OniAttacks>>();
    private EnemyAttack<OniAttacks> _chosenAttack;
    
    #endregion
    
    protected override void Start()
    {
        base.Start();
        _enemyHealth = GetComponent<EnemyHealth>();
        
    }

    public override void HandleTurn()
    {
        switch (_chosenAttack.Attack)
        {
            case OniAttacks.StrongAttack:
                StrongHit();
                break;
            case OniAttacks.StunAttack:
                StunAttack();
                break;
            case OniAttacks.BuffAttack:
                BuffDamage();
                break;
            case OniAttacks.Berserk:
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
        DisplayEnemyIntentions();
    }

    private void GetTurnAttack()
    {
        _attacksPool = new List<EnemyAttack<OniAttacks>>();

        foreach (EnemyAttack<OniAttacks> enemyAttack in _oniPossibleAttacks)
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
    public void DisplayEnemyIntentions()
    {
        _attackIntentionText.text = _chosenAttack.AttackName;
        _attackDescriptionText.text = ChangeAttackDescription(_chosenAttack.Attack);
        AttackTypes attackType = _chosenAttack.AttackType;
        String iconName = (attackType + "_icon").ToLower();
        String iconPath = "Enemy Intention Icons/" + iconName;
        _iconGameObject.GetComponent<Image>().sprite = Resources.Load<Sprite> (iconPath);
    }

    private string ChangeAttackDescription(OniAttacks attackType)
    {
        string attackDescriptionText = _chosenAttack.Description;

        if (attackType == OniAttacks.StrongAttack)
        {
            attackDescriptionText = attackDescriptionText.Replace("@", ((_strongHitMinDmg + _currentDamageBuff) * Mathf.RoundToInt((100 + (float)_currentBerserkMuliplier) / 100)).ToString());
            attackDescriptionText = attackDescriptionText.Replace("#", ((_strongHitMaxDmg + _currentDamageBuff) * Mathf.RoundToInt((100 + (float)_currentBerserkMuliplier) / 100)).ToString());
        }
        if (attackType == OniAttacks.StunAttack)
        {
            attackDescriptionText = attackDescriptionText.Replace("@", ((_stunAttackMinDmg + _currentDamageBuff) * Mathf.RoundToInt((100 + (float)_currentBerserkMuliplier) / 100)).ToString());
            attackDescriptionText = attackDescriptionText.Replace("#", ((_stunAttackMaxDmg + _currentDamageBuff) * Mathf.RoundToInt((100 + (float)_currentBerserkMuliplier) / 100)).ToString());
            attackDescriptionText = attackDescriptionText.Replace("$", _minShieldToBlockStun.ToString());
        }
        if (attackType == OniAttacks.BuffAttack)
        {
            attackDescriptionText = attackDescriptionText.Replace("@", _damageBuffPerUse.ToString());
        }

        return attackDescriptionText;
    }

    private void DealDamage(int damage)
    {
        damage += _currentDamageBuff;
        damage *= Mathf.RoundToInt((100 + (float)_currentBerserkMuliplier) / 100);
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
        if (playerHealth.GetPlayerShield() < _minShieldToBlockStun)
        {
            _combatManager.StunPlayer();
        }
        
        int damage = UnityEngine.Random.Range(_stunAttackMinDmg, _stunAttackMaxDmg);
        DealDamage(damage);
        
        Debug.Log("stunAttack");
    }

    private void BuffDamage()
    {
        _currentDamageBuff += _damageBuffPerUse;
        Debug.Log("buffDamage");
    }

    private void Berserk()
    {
        _enemyHealth.SetCurrentHealth(_enemyHealth.GetCurrentHealth()/(100 / _berserkHealthLossPercentage));
        _currentBerserkMuliplier = _berserkDamageBuffPercentage;
        Debug.Log("berserk");
    }
}

public enum OniAttacks { StrongAttack, StunAttack, BuffAttack, Berserk }