using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainManaCard : AllTargetsCard
{
    protected override void PlayCard()
    {
        base.PlayCard();
        _combatManager.IncreaseCurrentMana(CardData.Value);
    }
}
