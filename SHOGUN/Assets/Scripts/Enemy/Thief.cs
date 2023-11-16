using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Thief : EnemyCombat
{
    [SerializeField] private int _basicAttackCooldown;
    [SerializeField] private int _comboAttackCooldown;
    [SerializeField] private int _blockActionCooldown;
    [SerializeField] private int _buffBlockCooldown;
    [SerializeField] private int _comboCounter = 1;
    [SerializeField] private double _damageMultiplier;
    
    private Animator _animator;

    // public enum BeltColor
    // {
    //     White=100,
    //     Orange=120,
    //     Blue=140,
    //     Yellow=160,
    //     Green=180,
    //     Brown=200,
    //     Black=220
    // }

    // public Thief(BeltColor beltColor)
    // {
    //     _damageMultiplier = (double) beltColor / 100;
    // }

    protected override void Start()
    {
        _animator = GetComponent<Animator>();

        base.Start();
        string[] beltColors = { "white", "orange", "blue", "yellow", "green", "brown", "black" };
        int[] beltDamages = { 100, 120, 140, 160, 180, 200, 220 };
        int randomIndex = Random.Range(0, beltColors.Length);
        _damageMultiplier = (double) beltDamages[randomIndex] / 100;
        Debug.Log(beltColors[randomIndex]);
        
        //TODO: SET ENEMY MODEL TO ACCORDING BELTCOLOR
    }
    
    public override void HandleTurn()
    {
        base.HandleTurn();
        List<string> availableAbilities = new List<string>();
        if (_basicAttackCooldown <= 0) availableAbilities.Add("basicAttack");
        if (_comboAttackCooldown <= 0) availableAbilities.Add("comboAttack");
        if (_blockActionCooldown <= 0) availableAbilities.Add("blockAction");
        if (_buffBlockCooldown <= 0) availableAbilities.Add("buffBlock");
        
        _basicAttackCooldown--;
        _comboAttackCooldown--;
        _blockActionCooldown--;
        _buffBlockCooldown--;
        
        int selectedIndex = Random.Range(0, availableAbilities.Count);
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
        _animator.SetTrigger("Kick");
        int damage = (int) Math.Round(Random.Range(3, 5) * _damageMultiplier);
        Debug.Log(playerHealth);
        playerHealth.TakeDamage(damage);
    }

    private void ComboAttack()
    {
        _animator.SetTrigger("Combo");
        int damage = (int) Math.Round(2 * _comboCounter * _damageMultiplier);
        Debug.Log(playerHealth);
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