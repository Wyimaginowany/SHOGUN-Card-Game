using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Thief : EnemyCombat
{
    private readonly Random _rnd = new();
    [SerializeField] private int _basicAttackCooldown;
    [SerializeField] private int _comboAttackCooldown;
    [SerializeField] private int _blockActionCooldown;
    [SerializeField] private int _buffBlockCooldown;
    [SerializeField] private int _comboCounter = 1;
    [SerializeField] private double _damageMultiplier;
    
    public enum BeltColor
    {
        White=100,
        Orange=120,
        Blue=140,
        Yellow=160,
        Green=180,
        Brown=200,
        Black=220
    }

    public Thief(BeltColor beltColor)
    {
        _damageMultiplier = (double) beltColor / 100;
    }
    
    protected override void HandleTurn()
    {
        List<string> availableAbilities = new List<string>();
        if (_basicAttackCooldown <= 0) availableAbilities.Add("basicAttack");
        if (_comboAttackCooldown <= 0) availableAbilities.Add("comboAttack");
        if (_blockActionCooldown <= 0) availableAbilities.Add("blockAction");
        if (_buffBlockCooldown <= 0) availableAbilities.Add("buffBlock");
        
        _basicAttackCooldown--;
        _comboAttackCooldown--;
        _blockActionCooldown--;
        _buffBlockCooldown--;
        
        int selectedIndex = _rnd.Next(availableAbilities.Count);
        string selectedAbility = availableAbilities[selectedIndex];
        
        switch (selectedAbility)
        {
            case "basicAttack":
                BasicAttack();
                break;
            case "comboAttack":
                ComboAttack();
                break;
            case "blockAction":
                BlockAction();
                break;
            case "buffBlock":
                BuffBlock();
                break;
        }
    }
    
    private void BasicAttack()
    {
        int damage = (int) Math.Round(_rnd.Next(3, 5) * _damageMultiplier);
        playerHealth.TakeDamage(damage);
    }

    private void ComboAttack()
    {
        int damage = (int) Math.Round(2 * _comboCounter * _damageMultiplier);
        playerHealth.TakeDamage(damage);
        
        _comboAttackCooldown = 1;
    }

    private void BlockAction()
    {
        //TODO: give 5 block

        _blockActionCooldown = 1;
    }

    private void BuffBlock()
    {
        //TODO: block multi 150% for the rest of the combat

        _buffBlockCooldown = 5;
    }
}