using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [Header("To Attach")]
    [SerializeField] private TMP_Text _deckAmountText;
    [SerializeField] private TMP_Text _usedCardsAmountText;

    [Space(15)]
    //serialize to see in editor remove later
    [SerializeField] private List<Card> _deck = new List<Card>();
    [SerializeField] private List<Card> _usedCards = new List<Card>();

    private void Start()
    {
        Card.OnCardPlayed += HandleCardPlayed;

        _deckAmountText.text = _deck.Count.ToString();
        _usedCardsAmountText.text = _usedCards.Count.ToString();
    }

    private void OnDestroy()
    {
        Card.OnCardPlayed -= HandleCardPlayed;
    }

    private void HandleCardPlayed(Card playedCard, int x)
    {
        _usedCards.Add(playedCard);
        _usedCardsAmountText.text = _usedCards.Count.ToString();
    }

    public Card DrawTopCard()
    {
        if (_deck.Count <= 0)
        {
            ShuffleDeck();
        }

        Card cardOnTop = _deck[0];
        _deck.Remove(cardOnTop);
        _deckAmountText.text = _deck.Count.ToString();
        _usedCardsAmountText.text = _usedCards.Count.ToString();

        return cardOnTop;
    }

    private void ShuffleDeck()
    {
        foreach (Card usedCard in _usedCards)
        {
            _deck.Add(usedCard);
        }

        _usedCards.Clear();

        for (int i = 0; i < _deck.Count - 1; i++)
        {
            Card temporaryCard = _deck[i];
            int randomIndex = Random.Range(i, _deck.Count);
            _deck[i] = _deck[randomIndex];
            _deck[randomIndex] = temporaryCard;
        }
    }
}
