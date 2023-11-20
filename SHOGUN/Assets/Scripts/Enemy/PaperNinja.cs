using System.Collections.Generic;
using UnityEngine;

public class PaperNinja : EnemyCombat
{
    [Header("Max Cooldowns")]
    [SerializeField] private int _shurikenThrowMaxCD = 0;
    [SerializeField] private int _paperCutMaxCD = 1;
    [SerializeField] private int _vanishMaxCD = 1;
    
    [Header("Moves")]
    [SerializeField] private int _shurikenThrowMinDmg = 3;
    [SerializeField] private int _shurikenThrowMaxDmg = 4;
    [SerializeField] private int _paperCutMinDmg = 1;
    
    private int _shurikenThrowCooldown;
    private int _paperCutCooldown;
    private int _vanishCooldown;
    
    public override void HandleTurn()
    {
        base.HandleTurn();
        List<string> availableAbilities = new List<string>();
        if (_shurikenThrowCooldown <= 0) availableAbilities.Add("ShurikenThrow");
        if (_paperCutCooldown <= 0) availableAbilities.Add("PaperCut");
        //if (_vanishCooldown <= 0) availableAbilities.Add("Vanish");
        
        _shurikenThrowCooldown--;
        _paperCutCooldown--;
        _vanishCooldown--;
        
        //int selectedIndex = Random.Range(0, availableAbilities.Count);
        //string selectedAbility = availableAbilities[selectedIndex];

        _animator.SetTrigger("ShurikenThrow");
        /*
        switch (selectedAbility)
        {
            case "ShurikenThrow":
                ShurikenThrow();
                break;
            case "PaperCut":
                PaperCut();
                break;
            case "Vanish":
                Vanish();
                break;
        }*/
    }

    public void DefaultAttack()
    {
        playerHealth.TakeDamage(_damage);
    }

    private void ShurikenThrow()
    {
        //int damage = Random.Range(_shurikenThrowMinDmg, _shurikenThrowMaxDmg);
        //playerHealth.TakeDamage(damage);

        _shurikenThrowCooldown = _shurikenThrowMaxCD;
        Debug.Log("shurikenThrow");
    }

    private void PaperCut()
    {
        int damage = _paperCutMinDmg;
        //int bleedStacks = Random.Range(2, 3);
        playerHealth.TakeDamage(damage);
        
        //TODO: playerEffect z bleedStacks
        
        _paperCutCooldown = _paperCutMaxCD;
        Debug.Log("PaperCut");
    }

    private void Vanish()
    {
        //TODO: nietykalnosc na 1 ture

        _vanishCooldown = _vanishMaxCD;
        Debug.Log("vanish");
    }
}