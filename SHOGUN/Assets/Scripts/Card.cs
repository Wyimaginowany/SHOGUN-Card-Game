using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("To Attach")]
    [SerializeField] private CardScriptableObject _cardData;

    private CombatManager _combatManager;
    private Vector3 _startingPosition;
    private Vector3 _positionBeforeDrag;
    private int _slotIndex = -1;

    public static event Action<Card, int> OnCardPlayed;
    public static event Action<Card, int> OnCardThrownAway;

    private void Start()
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
        _positionBeforeDrag = transform.position;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        List<CardDropArea> possibleDropAreas = GetDropAreas();

        if (possibleDropAreas.Count == 0)
        {
            ReturnCardToHand();
            return;
        }

        foreach (CardDropArea dropArea in possibleDropAreas)
        {
            if (dropArea.GetDropArea() == PossibleAreas.PlayArea)
            {
                if (!_combatManager.HaveEnoughMana(_cardData.Cost))
                {
                    ReturnCardToHand();
                    return;
                }

                OnCardPlayed?.Invoke(this, _slotIndex);
                transform.position = _startingPosition;

                return;
            }
            if (dropArea.GetDropArea() == PossibleAreas.ThrowOutArea)
            {
                OnCardThrownAway?.Invoke(this, _slotIndex);
                transform.position = _startingPosition;

                return;
            }

            ReturnCardToHand();
        }

    }

    private void ReturnCardToHand()
    {
        transform.position = _positionBeforeDrag;
    }

    private List<CardDropArea> GetDropAreas()
    {
        List<CardDropArea> allDropAreas = new List<CardDropArea>();

        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultsList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultsList);

        for (int i = 0; i < raycastResultsList.Count; i++)
        {
            if (raycastResultsList[i].gameObject.GetComponent<CardDropArea>() != null)
            {
                allDropAreas.Add(raycastResultsList[i].gameObject.GetComponent<CardDropArea>());
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