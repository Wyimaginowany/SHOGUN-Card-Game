using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _maxMana = 5;

    [Header("To Attach")]
    [SerializeField] private EnemyTest[] _enemies;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private TMP_Text _manaAmountText;

    public static event Action OnPlayerTurnStart;
    public static event Action OnPlayerTurnEnd;

    private List<EnemyTest> _aliveEnemies = new List<EnemyTest>();
    private EnemyTest[] _enemyTurnAliveEnemies;

    private int _currentMana;
    private int _enemyOrderIndex = 0;

    private void Start()
    {
        Card.OnCardPlayed += HandleCardPlayed;
        EnemyTest.OnEnemyDeath += HandleEnemyDeath;

        _currentMana = _maxMana;
        _manaAmountText.text = _currentMana.ToString();
    }

    private void OnDestroy()
    {
        Card.OnCardPlayed -= HandleCardPlayed;
        EnemyTest.OnEnemyDeath -= HandleEnemyDeath;
    }

    private void HandleEnemyDeath(EnemyTest deadEnemy)
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
            GameObject newEnemy = Instantiate(_enemies[UnityEngine.Random.Range(0, _enemies.Length)].gameObject,
                                              _spawnPoints[i].position,
                                              Quaternion.identity);
            _aliveEnemies.Add(newEnemy.GetComponent<EnemyTest>());
        }
    }

    public void EndTurnButton()
    {
        _enemyOrderIndex = 0;
        _enemyTurnAliveEnemies = _aliveEnemies.ToArray();

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

            return;
        }

        _enemyTurnAliveEnemies[_enemyOrderIndex].HandleTurn();

        _enemyOrderIndex++;
    }

    public void HandleCardPlayed(Card cardPlayed, int slotIndex)
    {
        //cards will have own targets later
        _currentMana -= cardPlayed.GetCardCost();
        _manaAmountText.text = _currentMana.ToString();
        _aliveEnemies[0].TakeDamage(cardPlayed.GetCardData().Value);
    }

    public bool HaveEnoughMana(int cardCost)
    {
        if (cardCost > _currentMana) return false;      
        return true;
    }
}
