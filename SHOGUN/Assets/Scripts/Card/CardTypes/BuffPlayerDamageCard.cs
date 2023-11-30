using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPlayerDamageCard : AllTargetsCard
{
    protected override void PlayCard()
    {
        base.PlayCard();
        _combatManager.BuffPlayerDamage(CardData.Value);
    }
}
