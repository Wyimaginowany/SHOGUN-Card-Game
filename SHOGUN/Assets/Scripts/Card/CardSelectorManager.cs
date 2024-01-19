using UnityEngine;
using Random = UnityEngine.Random;

public class CardSelectorManager : MonoBehaviour
{
    [Header("To Attach")]
    [SerializeField] private Transform _cardsParent;
    [SerializeField] private Transform _hiddenCardsPoint;
    [SerializeField] private GameObject _cardSelectionUI;
    [SerializeField] private GameObject _inGameUI;
    [SerializeField] private CardSelectionButton[] _cardSelectionButtons;
    [SerializeField] private GameObject[] _allPossibleCards;


    private DeckManager _deckManager;
    private Card[] _currentCardsSelected = new Card[3];

    private void OnDestroy()
    {
        PlayerHealth.OnPlayerDeath -= HideCardsUI;
    }

    private void Start()
    {
        _deckManager = GetComponent<DeckManager>();
        PlayerHealth.OnPlayerDeath += HideCardsUI;
    }

    void HideCardsUI()
    {
        _cardSelectionUI.SetActive(false);
        _inGameUI.SetActive(false);
    }
    
    public void HandleNewCardSelected(Card cardSelected)
    {
        for (int i = 0; i < _cardSelectionButtons.Length; i++)
        {
            if (_currentCardsSelected[i] == cardSelected) continue;
            Destroy(_currentCardsSelected[i].gameObject);
        }

        cardSelected.gameObject.GetComponent<CardVisual>().SetupCard();
        _deckManager.AddCardToDeck(cardSelected);
        _cardSelectionUI.SetActive(false);
        MapObject.MapInstance.ShowMap();
    }

    public void SetupNewCardsToSelect()
    {
        for (int i = 0; i < _cardSelectionButtons.Length; i++)
        {
            GameObject newCard = Instantiate(_allPossibleCards[Random.Range(0, _allPossibleCards.Length)],
                                             _hiddenCardsPoint.position,
                                             Quaternion.identity,
                                             _cardsParent);
            _currentCardsSelected[i] = newCard.GetComponent<Card>();  
            _cardSelectionButtons[i].SetupNewCard(_currentCardsSelected[i]);
        }

        _inGameUI.SetActive(false);
        _cardSelectionUI.SetActive(true);
    }
}
