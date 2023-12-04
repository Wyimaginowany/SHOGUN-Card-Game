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
        SetupCard(_card.CardData);
    }

    private void SetupCard(CardScriptableObject cardData)
    {
        //_cardText.text = cardData.Value.ToString();
        _cardColorImage.color = cardData.CardColor;
        _cardName.text = cardData.CardName;
        _cardGraphicImage.sprite = cardData.CardImage;
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        _cardCostText.text = _card.GetCardCost().ToString();
        _cardDescription.text = _card.GetCardDescriptionDefault();
    }
}