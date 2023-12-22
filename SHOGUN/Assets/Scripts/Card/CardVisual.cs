using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardVisual : MonoBehaviour
{
    [Header("To Attach")]
    [SerializeField] private TMP_Text _cardName;
    //[SerializeField] private TMP_Text _cardText;
    [SerializeField] private TMP_Text _cardCostText;
    [SerializeField] private Image _cardColorImage;
    [SerializeField] private TMP_Text _cardDescription;
    [SerializeField] private Image _cardGraphicImage;

    private Card _card;

    void Start()
    {
        _card = GetComponent<Card>();
        SetupCard();
    }

    public void SetupCard()
    {
        //_cardText.text = cardData.Value.ToString();
        _cardColorImage.color = _card.CardData.CardColor;
        _cardName.text = _card.CardData.CardName;
        _cardGraphicImage.sprite = _card.CardData.CardImage;
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        _cardCostText.text = _card.GetCardCost().ToString();
        _cardDescription.text = _card.GetCardDescriptionDefault();
    }
}