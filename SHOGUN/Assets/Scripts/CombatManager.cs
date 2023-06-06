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

    private int _currentMana;

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
        OnPlayerTurnEnd?.Invoke();
        HandleEnemyTurn();
    }

    private void HandleEnemyTurn()
    {
        foreach(EnemyTest enemy in _aliveEnemies)
        {
            enemy.HandleTurn();
        }

        //enemy turn end
        OnPlayerTurnStart?.Invoke();

        _currentMana = _maxMana;
        _manaAmountText.text = _currentMana.ToString();
    }

    public void HandleCardPlayed(Card cardPlayed, int slotIndex)
    {
        //cards will have own targets later
        _currentMana -= cardPlayed.GetCardCost();
        _manaAmountText.text = _currentMana.ToString();
        _aliveEnemies[0].TakeDamage(cardPlayed._value);
    }

    public bool HaveEnoughMana(int cardCost)
    {
        if (cardCost > _currentMana) return false;      
        return true;
    }
}
