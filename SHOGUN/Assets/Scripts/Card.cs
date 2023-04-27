using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Card : MonoBehaviour, IPointerDownHandler
{
    [Header("Settings")]
    [SerializeField] private int _value;

    [Header("To Attach")]
    [SerializeField] private TMP_Text _cardText;
    [SerializeField] private RawImage _cardColorImage;

    public static event Action<Card, int> OnCardPlayed;

    private RectTransform _recTransform;
    private Vector2 _startPosition;
    private int _slotIndex = -1;

    void Start()
    {
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
        _cardColorImage.color = new Color(UnityEngine.Random.Range(0f, 1f),
                                          UnityEngine.Random.Range(0f, 1f),
                                          UnityEngine.Random.Range(0f, 1f));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //card clicked
        OnCardPlayed?.Invoke(this, _slotIndex);
        EnemyTest enemyTest = (EnemyTest)FindObjectOfType(typeof(EnemyTest));
        enemyTest.TakeDamege(_value);
        _recTransform.anchoredPosition = _startPosition;
    }

    private void CheckIfDrawn(Card drawnCard, int slotInHandIndex)
    {
        if (drawnCard != this) return;
        _slotIndex = slotInHandIndex;
    }
}
