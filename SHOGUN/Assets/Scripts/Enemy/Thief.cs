using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Thief : EnemyCombat
{
    [Header("Max Cooldowns")]
    [SerializeField] private int _basicAttackMaxCD = 0;
    [SerializeField] private int _comboAttackMaxCD = 1;
    [SerializeField] private int _blockActionMaxCD = 1;
    [SerializeField] private int _buffBlockMaxCD = 5;
    [Header("Moves")]
    [SerializeField] private int _basicAttackMinDmg = 3;
    [SerializeField] private int _basicAttackMaxDmg = 5;
    [SerializeField] private int _comboAttackMinDmg = 2;
    [SerializeField] private int _blockActionValue = 5;
    

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
    private int _basicAttackCooldown;
    private int _comboAttackCooldown;
    private int _blockActionCooldown;
    private int _buffBlockCooldown;
    private int _comboCounter = 1;
    private double _damageMultiplier;
    
    private EnemyHealth _enemyHealth;


    protected override void Start()
    {
        _animator = GetComponent<Animator>();

        base.Start();
        _enemyHealth = GetComponent<EnemyHealth>();
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
        Debug.Log(playerHealth);
        int damage = (int) Math.Round(Random.Range(_basicAttackMinDmg, _basicAttackMaxDmg) * _damageMultiplier);
        playerHealth.TakeDamage(damage);

        _basicAttackCooldown = _basicAttackMaxCD;
        Debug.Log("basicAttack");
    }

    private void ComboAttack()
    {
        _animator.SetTrigger("Combo");
        int damage = (int) Math.Round(_comboAttackMinDmg * _comboCounter * _damageMultiplier);
        Debug.Log(playerHealth);
        playerHealth.TakeDamage(damage);
        
        _comboAttackCooldown = _comboAttackMaxCD;
        Debug.Log("comboAttack");
    }

    private void BlockAction()
    {
        //TODO: give 5 block
        _enemyHealth.GiveShield(_blockActionValue);

        _blockActionCooldown = _blockActionMaxCD;
        Debug.Log("blockAction");
    }

    private void BuffBlock()
    {
        //TODO: block multi 150% for the rest of the combat

        _buffBlockCooldown = _buffBlockMaxCD;
        Debug.Log("buffBlock");
    }
}