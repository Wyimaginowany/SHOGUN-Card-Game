using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSelectionButton : MonoBehaviour
{
    [Header("To Attach")]
    [SerializeField] private TMP_Text _cardCostText;
    [SerializeField] private TMP_Text _cardValueText;
    [SerializeField] private RawImage _cardColorImage;

    private Card _cardToSelect;
    private Button _cardButton;
    private CardSelectorManager _cardSelector;

    void Start()
    {
        _cardSelector = (CardSelectorManager)FindObjectOfType(typeof(CardSelectorManager));

        _cardButton = GetComponent<Button>();

        _cardButton.onClick.AddListener(() =>
        {
            HandleCardSelection();
        });
    }

    private void HandleCardSelection()
    {
        _cardSelector.HandleNewCardSelected(_cardToSelect);
    }

    public void SetupNewCard(Card newCard)
    {
        _cardToSelect = newCard;
        CardScriptableObject cardData = newCard.CardData;
        _cardValueText.text = cardData.Value.ToString();
        _cardColorImage.color = cardData.CardColor;
        _cardCostText.text = cardData.Cost.ToString();
    }
}
