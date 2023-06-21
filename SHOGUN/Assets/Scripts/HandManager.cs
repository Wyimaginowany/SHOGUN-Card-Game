using System;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    [Header("To Attach")]
    [SerializeField] private GameObject[] _cardSlots;

    public static event Action<Card, int> OnCardDrawn;

    private DeckManager _deckManager;
    [SerializeField] private List<Card> _hand = new List<Card>(); //serialize to see in editor remove later

    private void Start()
    {
        _deckManager = GetComponent<DeckManager>();

        CombatManager.OnPlayerTurnStart += HandlePlayerTurnStart;
        Card.OnCardPlayed += RemoveFromHand;
        Card.OnCardThrownAway += RemoveFromHand;
    }

    private void OnDestroy()
    {
        CombatManager.OnPlayerTurnStart -= HandlePlayerTurnStart;
        Card.OnCardPlayed -= RemoveFromHand;
        Card.OnCardThrownAway -= RemoveFromHand;
    }
    private void HandlePlayerTurnStart()
    {
        DrawFullHand();
    }

    private void DrawFullHand()
    {
        for (int i = 0; i < _cardSlots.Length; i++)
        {
            if (!_cardSlots[i].activeSelf) continue;
            Card drawnCard = _deckManager.DrawTopCard();

            _hand.Add(drawnCard);

            drawnCard.gameObject.transform.position = _cardSlots[i].transform.position;
            _cardSlots[i].SetActive(false);

            OnCardDrawn?.Invoke(drawnCard, i);
        }
    }

    private void RemoveFromHand(Card cardPlayed, int cardPlayerSlotIndex)
    {
        _hand.Remove(cardPlayed);
        _cardSlots[cardPlayerSlotIndex].SetActive(true);
    }

    public void StartButton()
    {
        DrawFullHand();
    }
}
