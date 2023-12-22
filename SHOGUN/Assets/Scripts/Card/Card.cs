using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CardEnums;
[RequireComponent(typeof(CardVisual))]
public abstract class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Settings")]
    [SerializeField] private float _returnTransitionTime = 0.3f;
    [SerializeField] private float _beginTransitionTime = 0.8f;
    [SerializeField] private float _drawTransitionTime = 4f;
    [Header("To Attach")]
    public CardScriptableObject CardData;
    [SerializeField] private Transform _cardVisualDisplayPoint;
    [SerializeField] private CardAnimation _cardAnimation;

    //protected
    protected CombatManager _combatManager;
    protected HandManager _handManager;
    protected CardVisual _cardVisual;
    protected CanvasGroup _cardVisualCanvasGroup;
    protected int _thisTurnCardCostReduction = 0;
    protected int _thisTurnCardValueBuff = 0;
    protected int _currentCardCost;
    protected int _currentCardValue;
    protected Vector3 _startingPosition;
    protected String _cardDescriptionDefault;

    //events
    public static event Action<Card> OnCardPlayed;
    public static event Action<Card> OnCardThrownAway;
    public static event Action<Card, Transform> OnCardMouseHoverStart;
    public static event Action<Card> OnCardMouseHoverEnd;
    public static event Action OnBeginDragging;
    public static event Action OnEndDragging;

    //privates
    private RectTransform _rectTransform;
    private Quaternion _lastHandRotation;
    private Vector2 _newCardPosition;
    private Vector2 _cardDisplayPosition;
    private float _timer = 0f;
    private int _childIndexBeforeDrag = 0;
    private bool _moveToNewHandPosition = false;
    private bool _isBeingDragged = false;
    private bool _isBeingDrawn = false;
    private bool _isInteractable = false;

    protected virtual void Start()
    {
        _combatManager = (CombatManager)FindObjectOfType(typeof(CombatManager));
        _handManager = (HandManager)FindObjectOfType(typeof(HandManager));
        _cardVisualCanvasGroup = GetComponentInChildren<CanvasGroup>();
        _cardVisual = GetComponent<CardVisual>();

        _rectTransform = GetComponent<RectTransform>();
        _startingPosition = _rectTransform.anchoredPosition;
        _currentCardCost = CardData.Cost;
        _currentCardValue = CardData.Value;
        _cardDescriptionDefault = CardData.Description;

        CombatManager.OnPlayerTurnEnd += HandlePlayerTurnEnd;
    }

    private void OnDestroy()
    {
        CombatManager.OnPlayerTurnEnd -= HandlePlayerTurnEnd;
    }

    private void Update()
    {
        if (_isBeingDrawn)
        {
            _timer += Time.deltaTime;
            float percentageComplete = _timer / _drawTransitionTime;
            _rectTransform.anchoredPosition = Vector2.Lerp(_rectTransform.anchoredPosition, _cardDisplayPosition, percentageComplete);
        }

        if (_moveToNewHandPosition)
        {
            _timer += Time.deltaTime;
            float percentageComplete = _timer / _returnTransitionTime;
            _rectTransform.anchoredPosition = Vector2.Lerp(_rectTransform.anchoredPosition, _newCardPosition, percentageComplete);

            if (Vector2.Distance(_rectTransform.anchoredPosition, _newCardPosition) <= 0.1f)
            {
                _moveToNewHandPosition = false;
                if (_handManager.FullHandDrawn()) _isInteractable = true;
            }
        }

        if (_isBeingDragged && _isInteractable)
        {
            _timer += Time.deltaTime;
            float percentageComplete = Mathf.Clamp(_timer / _beginTransitionTime, 0f, 1f);
            
            transform.position = Vector2.Lerp(transform.position, Input.mousePosition, percentageComplete);
        }
    }

    private void HandlePlayerTurnEnd()
    {
        _isInteractable = false;
        _thisTurnCardCostReduction = 0;
        _thisTurnCardValueBuff = 0;
    }

    protected List<PossibleAreas> GetDropAreas()
    {
        List<PossibleAreas> allDropAreas = new List<PossibleAreas>();

        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultsList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultsList);

        for (int i = 0; i < raycastResultsList.Count; i++)
        {
            if (raycastResultsList[i].gameObject.GetComponent<CardDropArea>() != null)
            {
                if (allDropAreas.Contains(raycastResultsList[i].gameObject.GetComponent<CardDropArea>().GetDropArea())) continue;

                allDropAreas.Add(raycastResultsList[i].gameObject.GetComponent<CardDropArea>().GetDropArea());
            }
        }

        return allDropAreas;
    }

    public void DrawThisCard(Vector2 drawDisplayPoistion)
    {
        _cardVisualCanvasGroup.alpha = 1;
        _cardDisplayPosition = drawDisplayPoistion;
        _timer = 0;
        _isBeingDrawn = true;
    }

    protected void ReturnCardToHand()
    {
        _isInteractable = false;
        transform.SetSiblingIndex(_childIndexBeforeDrag);
        _rectTransform.rotation = _lastHandRotation;
        _timer = 0;
        _moveToNewHandPosition = true;
    }

    public void ReduceCostPermanently(int reduceAmount)
    {
        _currentCardCost -= reduceAmount;
        _cardVisual.UpdateVisual();
    }

    public void ReduceCostThisTurn(int reduceAmount)
    {
        _thisTurnCardCostReduction = reduceAmount;
        _cardVisual.UpdateVisual();
    }

    public void HideCard()
    {
        _cardVisualCanvasGroup.alpha = 0;
    }

    public void ShowCard()
    {
        _cardVisualCanvasGroup.alpha = 1;
    }

    public void ShuffleCardIntoDeck()
    {
        _isInteractable = false;
        _moveToNewHandPosition = false;
        _rectTransform.anchoredPosition = _startingPosition;
        OnCardThrownAway?.Invoke(this);
    }

    public void SetNewHandPosition(Vector2 newPosition)
    {
        _newCardPosition = newPosition;
        _timer = 0;
        _isBeingDrawn = false;
        _moveToNewHandPosition = true;
    }

    public void SetNewHandRotation(Vector3 newRotation)
    {
        _lastHandRotation = Quaternion.Euler(newRotation);
        _rectTransform.rotation = Quaternion.Euler(newRotation.x, newRotation.y, newRotation.z);
    }

    public void MakeInteractable()
    {
        _isInteractable = true;
    }

    #region Virtual methods
    protected virtual void BeginDragging()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        _cardVisualCanvasGroup.alpha = 1;
        _isBeingDragged = true;
        _timer = 0f;
        _childIndexBeforeDrag = transform.GetSiblingIndex();
        transform.SetAsLastSibling();
        OnBeginDragging?.Invoke();
    }
    protected virtual void OnBeeingDragged()
    {
        //
    }

    protected virtual void EndDragging()
    {
        _isBeingDragged = false;
        OnEndDragging?.Invoke();
    }

    protected virtual void PlayCard()
    {
        OnCardPlayed?.Invoke(this);
        _rectTransform.anchoredPosition = _startingPosition;
        PlayerCardAnimations.TriggerAnimation(_cardAnimation);
        _isInteractable = false;
    }
    #endregion

    #region Getter functions

    public String GetCardDescriptionDefault()
    {
        String cardDescription = CardData.Description.Replace("X", GetCardValue().ToString());
        return cardDescription;
    }

    public int GetCardCost()
    {
        int currentCardCost = _currentCardCost - _thisTurnCardCostReduction;
        if (currentCardCost < 0) currentCardCost = 0;
        return currentCardCost;
    }

    public int GetCardValue()
    {
        return Mathf.Clamp(_currentCardValue + _thisTurnCardValueBuff, 0, 10000);
    }

    #endregion

    #region InterfaceMethods

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_isInteractable) return;
        OnCardMouseHoverStart?.Invoke(this, _cardVisualDisplayPoint);     
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_isInteractable) return;
        OnCardMouseHoverEnd?.Invoke(this);
        _cardVisualCanvasGroup.alpha = 1;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!_isInteractable) return;
        BeginDragging();
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!_isInteractable) return;
        OnBeeingDragged();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_isInteractable) return;
        EndDragging();
    }

    #endregion
}