using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Card : MonoBehaviour, IPointerDownHandler
{
    [Header("Settings")]
    //public for combatManager, cards will have own targets later
    [SerializeField] public int _value;
    [SerializeField] private int _cardCost;

    [Header("To Attach")]
    [SerializeField] private TMP_Text _cardText;
    [SerializeField] private TMP_Text _cardCostText;
    [SerializeField] private RawImage _cardColorImage;

    public static event Action<Card, int> OnCardPlayed;

    private CombatManager _combatManager;
    private RectTransform _recTransform;
    private Vector2 _startPosition;
    private int _slotIndex = -1;

    void Start()
    {
        _combatManager = GameObject.FindObjectOfType<CombatManager>();

        HandManager.OnCardDrawn += CheckIfDrawn;

        _recTransform = GetComponent<RectTransform>();
        _startPosition = _recTransform.anchoredPosition;

        SetupCard();
    }
    private void OnDestroy()
    {
        HandManager.OnCardDrawn -= CheckIfDrawn;
    }

    private void SetupCard()
    {
        _cardText.text = _value.ToString();
        _cardCostText.text = _cardCost.ToString();
        _cardColorImage.color = new Color(UnityEngine.Random.Range(0f, 1f),
                                          UnityEngine.Random.Range(0f, 1f),
                                          UnityEngine.Random.Range(0f, 1f));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //card clicked
        if (!_combatManager.HaveEnoughMana(_cardCost)) return;
        PlayCard();
    }

    private void PlayCard()
    {
        OnCardPlayed?.Invoke(this, _slotIndex);
        _recTransform.anchoredPosition = _startPosition;

        //card effect here
    }

    private void CheckIfDrawn(Card drawnCard, int slotInHandIndex)
    {
        if (drawnCard != this) return;   
        _slotIndex = slotInHandIndex;
    }

    public int GetCardCost()
    {
        return _cardCost;
    }

}
