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
    [Space(5)]
    [Header("Stages")]
    [SerializeField] private EnemyGroupsScriptableObject enemyStages;


    public static event Action OnPlayerTurnStart;
    public static event Action OnPlayerTurnEnd;
    public static event Action OnAllEnemiesKilled;
    public static event Action<int> OnPermanentDamageCardBuff;
    public static event Action<int> OnOneTurnDamageCardBuff;
    public static event Action<int> OnPermanentDamageCardDebuff;
    public static event Action<int> OnOneTurnDamageCardDebuff;

    private List<EnemyHealth> _aliveEnemies = new List<EnemyHealth>();

    private PlayerHealth _playerHealth;
    private CardSelectorManager _cardSelectorManager;
    private HandManager _handManager;
    private int _currentMana;
    private int _enemyOrderIndex = 0;
    public int turnCounter;
    private int _currentStage = 0;
    private bool _isPlayerStunned = false;

    private void Start()
    {
        _playerHealth = (PlayerHealth)FindObjectOfType(typeof(PlayerHealth));
        _handManager = GetComponent<HandManager>();
        _cardSelectorManager = GetComponent<CardSelectorManager>();

        Card.OnCardPlayed += HandleCardPlayed;
        EnemyHealth.OnEnemyDeath += HandleEnemyDeath;
        MapEvent.OnNewStageStarted += HandleNewStageStart;

        _currentMana = _maxMana;
        _manaAmountText.text = _currentMana.ToString();
        turnCounter = 0;
        StartGame();
    }

    private void OnDestroy()
    {
        Card.OnCardPlayed -= HandleCardPlayed;
        EnemyHealth.OnEnemyDeath -= HandleEnemyDeath;
        MapEvent.OnNewStageStarted -= HandleNewStageStart;
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
            SpawnRandom();
        }
    }

    private void StartGame()
    {
        MapObject.MapInstance.ShowMap();
    }

    private void HandleNewStageStart()
    {
        _endTurnButton.SetActive(false);
        _endTurnButtonBlocked.SetActive(true);
        ResetMana();
        SpawnNewEnemies();
        _handManager.DrawFullHand();
    }

    private void HandleStageComplete()
    {
        OnAllEnemiesKilled?.Invoke();
        _handManager.ShuffleHandIntoDeck();
        _cardSelectorManager.SetupNewCardsToSelect();
    }

    public void HandleTreasureChest(){
        _cardSelectorManager.SetupNewCardsToSelect();
    }

    public void EndPlayerTurn()
    {
        _enemyOrderIndex = 0;
        _endTurnButton.SetActive(false);
        _endTurnButtonBlocked.SetActive(true);


        OnPlayerTurnEnd?.Invoke();
        NextEnemyTurn();
    }

    //chwilowo potrzebne do działania mapy



    #region Affect Enemy
    public void SpawnNewEnemies()
    {
        if (_currentStage >= enemyStages.enemyGroups.Length - 1)
        {
            _currentStage = 0;
        }


        int random = UnityEngine.Random.Range(0, enemyStages.enemyGroups[_currentStage].PossibleEnemyGroups.Length);
        int enemiesAmount = enemyStages.enemyGroups[_currentStage].PossibleEnemyGroups[random].EnemiesOnStage.Length;

        for (int i = 0; i < enemiesAmount; i++)
        {
            GameObject newEnemy = Instantiate(enemyStages.enemyGroups[_currentStage].PossibleEnemyGroups[random].EnemiesOnStage[i],
                                              _spawnPoints[i].position,
                                              Quaternion.identity);

            _aliveEnemies.Add(newEnemy.GetComponent<EnemyHealth>());
        }

        _currentStage++;
        
    }

    public void SpawnRandom()
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

    private void HandleEnemyDeath(EnemyHealth deadEnemy)
    {
        _aliveEnemies.Remove(deadEnemy);

        if (_aliveEnemies.Count > 0) return;

        HandleStageComplete();
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

    #endregion

    #region Affect Player
    private void HandlePlayerTurnStart()
    {
        OnPlayerTurnStart?.Invoke();

        _currentMana = _maxMana;
        _manaAmountText.text = _currentMana.ToString();

        if (_isPlayerStunned)
        {
            HandlePlayerStunned();
        }
    }

    private void HandlePlayerStunned()
    {
        _isPlayerStunned = false;
        EndPlayerTurn();
    }

    public void ResetMana()
    {
        _currentMana = _maxMana;
        _manaAmountText.text = _currentMana.ToString();
    }

    public void HandleCardPlayed(Card cardPlayed)
    {
        //cards will have own targets later
        _currentMana -= cardPlayed.GetCardCost();
        _manaAmountText.text = _currentMana.ToString();
    }

    public void FullHandDrawn()
    {
        _endTurnButton.SetActive(true);
        _endTurnButtonBlocked.SetActive(false);
    }

    public bool HaveEnoughMana(int cardCost)
    {
        if (cardCost > _currentMana) return false;
        return true;
    }

    public void BuffPlayerDamagePermenent(int buffAmount)
    {
        OnPermanentDamageCardBuff?.Invoke(buffAmount);
    }

    public void BuffPlayerDamageOneTurn(int buffAmount)
    {
        OnOneTurnDamageCardBuff?.Invoke(buffAmount);
    }

    public void DebuffPlayerDamagePermement(int debuffAmount)
    {
        OnPermanentDamageCardBuff?.Invoke(debuffAmount);
    }

    public void DebuffPlayerDamageOneTurn(int debuffAmount)
    {
        OnOneTurnDamageCardBuff?.Invoke(debuffAmount);
    }

    public void StunPlayer()
    {
        _isPlayerStunned = true;
    }

    public void ReduceCardsCost(int reduceAmount)
    {
        _handManager.ReducePermenentCardsCostInHand(reduceAmount);
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

    public void AddBleedStacksToPlayer(int stackCount)
    {
        _playerHealth.AddBleedStacks(stackCount);
    }

    #endregion
}
