using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleEnemyDamageCard : SingleTargetCard
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
        _cardVisual.UpdateVisual();
    }

    protected override void PlayCardOnTarget(EnemyHealth enemyHealth)
    {
        base.PlayCardOnTarget(enemyHealth);
        _combatManager.DealDamageToEnemy(enemyHealth, _currentCardValue);
    }
}
