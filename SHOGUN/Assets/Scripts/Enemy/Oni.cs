using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Oni : EnemyCombat
{
    private readonly Random _rnd = new();
    [SerializeField] private int _strongHitCooldown;
    [SerializeField] private int _stunAttackCooldown;
    [SerializeField] private int _buffDamageCooldown;
    [SerializeField] private int _berserkCooldown;
    
    protected override void HandleTurn()
    {
        List<string> availableAbilities = new List<string>();
        if (_strongHitCooldown <= 0) availableAbilities.Add("strongHit");
        if (_stunAttackCooldown <= 0) availableAbilities.Add("stunAttack");
        if (_buffDamageCooldown <= 0) availableAbilities.Add("buffDamage");
        if (_berserkCooldown <= 0) availableAbilities.Add("berserk");
        
        _strongHitCooldown--;
        _stunAttackCooldown--;
        _buffDamageCooldown--;
        
        int selectedIndex = _rnd.Next(availableAbilities.Count);
        string selectedAbility = availableAbilities[selectedIndex];
        
        switch (selectedAbility)
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
        }
    }
    
    private void StrongHit()
    {
        int damage = _rnd.Next(12, 20);
        playerHealth.TakeDamage(damage);

        _strongHitCooldown = 2;
    }

    private void StunAttack()
    {
        int damage = _rnd.Next(6, 9);
        playerHealth.TakeDamage(damage);
        
        //TODO: stun if player doesnt have enough block to block all damage
        
        _stunAttackCooldown = 2;
    }

    private void BuffDamage()
    {
        //TODO: buff dmg by 2 for the rest of the combat (effect stacks)
        _buffDamageCooldown = 1;
    }

    private void Berserk()
    {
        //TODO: decrease currentHealth by 50%, but increase damage by 30% for the rest of the combat (one time use)

        _berserkCooldown++;
    }
}