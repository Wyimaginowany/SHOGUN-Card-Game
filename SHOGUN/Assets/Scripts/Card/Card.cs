using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("To Attach")]
    [SerializeField] protected CardScriptableObject _cardData;

    protected CombatManager _combatManager;
    protected Vector3 _startingPosition;
    protected Vector3 _positionBeforeDrag;

    public static event Action<Card> OnCardPlayed;
    public static event Action<Card> OnCardThrownAway;

    private RectTransform _rectTransform;
    private int _childIndexBeforeDrag = 0;

    protected virtual void Start()
    {
        _combatManager = (CombatManager)FindObjectOfType(typeof(CombatManager));

        _startingPosition = transform.position;
        _rectTransform = GetComponent<RectTransform>();
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

    protected virtual void BeginDragging()
    {
        _positionBeforeDrag = transform.position;
        _childIndexBeforeDrag = transform.GetSiblingIndex();
        transform.SetAsLastSibling();
    }
    protected virtual void OnBeeingDragged()
    {
        transform.position = Input.mousePosition;
    }

    protected virtual void EndDragging()
    {
        ReturnCardToHand();
    }

    protected void PlayCard()
    {
        OnCardPlayed?.Invoke(this);
        transform.position = _startingPosition;
    }

    protected void ShuffleCardIntoDeck()
    {
        OnCardThrownAway?.Invoke(this);
        transform.position = _startingPosition;
    }

    protected void ReturnCardToHand()
    {
        transform.SetSiblingIndex(_childIndexBeforeDrag);
        transform.position = _positionBeforeDrag;
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

    public void SetNewHandPosition(Vector2 newPosition)
    {
        _rectTransform.anchoredPosition = newPosition;
    }
}