using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _maxHealth = 100;

    [Header("To attach")]
    [SerializeField] private TMP_Text _healthAmountText;

    private int _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
        _healthAmountText.text = _currentHealth.ToString();
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            HandlePlayerDeath();
        }

        _healthAmountText.text = _currentHealth.ToString();
    }

    private void HandlePlayerDeath()
    {
        Debug.Log("Player has died. Returning player to full health");
        _currentHealth = _maxHealth;
    }
}
