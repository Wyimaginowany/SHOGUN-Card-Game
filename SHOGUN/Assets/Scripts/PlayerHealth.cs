using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _maxHealth = 100;

    [Header("To attach")]
    [SerializeField] private TMP_Text _healthAmountText;
    [SerializeField] private TMP_Text _damageText;
    [SerializeField] private Animator _popupAnimator;
    

    private int _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
        _healthAmountText.text = _currentHealth.ToString();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("adssadasda");
        _damageText.text = "-"+damage.ToString();
        _popupAnimator.SetTrigger("Take-damage");

        
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            HandlePlayerDeath();
        }

        _healthAmountText.text = _currentHealth.ToString();
    }

    public void HealPlayer(int healAmount)
    {
        if (_currentHealth + healAmount > _maxHealth)
        {
            _damageText.text = "+"+(_maxHealth - _currentHealth).ToString();
            _popupAnimator.SetTrigger("Heal-self");
            _currentHealth = _maxHealth;
        }
        else
        {
            _damageText.text = "+"+(healAmount).ToString();
            
            _currentHealth += healAmount;
        }
        _popupAnimator.SetTrigger("Heal-self");
        _healthAmountText.text = _currentHealth.ToString();
    }

    private void HandlePlayerDeath()
    {
        Debug.Log("Player has died. Returning player to full health");
        _currentHealth = _maxHealth;
    }


}
