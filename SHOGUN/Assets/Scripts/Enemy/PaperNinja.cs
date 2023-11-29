using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        _attackIntentionText.text = _chosenAttack.AttackType.ToString(); //zamiast tego ikonka miecz/shield/cos tam na buffa/cos na debuffa
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
        
        //TODO: playerEffect z bleedStacks
        
        Debug.Log("PaperCut");
    }

    private void Vanish()
    {
        //TODO: nietykalnosc na 1 ture
        
        Debug.Log("vanish");
    }
}

public enum PaperNinjaAttacks { ShurikenThrow, PaperCut, Vanish }