using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _damage;

    [Header("To Attach")]
    [SerializeField] private TMP_Text _healthText;

    public static event Action<EnemyTest> OnEnemyDeath;

    private int _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
        _healthText.text = _currentHealth.ToString();
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            OnEnemyDeath?.Invoke(this);
            Destroy(gameObject);
        }

        _healthText.text = _currentHealth.ToString();
    }

    public void HandleTurn()
    {
        Debug.Log("Deal" + _damage + "to player");
    }
}
