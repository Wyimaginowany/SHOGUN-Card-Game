using System;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    [Header("To Attach")]
    [SerializeField] private Transform _handCardsParent;
    [Space(15)]
    [Header("Position Settings")]
    [SerializeField] private int _startMaxHandSize = 4;
    [SerializeField] private float _cardsInHandPositionY = 90f;
    [SerializeField] private float _spaceBetweenCards = 100f;
    [SerializeField] private float _maxSpaceBetweenCards = 100f;
    [SerializeField] private float _minSpaceBetweenCards = 0f;
    [SerializeField] private float _spaceBetweenCardsGrowFactor = 0.5f;
    [Space(15)]
    [Header("Rotation Settings")]
    [SerializeField] private float _rotationPerCard = 1f;


    [SerializeField] private List<Card> _hand = new List<Card>(); //serialize to see in editor remove later
    private float _middlePositionX;
    private DeckManager _deckManager;
    private RectTransform _rectTransform;
    private int _currentMaxHandSize;


    private void Start()
    {
        _deckManager = GetComponent<DeckManager>();
        _rectTransform = GetComponent<RectTransform>();
        _middlePositionX = _rectTransform.rect.width / 2;
        _currentMaxHandSize = _startMaxHandSize;

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
        for (int i = _hand.Count; i < _currentMaxHandSize; i++)
        {
            Card drawnCard = _deckManager.DrawTopCard();
            _hand.Add(drawnCard);
        }
        UpdateCardsPosition(_hand.Count);
        UpdateCardsRotation(_hand.Count);
    }

    private void RemoveFromHand(Card cardPlayed)
    {
        _hand.Remove(cardPlayed);
        UpdateCardsPosition(_hand.Count);
        UpdateCardsRotation(_hand.Count);
    }

    private void UpdateCardsPosition(int newCardsAmount)
    {
        for (int i = 0; i < newCardsAmount; i++)
        {
            float spaceFactor =  Mathf.Clamp(1 /(_hand.Count * _spaceBetweenCardsGrowFactor), _minSpaceBetweenCards, _maxSpaceBetweenCards);
            float cardNewPositionX = _middlePositionX + (_spaceBetweenCards * (2 * i - _hand.Count + 1) / (1 / spaceFactor));
            Vector2 newCardPosition = new Vector2(cardNewPositionX, _cardsInHandPositionY);

            _hand[i].gameObject.transform.SetSiblingIndex(i);
            _hand[i].SetNewHandPosition(newCardPosition);
        }
        Debug.Log("cards in hand:" + _hand.Count);
    }

    private void UpdateCardsRotation(int newCardsAmount)
    {
        bool isEvenNumber = newCardsAmount % 2 == 0;
        int cardIndex = Mathf.FloorToInt(newCardsAmount / 2);

        for (int i = 0; i < newCardsAmount; i++)
        {
            if (isEvenNumber && Mathf.FloorToInt(newCardsAmount / 2) - i == 0)
            {
                cardIndex--;
            }
            Vector3 newCardRotation = new Vector3(0f, 0f, (cardIndex - i) * _rotationPerCard);
            _hand[i].SetNewHandRotation(newCardRotation);
        }
    }

    private void Test(int handSize)
    {
        for (int i = 0; i < handSize; i++)
        {
            Debug.Log(i + " " + (-(((_spaceBetweenCards / 2) * (handSize - 1)) - (2 * i * (_spaceBetweenCards / 2)))));
        }
    }

    public void StartButton()
    {
        DrawFullHand();
    }
}
