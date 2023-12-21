using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PaperNinja : EnemyCombat
{ 
    [Header("Attacks")]
    [SerializeField] private List<EnemyAttack<PaperNinjaAttacks>> _paperNinjaPossibleAttacks = new List<EnemyAttack<PaperNinjaAttacks>>();
    [Space(5)]
    [Header("Basic Attack")]
    [SerializeField] private int _shurikenThrowMinDmg = 3;
    [SerializeField] private int _shurikenThrowMaxDmg = 5;
    [Space(5)]
    [Header("Combo Attack")]
    [SerializeField] private int _paperCutMinDmg = 2;
    
    private List<EnemyAttack<PaperNinjaAttacks>> _attacksPool = new List<EnemyAttack<PaperNinjaAttacks>>();
    private EnemyAttack<PaperNinjaAttacks> _chosenAttack;
    private EnemyHealth _enemyHealth;
    
    protected override void Start()
    {
        base.Start();
        _enemyHealth = GetComponent<EnemyHealth>();
    }
    public override void HandleTurn()
    {
        switch (_chosenAttack.Attack)
        {
            case PaperNinjaAttacks.ShurikenThrow:
                ShurikenThrow();
                break;
            case PaperNinjaAttacks.PaperCut:
                PaperCut();
                break;
            case PaperNinjaAttacks.Vanish:
                Vanish();
                break;
        }

        _turnTimeAmount = _chosenAttack.AttackDuration;
        _animator.SetTrigger(_chosenAttack.AnimatorTrigger);
        _chosenAttack.AttackCooldown = _chosenAttack.AttackMaxCooldown;
        if (_chosenAttack.RemoveAfterUsage) _paperNinjaPossibleAttacks.Remove(_chosenAttack);

        base.HandleTurn();
    }
    
    public override void PrepareAttack()
    {
        GetTurnAttack();
        DisplayEnemyIntentions();
    }
    private void GetTurnAttack()
    {
        _attacksPool = new List<EnemyAttack<PaperNinjaAttacks>>();

        foreach (EnemyAttack<PaperNinjaAttacks> enemyAttack in _paperNinjaPossibleAttacks)
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
        _attackDescriptionText.text = _chosenAttack.Description;
        AttackTypes attackType = _chosenAttack.AttackType;
        String iconName = (attackType + "_icon").ToLower();
        String iconPath = "Enemy Intention Icons/" + iconName;
        _iconGameObject.GetComponent<Image>().sprite = Resources.Load<Sprite> (iconPath);
    }

    private void ShurikenThrow()
    {
        int damage = Random.Range(_shurikenThrowMinDmg, _shurikenThrowMaxDmg);
        _combatManager.DealDamageToPlayer(damage);
        
        Debug.Log("shurikenThrow");
    }

    private void PaperCut()
    {
        int damage = _paperCutMinDmg;
        int bleedStacks = Random.Range(2, 3);
        _combatManager.DealDamageToPlayer(damage);
        _combatManager.AddBleedStacksToPlayer(bleedStacks);
        
        Debug.Log("PaperCut");
    }

    private void Vanish()
    {
        //TODO: nietykalnosc na 1 ture
        _enemyHealth.MakeUntargetable();
        Debug.Log("vanish");
    }
}

public enum PaperNinjaAttacks { ShurikenThrow, PaperCut, Vanish }