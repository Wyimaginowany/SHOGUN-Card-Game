using System;
using System.Collections.Generic;
using UnityEngine;

public class Thief : EnemyCombat
{
    [Header("Attacks")]
    [SerializeField] private List<EnemyAttack> _thiefPossibleAttacks = new List<EnemyAttack>();
    [Space(5)]
    [Header("Basic Attack")]
    [SerializeField] private int _basicAttackMinDmg = 3;
    [SerializeField] private int _basicAttackMaxDmg = 5;
    [Space(5)]
    [Header("Combo Attack")]
    [SerializeField] private int _comboAttackMinDmg = 2;
    [Space(5)]
    [Header("Block Move")]
    [SerializeField] private int _blockActionValue = 5;
    [Space(5)]
    [Header("Block buff")]
    [SerializeField] private int _blockBuffPercentage = 50;
    
    private int _comboCounter = 1;
    private double _damageMultiplier;
    
    private EnemyHealth _enemyHealth;

    [System.Serializable]
    public class EnemyAttack
    {
        public ThiefAttackTypes AttackType;
        public int AttackPriority = 0;
        public int AttackCooldown = 2;
        public int AttackCooldownAtStart = 2;
        public bool RemoveAfterUsage = false;
        public string AnimatorTrigger = "";
        public float AttackDuration = 2f;
    }
    
    private EnemyAttack GetTurnAttack()
    {
        EnemyAttack chosenAttack = null;
        int enemyPriority = -1;

        foreach (EnemyAttack enemyAttack in _thiefPossibleAttacks)
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

    protected override void Start()
    {
        base.Start();
        _enemyHealth = GetComponent<EnemyHealth>();
        string[] beltColors = { "white", "orange", "blue", "yellow", "green", "brown", "black" };
        int[] beltDamages = { 100, 120, 140, 160, 180, 200, 220 };
        int randomIndex = UnityEngine.Random.Range(0, beltColors.Length);
        _damageMultiplier = (double) beltDamages[randomIndex] / 100;
        Debug.Log("Belt color: " + beltColors[randomIndex]);
        
        //TODO: SET ENEMY MODEL TO ACCORDING BELTCOLOR
    }
    
    public override void HandleTurn()
    {
        EnemyAttack thiefAttackSelected = GetTurnAttack();

        switch (thiefAttackSelected.AttackType)
        {
            case ThiefAttackTypes.BasicAttack:
                BasicAttack();
                break;
            case ThiefAttackTypes.ComboAttack:
                ComboAttack();
                break;
            case ThiefAttackTypes.BlockAction:
                BlockAction();
                break;
            case ThiefAttackTypes.BuffBlock:
                BuffBlock();
                break;           
        }

        _turnTimeAmount = thiefAttackSelected.AttackDuration;
        _animator.SetTrigger(thiefAttackSelected.AnimatorTrigger);
        thiefAttackSelected.AttackCooldown = thiefAttackSelected.AttackCooldownAtStart;
        if (thiefAttackSelected.RemoveAfterUsage) _thiefPossibleAttacks.Remove(thiefAttackSelected);

        base.HandleTurn();
    }
    
    private void DealDamageToPlayer(int damage)
    {
        damage *= (int) Math.Round(100 * _damageMultiplier);
        playerHealth.TakeDamage(damage);
    }
    
    private void BasicAttack()
    {
        int damage = (int) Math.Round(UnityEngine.Random.Range(_basicAttackMinDmg, _basicAttackMaxDmg) * _damageMultiplier);
        DealDamageToPlayer(damage);
        Debug.Log("basicAttack");
    }

    private void ComboAttack()
    {
        int damage = (int) Math.Round(_comboAttackMinDmg * _comboCounter * _damageMultiplier);
        DealDamageToPlayer(damage);
        Debug.Log("comboAttack");
    }

    private void BlockAction()
    {
        //TODO: give 5 block
        _enemyHealth.GiveShield(_blockActionValue);
        
        Debug.Log("blockAction");
    }

    private void BuffBlock()
    {
        //TODO: block multi 150% for the rest of the combat
        
        Debug.Log("buffBlock");
    }
}
public enum ThiefAttackTypes { BasicAttack, ComboAttack, BlockAction ,BuffBlock}