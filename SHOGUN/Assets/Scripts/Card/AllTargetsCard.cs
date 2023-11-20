using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllTargetsCard : Card
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void BeginDragging()
    {
        base.BeginDragging();
    }

    protected override void OnBeeingDragged()
    {
        base.OnBeeingDragged();
    }

    protected override void EndDragging()
    {
        base.EndDragging();

        List<PossibleAreas> possibleDropAreas = GetDropAreas();

        if (possibleDropAreas.Contains(PossibleAreas.ThrowOutArea))
        {
            ShuffleCardIntoDeck();
            return;
        }

        if (!_combatManager.HaveEnoughMana(_cardData.Cost))
        {
            ReturnCardToHand();
            return;
        }

        if (possibleDropAreas.Contains(PossibleAreas.PlayArea))
        {
            _combatManager.DealDamageToAllEnemies(_cardData.Value);
            PlayCard();
            return;
        }

        ReturnCardToHand();
    }
}
