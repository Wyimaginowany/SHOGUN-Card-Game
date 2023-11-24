using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceCardsCostCard : AllTargetsCard
{
    protected override void PlayCard()
    {
        base.PlayCard();
        _combatManager.ReduceCardsCost(CardData.Value);
    }
}