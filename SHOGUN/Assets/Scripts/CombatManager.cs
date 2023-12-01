using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _maxMana = 5;

    [Header("To Attach")]
    [SerializeField] private EnemyHealth[] _enemies;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private TMP_Text _manaAmountText;
    [SerializeField] private GameObject _endTurnButton;
    [SerializeField] private GameObject _endTurnButtonBlocked;

    public static event Action OnPlayerTurnStart;
    public static event Action OnPlayerTurnEnd;
    public  event Action<int> OnDamageCardBuff;

    private List<EnemyHealth> _aliveEnemies = new List<EnemyHealth>();

    private PlayerHealth _playerHealth;
    private HandManager _handManager;
    private int _currentMana;
    private int _enemyOrderIndex = 0;
    public int turnCounter;
    private int _currentPlayerBleedStacks = 0;

    private void Start()
    {
        _playerHealth = (PlayerHealth)FindObjectOfType(typeof(PlayerHealth));
        _handManager = GetComponent<HandManager>();

        Card.OnCardPlayed += HandleCardPlayed;
        EnemyHealth.OnEnemyDeath += HandleEnemyDeath;

        _currentMana = _maxMana;
        _manaAmountText.text = _currentMana.ToString();
        turnCounter = 0;
        MapEvent.OnPlayerTurnEnd+=StartCombat;
        
    }

    private void StartCombat(){
        
        SpawnNewEnemies();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            EnemyHealth[] aliveEnemies = _aliveEnemies.ToArray();
            foreach (EnemyHealth enemy in aliveEnemies)
            {
                _aliveEnemies.Remove(enemy);
                Destroy(enemy.gameObject);
            }
            SpawnNewEnemies();
        }
    }

    private void OnDestroy()
    {
        Card.OnCardPlayed -= HandleCardPlayed;
        EnemyHealth.OnEnemyDeath -= HandleEnemyDeath;
        MapEvent.OnPlayerTurnEnd-=StartCombat;
    }

    public void FullHandDrawn()
    {
        _endTurnButton.SetActive(true);
        _endTurnButtonBlocked.SetActive(false);
    }

    private void HandleEnemyDeath(EnemyHealth deadEnemy)
    {
        _aliveEnemies.Remove(deadEnemy);

        if (_aliveEnemies.Count > 0) return;

        

        //chwilowe rozwiazanie
        //gdzies musi byc koniec poziomu
        CardSelectorManager tmp = (CardSelectorManager)FindObjectOfType(typeof(CardSelectorManager));
        tmp.SetupNewCardsToSelect();
    }

    public void SpawnNewEnemies()
    {
        int randomEnemiesAmount = UnityEngine.Random.Range(1, _spawnPoints.Length + 1);

        for (int i = 0; i < randomEnemiesAmount; i++)
        {
            GameObject newEnemy = Instantiate(_enemies[UnityEngine.Random.Range(0, _enemies.Length)].gameObject,
                                              _spawnPoints[i].position,
                                              Quaternion.identity);
            
            _aliveEnemies.Add(newEnemy.GetComponent<EnemyHealth>());
        }
    }

    public void EndTurnButton()
    {
        _enemyOrderIndex = 0;
        _endTurnButton.SetActive(false);
        _endTurnButtonBlocked.SetActive(true);


        OnPlayerTurnEnd?.Invoke();
        NextEnemyTurn();
    }

    public void NextEnemyTurn()
    {
        if (_enemyOrderIndex == _aliveEnemies.Count)
        {
            HandlePlayerTurnStart();
            turnCounter++;

            return;
        }
        //HandleTurn is protected now
        _aliveEnemies[_enemyOrderIndex].GetComponent<EnemyCombat>().HandleTurn();

        _enemyOrderIndex++;
    }

    private void HandlePlayerTurnStart()
    {
        OnPlayerTurnStart?.Invoke();

        _currentMana = _maxMana;
        _manaAmountText.text = _currentMana.ToString();
    }

    public void HandleCardPlayed(Card cardPlayed)
    {
        //cards will have own targets later
        _currentMana -= cardPlayed.GetCardCost();
        _manaAmountText.text = _currentMana.ToString();
    }

    public bool HaveEnoughMana(int cardCost)
    {
        if (cardCost > _currentMana) return false;      
        return true;
    }

    public void DealDamageToEnemy(EnemyHealth enemy, int damage)
    {
        enemy.TakeDamage(damage);
    }

    public void DealDamageToAllEnemies(int damage)
    {
        EnemyHealth[] currentlyAliveEnemies = _aliveEnemies.ToArray();

        foreach (EnemyHealth enemy in currentlyAliveEnemies)
        {
            enemy.TakeDamage(damage);
        }

    }

    public void DealDamageToPlayer(int damage)
    {
        _playerHealth.TakeDamage(damage);
    }

    public void HealPlayer(int healAmount)
    {
        _playerHealth.HealPlayer(healAmount);
    }

    public void GivePlayerShield(int shieldAmount)
    {
        _playerHealth.GiveShield(shieldAmount);
    }

    public void IncreaseCurrentMana(int amount)
    {
        _currentMana += amount;
        _manaAmountText.text = _currentMana.ToString();
    }

    public void ReduceCardsCost(int reduceAmount)
    {
        _handManager.ReduceCardsCostInHand(reduceAmount);
    }

    public void BuffPlayerDamage(int buffAmount)
    {
        OnDamageCardBuff?.Invoke(buffAmount);
    }

    public void IncreasePlayerBleed(int bleedAmount)
    {
        _currentPlayerBleedStacks += bleedAmount;
    }
}
