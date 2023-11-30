using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardVisual : MonoBehaviour
{
    [Header("To Attach")]
    [SerializeField] private TMP_Text _cardText;
    [SerializeField] private TMP_Text _cardCostText;
    [SerializeField] private Image _cardColorImage;
    
    private Card _card;

    void Start()
    {
        _card = GetComponent<Card>().GetCard();
        SetupCard(_card.CardData);
    }

    private void SetupCard(CardScriptableObject cardData)
    {
        _cardText.text = cardData.Value.ToString();
        _cardColorImage.color = cardData.CardColor; 
        _cardCostText.text = cardData.Cost.ToString();
    }

    public void UpdateCostVisual()
    {
        _cardCostText.text = _card.GetCardCost().ToString();
    }

    public void UpdateCardValueVisual(int buffAmount)
    {
        int currentValue = int.Parse(_cardText.text);
        _cardText.text = (buffAmount + currentValue).ToString();
    }
}
