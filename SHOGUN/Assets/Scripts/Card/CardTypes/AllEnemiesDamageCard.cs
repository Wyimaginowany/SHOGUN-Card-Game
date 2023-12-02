using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllEnemiesDamageCard : AllTargetsCard
{
    protected override void Start()
    {
        base.Start();
        _combatManager.OnDamageCardBuff += HandleBuffRecived;
    }

    private void OnDestroy()
    {
        //to wyrzuca b³¹d
        //_combat manager nie jest przypisany do trzech kart do wyboru 
        if (gameObject.activeSelf) _combatManager.OnDamageCardBuff -= HandleBuffRecived;
    }

    private void HandleBuffRecived(int buffAmount)
    {
        _currentCardValue += buffAmount;
        _cardVisual.UpdateCardValueVisual(buffAmount);
    }

    protected override void PlayCard()
    {
        base.PlayCard();
        _combatManager.DealDamageToAllEnemies(_currentCardValue);
    }
}
