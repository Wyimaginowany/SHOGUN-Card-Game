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

    private void Start()
    {
        _currentHealth = _maxHealth;
        _healthbarAmountText.text = _currentHealth.ToString() + "/" + _maxHealth.ToString();
        _healthbarSlider.value = 1;
    }

    public void TakeDamage(int damage)
    {
        _damageText.text = "-" + damage.ToString();
        _popupAnimator.SetTrigger("Take-damage");

        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            OnEnemyDeath?.Invoke(this);
            Destroy(gameObject);
        }

        _healthbarAmountText.text = _currentHealth.ToString() + "/" + _maxHealth.ToString();
        _healthbarSlider.value = ((float)_currentHealth) / ((float)_maxHealth);
    }
}
