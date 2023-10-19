using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("To Attach")]
    [SerializeField] protected CardScriptableObject _cardData;

    protected CombatManager _combatManager;
    protected CanvasGroup _cardVisualCanvasGroup;
    protected Vector3 _startingPosition;
    protected Vector3 _positionBeforeDrag;

    public static event Action<Card> OnCardPlayed;
    public static event Action<Card> OnCardThrownAway;
<<<<<<< HEAD
    public static event Action<Card> OnCardMouseHoverStart;
    public static event Action<Card> OnCardMouseHoverEnd;
    public static event Action OnBeginDragging;
    public static event Action OnEndDragging;
=======
>>>>>>> parent of b30b749 (commit)

    private RectTransform _rectTransform;
    private int _childIndexBeforeDrag = 0;
    private Quaternion _lastHandRotation;

    protected virtual void Start()
    {
        _combatManager = (CombatManager)FindObjectOfType(typeof(CombatManager));
        _cardVisualCanvasGroup = GetComponentInChildren<CanvasGroup>();

        _startingPosition = transform.position;
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
<<<<<<< HEAD
        //_cardHoverVisual.SetActive(true);
        OnCardMouseHoverStart?.Invoke(this);
        _cardVisualCanvasGroup.alpha = 0;
        Debug.Log("value: " + _cardData.Value + " Cost: " + _cardData.Cost);
=======
        BeginDragging();
>>>>>>> parent of b30b749 (commit)
    }
    public void OnDrag(PointerEventData eventData)
    {
<<<<<<< HEAD
        //_cardHoverVisual.SetActive(false);
        OnCardMouseHoverEnd?.Invoke(this);
        _cardVisualCanvasGroup.alpha = 1;
        Debug.Log("Exit value: " + _cardData.Value + " Cost: " + _cardData.Cost);
=======
        OnBeeingDragged();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        EndDragging();
>>>>>>> parent of b30b749 (commit)
    }

    protected virtual void BeginDragging()
    {
        OnBeginDragging?.Invoke();
        transform.rotation = Quaternion.Euler(Vector3.zero);
        _cardVisualCanvasGroup.alpha = 1;
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
        OnEndDragging?.Invoke();
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
        _rectTransform.rotation = _lastHandRotation;
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

    public void SetNewHandRotation(Vector3 newRotation)
    {
        _lastHandRotation = Quaternion.Euler(newRotation);
        _rectTransform.rotation = Quaternion.Euler(newRotation.x, newRotation.y, newRotation.z);
    }
}