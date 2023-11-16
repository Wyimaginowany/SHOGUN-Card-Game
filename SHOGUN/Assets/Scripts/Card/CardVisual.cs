using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Card))]
public class CardVisual : MonoBehaviour
{
    [Header("To Attach")]
    [SerializeField] private TMP_Text _cardText;
    [SerializeField] private TMP_Text _cardCostText;
    [SerializeField] private RawImage _cardColorImage;
    
    private CardScriptableObject _cardData;

    void Start()
    {
        _cardData = GetComponent<Card>().GetCardData();

        SetupCard();
    }

    private void SetupCard()
    {
        _cardText.text = _cardData.Value.ToString();
        _cardCostText.text = _cardData.Cost.ToString();
        _cardColorImage.color = _cardData.CardColor; 
    }
}
