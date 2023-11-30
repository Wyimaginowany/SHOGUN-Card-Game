using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCard : AllTargetsCard
{
    protected override void PlayCard()
    {
        base.PlayCard();
        _combatManager.HealPlayer(CardData.Value);
    }
}
