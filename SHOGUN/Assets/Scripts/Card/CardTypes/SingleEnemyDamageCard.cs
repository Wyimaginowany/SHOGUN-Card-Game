using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleEnemyDamageCard : SingleTargetCard
{
    protected override void Start()
    {
        base.Start();
        _combatManager.OnDamageCardBuff += HandleBuffRecived;
    }

    private void OnDestroy()
    {
        //to wyrzuca b��d
        //_combat manager nie jest przypisany do trzech kart do wyboru 
        if (gameObject.activeSelf) _combatManager.OnDamageCardBuff -= HandleBuffRecived;
    }

    private void HandleBuffRecived(int buffAmount)
    {
        _currentCardValue += buffAmount;
        _cardVisual.UpdateCardValueVisual(buffAmount);
        _cardVisual.UpdateAttackValueVisual();
    }

    protected override void PlayCardOnTarget(EnemyHealth enemyHealth)
    {
        base.PlayCardOnTarget(enemyHealth);
        _combatManager.DealDamageToEnemy(enemyHealth, _currentCardValue);
    }
}
