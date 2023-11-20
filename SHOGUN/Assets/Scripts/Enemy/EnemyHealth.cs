using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _maxHealth;

    [Header("To Attach")]
    [SerializeField] private TMP_Text _damageText;
    [SerializeField] private Animator _popupAnimator;
    [SerializeField] private TMP_Text _healthbarAmountText;
    [SerializeField] private Slider _healthbarSlider;

    public static event Action<EnemyHealth> OnEnemyDeath;

    private int _currentHealth;
    private int _currentShield = 0;

    private void Start()
    {
        _currentHealth = _maxHealth;
        _healthbarAmountText.text = _currentHealth.ToString() + "/" + _maxHealth.ToString();
        _healthbarSlider.value = 1;
        
        CombatManager.OnPlayerTurnEnd += ResetShield;
    }
    
    private void ResetShield()
    {
        _currentShield = 0;
    }
    
    public void GiveShield(int shieldAmount)
    {
        _currentShield += shieldAmount;
    }
    
    public int GetEnemyShield()
    {
        return _currentShield;
    }

    public void TakeDamage(int damage)
    {
        _damageText.text = "-" + damage.ToString();
        _popupAnimator.SetTrigger("Take-damage");
        
        int damageToEnemy = damage - _currentShield;
        _currentShield = Mathf.Clamp(_currentShield - damage, 0, 10000);

        _currentHealth -= damageToEnemy;

        if (_currentHealth <= 0)
        {
            OnEnemyDeath?.Invoke(this);
            Destroy(gameObject);
        }

        _healthbarAmountText.text = _currentHealth.ToString() + "/" + _maxHealth.ToString();
        _healthbarSlider.value = ((float)_currentHealth) / ((float)_maxHealth);
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }

    public void SetCurrentHealth(int newHealth)
    {
        _currentHealth = newHealth;
    }
}
