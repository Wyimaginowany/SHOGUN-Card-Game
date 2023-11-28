using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PaperNinja : EnemyCombat
{
    [Header("Attacks")]
    [SerializeField] private List<EnemyAttack> _paperNinjaPossibleAttacks = new List<EnemyAttack>();
    [Space(5)]
    [Header("Basic Attack")]
    [SerializeField] private int _shurikenThrowMinDmg = 3;
    [SerializeField] private int _shurikenThrowMaxDmg = 5;
    [Space(5)]
    [Header("Combo Attack")]
    [SerializeField] private int _paperCutMinDmg = 2;
    
    private List<EnemyAttack> _attacksPool = new List<EnemyAttack>();

    [System.Serializable]
    public class EnemyAttack
    {
        public PaperNinjaAttackTypes AttackType;
        public int AttackPriority = 0;
        public int AttackCooldown = 0;
        public int AttackMaxCooldown = 2;
        public string AnimatorTrigger = "";
        public float AttackDuration = 2f;
    }
    
    private EnemyAttack GetTurnAttack()
    {
        EnemyAttack chosenAttack;
        _attacksPool = new List<EnemyAttack>();

        foreach (EnemyAttack enemyAttack in _paperNinjaPossibleAttacks)
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

        chosenAttack = _attacksPool.ElementAt(UnityEngine.Random.Range(0, _attacksPool.Count));
        chosenAttack.AttackCooldown++;
        return chosenAttack;
    }
    
    public override void HandleTurn()
    {
        EnemyAttack paperNinjaAttackSelected = GetTurnAttack();

        switch (paperNinjaAttackSelected.AttackType)
        {
            case PaperNinjaAttackTypes.ShurikenThrow:
                ShurikenThrow();
                break;
            case PaperNinjaAttackTypes.PaperCut:
                PaperCut();
                break;
            case PaperNinjaAttackTypes.Vanish:
                Vanish();
                break;
        }

        _turnTimeAmount = paperNinjaAttackSelected.AttackDuration;
        _animator.SetTrigger(paperNinjaAttackSelected.AnimatorTrigger);
        paperNinjaAttackSelected.AttackCooldown = paperNinjaAttackSelected.AttackMaxCooldown;

        base.HandleTurn();
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

public enum PaperNinjaAttackTypes { ShurikenThrow, PaperCut, Vanish}