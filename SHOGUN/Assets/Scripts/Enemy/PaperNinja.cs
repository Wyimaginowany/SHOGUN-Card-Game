using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PaperNinja : EnemyCombat
{
    [SerializeField] private int _shurikenThrowCooldown;
    [SerializeField] private int _paperCutCooldown;
    [SerializeField] private int _vanishCooldown;
    
    public override void HandleTurn()
    {
        base.HandleTurn();
        List<string> availableAbilities = new List<string>();
        if (_shurikenThrowCooldown <= 0) availableAbilities.Add("ShurikenThrow");
        if (_paperCutCooldown <= 0) availableAbilities.Add("PaperCut");
        if (_vanishCooldown <= 0) availableAbilities.Add("Vanish");
        
        _shurikenThrowCooldown--;
        _paperCutCooldown--;
        _vanishCooldown--;
        
        int selectedIndex = Random.Range(0, availableAbilities.Count);
        string selectedAbility = availableAbilities[selectedIndex];
        
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
        }
    }
    
    private void ShurikenThrow()
    {
        int damage = Random.Range(3, 4);
        Debug.Log(playerHealth);
        playerHealth.TakeDamage(damage);
    }

    private void PaperCut()
    {
        int damage = 1;
        int bleedStacks = Random.Range(2, 3);
        playerHealth.TakeDamage(damage);
        
        //TODO: playerEffect z bleedStacks
        
        _paperCutCooldown = 1;
    }

    private void Vanish()
    {
        //TODO: nietykalnosc na 1 ture

        _vanishCooldown = 5;
    }
}