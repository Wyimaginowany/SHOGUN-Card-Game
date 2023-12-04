using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllEnemiesDamageCard : AllTargetsCard
{
    protected override void Start()
    {
        base.Start();
        CombatManager.OnOneTurnDamageCardBuff += HandleOneTurnBuffRecived;
        CombatManager.OnPermanentDamageCardBuff += HandleBuffRecived;
    }

    private void OnDestroy()
    {
        CombatManager.OnOneTurnDamageCardBuff -= HandleOneTurnBuffRecived;
        CombatManager.OnPermanentDamageCardBuff -= HandleBuffRecived;
    }

    private void HandleOneTurnBuffRecived(int buffAmount)
    {
        _thisTurnCardValueBuff += buffAmount;
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
