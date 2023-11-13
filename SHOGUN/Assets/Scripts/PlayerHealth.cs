using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _maxHealth = 100;

    [Header("To attach")]
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private TMP_Text _damageText;
    [SerializeField] private Animator _popupAnimator;
    [SerializeField] private TMP_Text _healthbarAmountText;
    [SerializeField] private Slider _healthbarSlider;
    

    private int _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;

        _healthbarAmountText.text = _currentHealth.ToString() + "/" + _maxHealth.ToString();
        _healthbarSlider.value=1;
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
        else{
            _playerAnimator.SetTrigger("Take-damage");
        }
        UpdateHealthbar();
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
        UpdateHealthbar();

        
    }

    private void UpdateHealthbar(){
        _healthbarAmountText.text = _currentHealth.ToString() + "/" + _maxHealth.ToString();
        _healthbarSlider.value= ((float) _currentHealth) / ((float)_maxHealth);
    }

    private void HandlePlayerDeath()
    {
         _playerAnimator.SetTrigger("Death");
        Debug.Log("Player has died. Returning player to full health");
        _currentHealth = _maxHealth;
    }
}
