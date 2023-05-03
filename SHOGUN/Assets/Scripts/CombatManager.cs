using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private EnemyTest[] _enemies;
    [SerializeField] private Transform[] _spawnPoints;

    public static event Action OnPlayerTurnStart;
    public static event Action OnPlayerTurnEnd;

    private List<EnemyTest> _aliveEnemies = new List<EnemyTest>();

    private void Start()
    {
        EnemyTest.OnEnemyDeath += HandleEnemyDeath;
    }

    private void OnDestroy()
    {
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

        OnPlayerTurnStart?.Invoke();
    }

    public void PlayCard(int damage)
    {
        _aliveEnemies[0].TakeDamage(damage);
    }
}
