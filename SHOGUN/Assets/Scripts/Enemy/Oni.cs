using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Oni : EnemyCombat
{
    [Header("Max Cooldowns")]
    [SerializeField] private int _strongHitMaxCD = 2;
    [SerializeField] private int _stunAttackMaxCD = 2;
    [SerializeField] private int _buffDamageMaxCD = 2;
    [SerializeField] private int _berserkMaxCD = 10000;
    
    [Header("Moves")]
    [SerializeField] private int _strongHitMinDmg = 12;
    [SerializeField] private int _strongHitMaxDmg = 20;
    [SerializeField] private int _stunAttackMinDmg = 6;
    [SerializeField] private int _stunAttackMaxDmg = 9;
    [SerializeField] private int _minShieldToBlockStun = 6;
    
    private int _strongHitCooldown;
    private int _stunAttackCooldown;
    private int _buffDamageCooldown;
    private int _berserkCooldown;
    private double _berserkMulti = 1;
    
    private EnemyHealth _enemyHealth;
    
    
    protected override void Start()
    {
        base.Start();
        _enemyHealth = GetComponent<EnemyHealth>();
        
    }
    public override void HandleTurn()
    {
        base.HandleTurn();
        List<string> availableAbilities = new List<string>();
        //if (_strongHitCooldown <= 0) availableAbilities.Add("strongHit");
        //if (_stunAttackCooldown <= 0) availableAbilities.Add("stunAttack");
        //if (_buffDamageCooldown <= 0) availableAbilities.Add("buffDamage");
        //if (_berserkCooldown <= 0) availableAbilities.Add("berserk");
        
        _strongHitCooldown--;
        _stunAttackCooldown--;
        _buffDamageCooldown--;
        
        int selectedIndex = Random.Range(0, availableAbilities.Count);
        //string selectedAbility = availableAbilities[selectedIndex];


        _animator.SetTrigger("attack");
        /*switch (selectedAbility)
        {
            case "strongHit":
                StrongHit();
                break;
            case "stunAttack":
                StunAttack();
                break;
            case "buffDamage":
                BuffDamage();
                break;
            case "berserk":
                Berserk();
                break;
            
        }*/
    }

    public void DefaultAttack()
    {
        playerHealth.TakeDamage(_damage);
    }

    private void StrongHit()
    {
        //_animator.SetTrigger("strongHit");
        int damage = (int)(Random.Range(_strongHitMinDmg, _strongHitMaxDmg) * _berserkMulti);
        playerHealth.TakeDamage(damage);

        _strongHitCooldown = _strongHitMaxCD;
        Debug.Log("strongHit");
    }

    private void StunAttack()
    {
        int damage = (int)(Random.Range(_stunAttackMinDmg, _stunAttackMaxDmg) * _berserkMulti);
        playerHealth.TakeDamage(damage);

        if (playerHealth.GetPlayerShield() < _minShieldToBlockStun)
        {
            //TODO: Stun player
            Debug.Log("stunPlayer");
        }
        
        _stunAttackCooldown = _stunAttackMaxCD;
        Debug.Log("stunAttack");
    }

    private void BuffDamage()
    {
        //TODO: buff dmg by 2 for the rest of the combat (effect stacks)
        _buffDamageCooldown = _buffDamageMaxCD;
        Debug.Log("buffDamage");
    }

    private void Berserk()
    {
        //TODO: decrease currentHealth by 50%, but increase damage by 30% for the rest of the combat (one time use)
        _enemyHealth.SetCurrentHealth(_enemyHealth.GetCurrentHealth()/2);
        _berserkMulti = 1.3;
        _berserkCooldown = _berserkMaxCD;
        Debug.Log("berserj");
    }
}