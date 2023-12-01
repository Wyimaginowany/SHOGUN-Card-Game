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
        if (gameObject.activeSelf) _combatManager.OnDamageCardBuff -= HandleBuffRecived;
    }

    private void HandleBuffRecived(int buffAmount)
    {
        _currentCardValue += buffAmount;
        _cardVisual.UpdateCardValueVisual(buffAmount);
    }

    protected override void PlayCardOnTarget(EnemyHealth enemyHealth)
    {
        base.PlayCardOnTarget(enemyHealth);
        _combatManager.DealDamageToEnemy(enemyHealth, _currentCardValue);
    }
}
