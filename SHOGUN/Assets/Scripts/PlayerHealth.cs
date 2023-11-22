using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CardEnums;
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
    [SerializeField] private TMP_Text _shieldAmountText;

    public Animator GetAnimator(){ return _playerAnimator;}

    private int _currentShield = 0;
    private int _currentHealth;
    private void Start()
    {
        _currentHealth = _maxHealth;

        _healthbarAmountText.text = _currentHealth.ToString() + "/" + _maxHealth.ToString();
        _healthbarSlider.value=1;

        CombatManager.OnPlayerTurnStart += ResetShield;
    }

    private void OnDestroy()
    {
        CombatManager.OnPlayerTurnStart -= ResetShield;
    }

    private void ResetShield()
    {
        _shieldAmountText.text = "";
        _currentShield = 0;
    }

    public void TakeDamage(int damage)
    {
        int damageToPlayer = damage - _currentShield;
        _currentShield = Mathf.Clamp(_currentShield - damage, 0, 10000);
        UpdateShieldDisplay();
        Debug.Log("Player has taken:  "+ damage);
        
        if (damageToPlayer <= 0) {
            DisplayBlockedPopup();
            return;
        }

        DisplayDamagePopup(damageToPlayer);
        _currentHealth -= damageToPlayer;
    
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

    public void GiveShield(int shieldAmount)
    {
        _currentShield += shieldAmount;
        _damageText.text = "+"+shieldAmount.ToString();
        _popupAnimator.SetTrigger("Give-shield");
        UpdateShieldDisplay();
    }

    private void UpdateHealthbar(){
        _healthbarAmountText.text = _currentHealth.ToString() + "/" + _maxHealth.ToString();
        _healthbarSlider.value= ((float) _currentHealth) / ((float)_maxHealth);
    }
    private void UpdateShieldDisplay(){
        _shieldAmountText.text = _currentShield.ToString();
    }

    private void DisplayDamagePopup(int damage){
        _damageText.text = "-"+damage.ToString();
        _popupAnimator.SetTrigger("Take-damage");
    }

    private void DisplayBlockedPopup(){
        _damageText.text = "blocked";
        _popupAnimator.SetTrigger("Block-damage");
    }

    private void HandlePlayerDeath()
    {
         _playerAnimator.SetTrigger("Death");
        Debug.Log("Player has died. Returning player to full health");
        _currentHealth = _maxHealth;
    }

    public int GetPlayerShield()
    {
        return _currentShield;
    }
}
