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
        CombatManager.OnPermanentDamageCardBuff += HandlePermenentBuffRecived;
        CombatManager.OnOneTurnDamageCardDebuff += HandleOneTurnDebuffRecived;
        CombatManager.OnPermanentDamageCardDebuff += HandlePermenentDebuffRecived;
    }

    private void OnDestroy()
    {
        CombatManager.OnOneTurnDamageCardBuff -= HandleOneTurnBuffRecived;
        CombatManager.OnPermanentDamageCardBuff -= HandlePermenentBuffRecived;
        CombatManager.OnOneTurnDamageCardDebuff -= HandleOneTurnDebuffRecived;
        CombatManager.OnPermanentDamageCardDebuff -= HandlePermenentDebuffRecived;
    }

    protected override void PlayCard()
    {
        base.PlayCard();
        _combatManager.DealDamageToAllEnemies(_currentCardValue);
    }

    private void HandleOneTurnBuffRecived(int buffAmount)
    {
        _thisTurnCardValueBuff += buffAmount;
        _cardVisual.UpdateVisual();
    }

    private void HandlePermenentBuffRecived(int buffAmount)
    {
        _currentCardValue += buffAmount;
        _cardVisual.UpdateVisual();
    }

    private void HandlePermenentDebuffRecived(int debuffAmount)
    {
        _currentCardValue -= debuffAmount;
        _cardVisual.UpdateVisual();
    }

    private void HandleOneTurnDebuffRecived(int debuffAmount)
    {
        _thisTurnCardValueBuff -= debuffAmount;
        _cardVisual.UpdateVisual();
    }

}
