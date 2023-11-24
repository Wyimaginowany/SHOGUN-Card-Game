using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCard : AllTargetsCard
{
    protected override void PlayCard()
    {
        base.PlayCard();
        _combatManager.GivePlayerShield(CardData.Value);
    }
}
