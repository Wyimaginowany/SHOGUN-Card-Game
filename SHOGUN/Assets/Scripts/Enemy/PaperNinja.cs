using System.Collections.Generic;
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

    [System.Serializable]
    public class EnemyAttack
    {
        public PaperNinjaAttackTypes AttackType;
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

        foreach (EnemyAttack enemyAttack in _paperNinjaPossibleAttacks)
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
        paperNinjaAttackSelected.AttackCooldown = paperNinjaAttackSelected.AttackCooldownAtStart;
        if (paperNinjaAttackSelected.RemoveAfterUsage) _paperNinjaPossibleAttacks.Remove(paperNinjaAttackSelected);

        base.HandleTurn();
    }

    private void ShurikenThrow()
    {
        int damage = Random.Range(_shurikenThrowMinDmg, _shurikenThrowMaxDmg);
        playerHealth.TakeDamage(damage);
        
        Debug.Log("shurikenThrow");
    }

    private void PaperCut()
    {
        int damage = _paperCutMinDmg;
        int bleedStacks = Random.Range(2, 3);
        playerHealth.TakeDamage(damage);
        
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