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

    public static event Action OnPlayerTurnStart;
    public static event Action OnPlayerTurnEnd;

    private List<EnemyHealth> _aliveEnemies = new List<EnemyHealth>();

    private int _currentMana;
    private int _enemyOrderIndex = 0;
    public int turnCounter;

    private void Start()
    {
        Card.OnCardPlayed += HandleCardPlayed;
        EnemyHealth.OnEnemyDeath += HandleEnemyDeath;

        _currentMana = _maxMana;
        _manaAmountText.text = _currentMana.ToString();
        turnCounter = 0;
    }

    private void OnDestroy()
    {
        Card.OnCardPlayed -= HandleCardPlayed;
        EnemyHealth.OnEnemyDeath -= HandleEnemyDeath;
    }

    private void HandleEnemyDeath(EnemyHealth deadEnemy)
    {
        _aliveEnemies.Remove(deadEnemy);

        if (_aliveEnemies.Count > 0) return;

        SpawnNewEnemies();
    }

    public void SpawnNewEnemies()
    {
        int randomEnemiesAmount = UnityEngine.Random.Range(1, _spawnPoints.Length + 1);

        for (int i = 0; i < randomEnemiesAmount; i++)
        {
            GameObject newEnemy = Instantiate(_enemies[UnityEngine.Random.Range(3,5)].gameObject,
                                              _spawnPoints[i].position,
                                              Quaternion.identity);
            
            _aliveEnemies.Add(newEnemy.GetComponent<EnemyHealth>());
        }
    }

    public void EndTurnButton()
    {
        _enemyOrderIndex = 0;


        OnPlayerTurnEnd?.Invoke();
        NextEnemyTurn();
    }

    public void NextEnemyTurn()
    {
        if (_enemyOrderIndex == _aliveEnemies.Count)
        {
            OnPlayerTurnStart?.Invoke();
            
            _currentMana = _maxMana;
            _manaAmountText.text = _currentMana.ToString();
            turnCounter++;
            
            return;
        }
        //HandleTurn is protected now
        _aliveEnemies[_enemyOrderIndex].GetComponent<EnemyCombat>().HandleTurn();

        _enemyOrderIndex++;
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

    public void DealDamageToAllEnemies(int damage)
    {
        EnemyHealth[] currentlyAliveEnemies = _aliveEnemies.ToArray();

        foreach (EnemyHealth enemy in currentlyAliveEnemies)
        {
            enemy.TakeDamage(damage);
        }

    }
}
