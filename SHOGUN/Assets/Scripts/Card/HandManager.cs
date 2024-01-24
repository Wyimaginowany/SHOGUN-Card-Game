using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandManager : MonoBehaviour
{
    [Header("To Attach")]
    [SerializeField] private Transform _cardVisualPrefab;
    [SerializeField] private Transform _hiddenCardsPoint;
    [SerializeField] private RectTransform _drawCardDisplayPoint;
    [Space(15)]
    [Header("Hand Settings")]
    [SerializeField] private int _startMaxHandSize = 10;
    [SerializeField] private float _spaceBetweenCards = 600f;
    [SerializeField] private float _middlePositionY = 100f;
    [SerializeField] private float _positionPerCardAmplitudeY = 3.5f;
    [SerializeField] private float _maxSpaceBetweenCards = 0.15f;
    [SerializeField] private float _minSpaceBetweenCards = 0f;
    [SerializeField] private float _spaceBetweenCardsGrowFactor = 1.5f;
    [SerializeField] private float _rotationPerCard = 1f;
    [SerializeField] private float _timeBeforeNextCardDraw = 0.7f;
    [Space(10)]
    [Header("Card Hover Settings")]
    [SerializeField] private TMP_Text _cardNameText;
    [SerializeField] private TMP_Text _cardCostText;
    [SerializeField] private TMP_Text _cardDescriptionText;
    [SerializeField] private Image _cardColorImage;
    [SerializeField] private Image _cardGraphicImage;


    [SerializeField] private List<Card> _hand = new List<Card>(); //serialize to see in editor remove later
    private float _middlePositionX;
    private CombatManager _combatManager;
    private DeckManager _deckManager;
    private RectTransform _rectTransform;
    private int _currentMaxHandSize;
    private int _hoverCounter = 0;
    private bool _isBeingDragged = false;
    private bool _fullHandDrawn = false;
    private bool _isDrawing = false;

    private void Start()
    {
        _deckManager = GetComponent<DeckManager>();
        _rectTransform = GetComponent<RectTransform>();
        _middlePositionX = _rectTransform.rect.width / 2;
        _currentMaxHandSize = _startMaxHandSize;
        _combatManager = GetComponent<CombatManager>();

        CombatManager.OnPlayerTurnStart += HandlePlayerTurnStart;
        Card.OnCardPlayed += RemoveFromHand;
        Card.OnCardThrownAway += RemoveFromHand;
        Card.OnCardMouseHoverStart += CardMouseHoverVisualStart;
        Card.OnCardMouseHoverEnd += CardMouseHoverEnd;
        Card.OnBeginDragging += CardBeginDrag;
        Card.OnEndDragging += CardEndDragging;
    }

    private void OnDestroy()
    {
        CombatManager.OnPlayerTurnStart -= HandlePlayerTurnStart;
        Card.OnCardPlayed -= RemoveFromHand;
        Card.OnCardThrownAway -= RemoveFromHand;
        Card.OnCardMouseHoverStart -= CardMouseHoverVisualStart;
        Card.OnCardMouseHoverEnd -= CardMouseHoverEnd;
        Card.OnBeginDragging -= CardBeginDrag;
        Card.OnEndDragging -= CardEndDragging;
    }

    private void CardEndDragging()
    {
        _isBeingDragged = false;
        _hoverCounter = 0;
    }

    private void CardBeginDrag()
    {
        _isBeingDragged = true;
        _cardVisualPrefab.position = _hiddenCardsPoint.position;
        _hoverCounter = 0;
    }

    private void CardMouseHoverVisualStart(Card card, Transform cardVisualNewPosition)
    {
        if (_isBeingDragged) return;
        if (!_fullHandDrawn) return;

        card.HideCard();
        _hoverCounter++;
        SetupCardHoverVisual(card);

        _cardVisualPrefab.position = cardVisualNewPosition.position;
        _cardVisualPrefab.rotation = cardVisualNewPosition.rotation;
    }

    private void SetupCardHoverVisual(Card card)
    {
        CardScriptableObject cardData = card.CardData;
        _cardDescriptionText.text = card.GetCardDescriptionDefault();
        _cardNameText.text = cardData.CardName;
        _cardColorImage.color = cardData.CardColor;
        _cardCostText.text = card.GetCardCost().ToString();
        _cardGraphicImage.sprite= cardData.CardImage;
    }

    private void CardMouseHoverEnd(Card card)
    {
        _hoverCounter--;
        if (_hoverCounter > 0) return;

        _cardVisualPrefab.position = _hiddenCardsPoint.position;
    }

    private void HandlePlayerTurnStart()
    {
        _fullHandDrawn = false;
        if (!_isDrawing) DrawFullHand();
        _isDrawing = true;
    }

    public void DrawFullHand()
    {
        StartCoroutine(drawCard(_timeBeforeNextCardDraw));
    }

    private IEnumerator drawCard(float timeBeforeNextCardDraw)
    {
        for (int i = _hand.Count; i < _currentMaxHandSize; i++)
        {
            Card drawnCard = _deckManager.DrawTopCard();
            drawnCard.DrawThisCard(_drawCardDisplayPoint.anchoredPosition);
            yield return new WaitForSeconds(timeBeforeNextCardDraw);

            _hand.Add(drawnCard);
            UpdateCardsPosition(_hand.Count);
            UpdateCardsRotation(_hand.Count);
        }

        HandleFullHandDrawn();
    }

    private void HandleFullHandDrawn()
    {
        foreach (Card card in _hand)
        {
            card.MakeInteractable();
        }

        _combatManager.FullHandDrawn();
        _fullHandDrawn = true;
        _isDrawing = false;
    }

    private void RemoveFromHand(Card cardPlayed)
    {
        _hand.Remove(cardPlayed);
        UpdateCardsPosition(_hand.Count);
        UpdateCardsRotation(_hand.Count);
        _hoverCounter = 0;
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
    
    public void ReducePermenentCardsCostInHand(int reduceAmount)
    {
        foreach(Card card in _hand)
        {
            card.ReduceCostPermanently(reduceAmount);
        }
    }

    public void ReduceOneTurnCardsCostInHand(int reduceAmount)
    {
        foreach (Card card in _hand)
        {
            card.ReduceCostThisTurn(reduceAmount);
        }
    }

    public void ShuffleHandIntoDeck()
    {
        Card[] cardsInHand = _hand.ToArray();
        for (int i = 0; i < cardsInHand.Length; i++)
        {
            cardsInHand[i].ShuffleCardIntoDeck();
            _hand.Remove(cardsInHand[i]);
        }

        _fullHandDrawn = false;
        _hoverCounter = 0;
        _cardVisualPrefab.position = _hiddenCardsPoint.position;
    }

    public bool FullHandDrawn()
    {
        return _fullHandDrawn;
    }
}
