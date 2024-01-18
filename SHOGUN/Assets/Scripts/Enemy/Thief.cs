using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Thief : EnemyCombat
{
    [Header("Attacks")]
    [SerializeField] private List<EnemyAttack<ThiefAttacks>> _thiefPossibleAttacks = new List<EnemyAttack<ThiefAttacks>>();
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
    //private double _damageMultiplier;
    private double _currentBlockMultiplier = 1.0;
    private List<EnemyAttack<ThiefAttacks>> _attacksPool = new List<EnemyAttack<ThiefAttacks>>();
    private EnemyAttack<ThiefAttacks> _chosenAttack;
    
    private EnemyHealth _enemyHealth;
    
    protected override void Start()
    {
        base.Start();
        _enemyHealth = GetComponent<EnemyHealth>();
        //string[] beltColors = { "white", "orange", "blue", "yellow", "green", "brown", "black" };
        //int[] beltDamages = { 100, 120, 140, 160, 180, 200, 220 };
        //int randomIndex = UnityEngine.Random.Range(0, beltColors.Length);
        //_damageMultiplier = (double) beltDamages[randomIndex] / 100;
        //Debug.Log("Belt color: " + beltColors[randomIndex]);
        
        //TODO: SET ENEMY MODEL TO ACCORDING BELTCOLOR
    }
    
    public override void HandleTurn()
    {
        switch (_chosenAttack.Attack)
        {
            case ThiefAttacks.BasicAttack:
                BasicAttack();
                break;
            case ThiefAttacks.ComboAttack:
                ComboAttack();
                break;
            case ThiefAttacks.BlockAction:
                BlockAction();
                break;
            case ThiefAttacks.BuffBlock:
                BuffBlock();
                break;           
        }

        _turnTimeAmount = _chosenAttack.AttackDuration;
        _animator.SetTrigger(_chosenAttack.AnimatorTrigger);
        _chosenAttack.AttackCooldown = _chosenAttack.AttackMaxCooldown;
        if (_chosenAttack.RemoveAfterUsage) _thiefPossibleAttacks.Remove(_chosenAttack);

        base.HandleTurn();
    }

    public override void PrepareAttack()
    {
        GetTurnAttack();
        DisplayEnemyIntentions();
    }

    private void GetTurnAttack()
    {
        _attacksPool = new List<EnemyAttack<ThiefAttacks>>();

        foreach (EnemyAttack<ThiefAttacks> enemyAttack in _thiefPossibleAttacks)
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

    private string ChangeAttackDescription(ThiefAttacks attackType)
    {
        string attackDescriptionText = _chosenAttack.Description;

        if (attackType == ThiefAttacks.BasicAttack)
        {
            attackDescriptionText = attackDescriptionText.Replace("@", _basicAttackMinDmg.ToString());
            attackDescriptionText = attackDescriptionText.Replace("#", _basicAttackMaxDmg.ToString());
        }
        if (attackType == ThiefAttacks.ComboAttack)
        {
            attackDescriptionText = attackDescriptionText.Replace("@", (_comboAttackMinDmg * _comboCounter).ToString());
        }
        if (attackType == ThiefAttacks.BlockAction)
        {
            attackDescriptionText = attackDescriptionText.Replace("@", ((int)Math.Round(_blockActionValue * _currentBlockMultiplier)).ToString());
        }
        if (attackType == ThiefAttacks.BuffBlock)
        {
            attackDescriptionText = attackDescriptionText.Replace("@", ((int)Math.Round(_blockActionValue * _currentBlockMultiplier)).ToString());
        }

        return attackDescriptionText;
    }

    private void DealDamage(int damage)
    {
        _combatManager.DealDamageToPlayer(damage);
    }
    
    private void BasicAttack()
    {
        int damage = UnityEngine.Random.Range(_basicAttackMinDmg, _basicAttackMaxDmg);
        DealDamage(damage);
        Debug.Log("basicAttack");
    }

    private void ComboAttack()
    {
        int damage = _comboAttackMinDmg * _comboCounter;
        DealDamage(damage);
        _comboCounter++;
        Debug.Log("comboAttack");
    }

    private void BlockAction()
    {
        double block = _blockActionValue;
        block *= _currentBlockMultiplier;
        _enemyHealth.GiveShield((int)Math.Round(block));
        
        Debug.Log("blockAction: " + block);
    }

    private void BuffBlock()
    {
        BlockAction();
        if (_currentBlockMultiplier == 1.0) _currentBlockMultiplier = 0.0;
        _currentBlockMultiplier += (100 + _blockBuffPercentage) / 100.0;
        Debug.Log("buffBlock: " + _currentBlockMultiplier);
    }
}
public enum ThiefAttacks { BasicAttack, ComboAttack, BlockAction ,BuffBlock}