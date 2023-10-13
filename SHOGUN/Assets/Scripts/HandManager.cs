using System;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    [Header("To Attach")]
    [SerializeField] private Transform _handCardsParent;
    [Space(15)]
    [Header("Position Settings")]
    [SerializeField] private int _startMaxHandSize = 10;
    [SerializeField] private float _middlePositionY = 100f;
    [SerializeField] private float _positionPerCardAmplitudeY = 3.5f;
    [SerializeField] private float _spaceBetweenCards = 600f;
    [SerializeField] private float _maxSpaceBetweenCards = 0.15f;
    [SerializeField] private float _minSpaceBetweenCards = 0f;
    [SerializeField] private float _spaceBetweenCardsGrowFactor = 1.5f;
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
        bool isEvenNumber = newCardsAmount % 2 == 0;
        int halfDeckAmount = Mathf.FloorToInt(newCardsAmount / 2);

        for (int i = 0; i < newCardsAmount; i++)
        {
            float spaceFactor =  Mathf.Clamp(1 /(_hand.Count * _spaceBetweenCardsGrowFactor), _minSpaceBetweenCards, _maxSpaceBetweenCards);
            float cardNewPositionX = _middlePositionX + (_spaceBetweenCards * (2 * i - _hand.Count + 1) / (1 / spaceFactor));
            float cardNewPositionY = (i - halfDeckAmount) * _positionPerCardAmplitudeY;

            if (i == halfDeckAmount)
            {
                if (isEvenNumber) cardNewPositionY = (halfDeckAmount - 1 - i) * _positionPerCardAmplitudeY;
                else cardNewPositionY = 0;
            }

            if (i > halfDeckAmount)
            {
                if (isEvenNumber) cardNewPositionY = (halfDeckAmount - 1 - i) * _positionPerCardAmplitudeY;
                else cardNewPositionY = (halfDeckAmount - i) * _positionPerCardAmplitudeY;
            }            

            Vector2 newCardPosition = new Vector2(cardNewPositionX, _middlePositionY + cardNewPositionY);

            _hand[i].gameObject.transform.SetSiblingIndex(i);
            _hand[i].SetNewHandPosition(newCardPosition);
        }
    }

    private void UpdateCardsRotation(int newCardsAmount)
    {
        bool isEvenNumber = newCardsAmount % 2 == 0;
        int halfDeckAmount = Mathf.FloorToInt(newCardsAmount / 2);

        for (int i = 0; i < newCardsAmount; i++)
        {
            if (isEvenNumber && Mathf.FloorToInt(newCardsAmount / 2) - i == 0)
            {
                halfDeckAmount--;
            }
            Vector3 newCardRotation = new Vector3(0f, 0f, (halfDeckAmount - i) * _rotationPerCard);
            _hand[i].SetNewHandRotation(newCardRotation);
        }
    }

    public void StartButton()
    {
        DrawFullHand();
    }
}
