using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AllTargetsCard : Card
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

        if (!_combatManager.HaveEnoughMana(GetCardCost()))
        {
            ReturnCardToHand();
            return;
        }

        if (possibleDropAreas.Contains(PossibleAreas.PlayArea))
        {
            PlayCard();
            return;
        }

        ReturnCardToHand();
    }

    protected override void PlayCard()
    {
        base.PlayCard();
    }
}
