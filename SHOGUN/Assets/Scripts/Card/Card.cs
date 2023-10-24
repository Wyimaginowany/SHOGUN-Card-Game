using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Settings")]
    [SerializeField] private float _returnTransitionTime = 0.3f;
    [SerializeField] private float _beginTransitionTime = 0.8f;
    [Header("To Attach")]
    [SerializeField] protected CardScriptableObject _cardData;
    [SerializeField] private Transform _cardVisualDisplayPoint;

    protected CombatManager _combatManager;
    protected CanvasGroup _cardVisualCanvasGroup;
    protected Vector3 _startingPosition;

    public static event Action<Card> OnCardPlayed;
    public static event Action<Card> OnCardThrownAway;
    public static event Action<Card, Transform> OnCardMouseHoverStart;
    public static event Action<Card> OnCardMouseHoverEnd;
    public static event Action OnBeginDragging;
    public static event Action OnEndDragging;

    private RectTransform _rectTransform;
    private Quaternion _lastHandRotation;
    private Vector2 _newCardPosition;
    private float _timer = 0f;
    private int _childIndexBeforeDrag = 0;
    private bool _moveToNewHandPosition = false;
    private bool _isBeingDragged = false;

    protected virtual void Start()
    {
        _combatManager = (CombatManager)FindObjectOfType(typeof(CombatManager));
        _cardVisualCanvasGroup = GetComponentInChildren<CanvasGroup>();

        _rectTransform = GetComponent<RectTransform>();
        _startingPosition = _rectTransform.anchoredPosition;
    }

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
        //_cardVisualCanvasGroup.alpha = 1;
    }

    protected virtual void EndDragging()
    {
        _isBeingDragged = false;
        OnEndDragging?.Invoke();
    }

    protected void PlayCard()
    {
        OnCardPlayed?.Invoke(this);
        _rectTransform.anchoredPosition = _startingPosition;
    }

    protected void ShuffleCardIntoDeck()
    {
        OnCardThrownAway?.Invoke(this);
        _rectTransform.anchoredPosition = _startingPosition;
    }

    protected void ReturnCardToHand()
    {
        transform.SetSiblingIndex(_childIndexBeforeDrag);
        _rectTransform.rotation = _lastHandRotation;
        _timer = 0;
        _moveToNewHandPosition = true;
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

    public int GetCardCost()
    {
        return _cardData.Cost;
    }

    public CardScriptableObject GetCardData()
    {
        return _cardData;
    }

    private void Update()
    {
        if (_moveToNewHandPosition)
        {
            _timer += Time.deltaTime;
            float percentageComplete = _timer / _returnTransitionTime;
            _rectTransform.anchoredPosition = Vector2.Lerp(_rectTransform.anchoredPosition, _newCardPosition, percentageComplete);

            if (Vector2.Distance(_rectTransform.anchoredPosition, _newCardPosition) <= 0.1f)
            {
                _moveToNewHandPosition = false;
            }
        }

        if (_isBeingDragged)
        {
            _timer += Time.deltaTime;
            float percentageComplete = Mathf.Clamp(_timer / _beginTransitionTime, 0f, 1f);
            
            transform.position = Vector2.Lerp(transform.position, Input.mousePosition, percentageComplete);
        }
    }

    public void SetNewHandPosition(Vector2 newPosition)
    {
        _newCardPosition = newPosition;
        _timer = 0;
        _moveToNewHandPosition = true;
    }

    public void SetNewHandRotation(Vector3 newRotation)
    {
        _lastHandRotation = Quaternion.Euler(newRotation);
        _rectTransform.rotation = Quaternion.Euler(newRotation.x, newRotation.y, newRotation.z);
    }

    public void HideCard()
    {      
        _cardVisualCanvasGroup.alpha = 0;
    }

    #region InterfaceMethods

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnCardMouseHoverStart?.Invoke(this, _cardVisualDisplayPoint);     
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnCardMouseHoverEnd?.Invoke(this);
        _cardVisualCanvasGroup.alpha = 1;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        BeginDragging();
    }
    public void OnDrag(PointerEventData eventData)
    {
        OnBeeingDragged();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        EndDragging();
    }

    #endregion
}