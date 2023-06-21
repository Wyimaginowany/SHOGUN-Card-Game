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
    protected int _slotIndex = -1;

    public static event Action<Card, int> OnCardPlayed;
    public static event Action<Card, int> OnCardThrownAway;

    protected virtual void Start()
    {
        _combatManager = (CombatManager)FindObjectOfType(typeof(CombatManager));

        HandManager.OnCardDrawn += CheckIfDrawn;

        _startingPosition = transform.position;
    }

    private void OnDestroy()
    {
        HandManager.OnCardDrawn -= CheckIfDrawn;
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
        OnCardPlayed?.Invoke(this, _slotIndex);
        transform.position = _startingPosition;
    }

    protected void ShuffleCardIntoDeck()
    {
        OnCardThrownAway?.Invoke(this, _slotIndex);
        transform.position = _startingPosition;
    }

    protected void ReturnCardToHand()
    {
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

    private void CheckIfDrawn(Card drawnCard, int slotInHandIndex)
    {
        if (drawnCard != this) return;
        _slotIndex = slotInHandIndex;
    }

    public int GetCardCost()
    {
        return _cardData.Cost;
    }

    public CardScriptableObject GetCardData()
    {
        return _cardData;
    }
}