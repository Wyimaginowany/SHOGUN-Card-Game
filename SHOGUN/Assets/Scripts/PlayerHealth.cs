using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CardEnums;
public class PlayerHealth : MonoBehaviour, IBleedable
{
    [Header("Settings")]
    [SerializeField] private int _maxHealth = 100;

    [Header("To attach")]
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private TMP_Text _healthAmountText;
    [SerializeField] private TMP_Text _shieldAmountText;
    [SerializeField] private Slider _healthbarSlider;
    [SerializeField] private Slider _shieldSlider;
    [Space(5)]
    [Header("Popups")]
    [SerializeField] private TMP_Text _damageText;
    [SerializeField] private TMP_Text _blockedDamageText;
    [SerializeField] private TMP_Text _healText;
    [SerializeField] private TMP_Text _shieldText;
    [SerializeField] private Animator _playerDamageAnimatorController;
    [SerializeField] private Animator _playerGainShieldAnimatorController;
    [SerializeField] private Animator _playerHealAnimatorController;
    [SerializeField] private Animator _playerBlockedAnimatorController;

    public static event Action OnPlayerDeath;
    public Animator GetAnimator(){ return _playerAnimator;}

    private int _currentShield = 0;
    private int _currentHealth;
    private int _currentBleedStacks = 0;


    private void Start()
    {
        _currentHealth = _maxHealth;

        UpdateHealthbar();
        UpdateShieldDisplay();

        CombatManager.OnPlayerTurnStart += HandlePlayerTurnStart;
        CombatManager.OnAllEnemiesKilled += HandleStageEnd;
    }

    private void OnDestroy()
    {
        CombatManager.OnPlayerTurnStart -= HandlePlayerTurnStart;
        CombatManager.OnAllEnemiesKilled -= HandleStageEnd;
    }

    private void HandleStageEnd()
    {
        _currentShield = 0;
        UpdateShieldDisplay();
    }

    private void HandlePlayerTurnStart()
    {
        _currentShield = 0;
        UpdateShieldDisplay();
        TakeBleedDamage();
    }

    public void TakeDamage(int damage)
    {
        int damageToPlayer = damage - _currentShield;

        if (_currentShield > 0)
        {
            if (damageToPlayer <= 0) DisplayBlockedPopup(damage);
            else DisplayBlockedPopup(_currentShield);

            _currentShield = Mathf.Clamp(_currentShield - damage, 0, 10000);
            UpdateShieldDisplay();
        }

        if (damageToPlayer > 0)
        {
            _currentHealth -= damageToPlayer;
            DisplayDamagePopup(damageToPlayer);
            UpdateHealthbar();
        }

    
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            HandlePlayerDeath();
            return;
        }

        _playerAnimator.SetTrigger("Take-damage");
        
    }

    public void HealPlayer(int healAmount)
    {
        if (_currentHealth + healAmount > _maxHealth)
        {
            _healText.text = "+"+(_maxHealth - _currentHealth).ToString();
            _playerHealAnimatorController.SetTrigger("heal");
            _currentHealth = _maxHealth;
        }
        else
        {
            _healText.text = "+"+(healAmount).ToString();
            
            _currentHealth += healAmount;
        }

        _playerHealAnimatorController.SetTrigger("heal");
        UpdateHealthbar();    
    }

    public void GiveShield(int shieldAmount)
    {
        _currentShield += shieldAmount;
        _shieldText.text = "+"+shieldAmount.ToString();
        _playerGainShieldAnimatorController.SetTrigger("shield");
        UpdateShieldDisplay();
    }

    private void UpdateHealthbar(){
        
        _healthAmountText.text = _currentHealth.ToString() + "/" + _maxHealth.ToString();
        _healthbarSlider.value = ((float) _currentHealth) / ((float)_maxHealth);
    }

    private void UpdateShieldDisplay()
    {
        _shieldAmountText.text = _currentShield.ToString();
        _shieldSlider.value = _currentShield;

        if (_currentShield > 0)
        {
            _shieldSlider.gameObject.SetActive(true);
            return;
        }

        _shieldSlider.gameObject.SetActive(false);
    }

    private void DisplayDamagePopup(int damage){
        _damageText.text = "-"+damage.ToString();
        _playerDamageAnimatorController.SetTrigger("damage");
    }

    private void DisplayBlockedPopup(int blockedAmount){
        _blockedDamageText.text = "-" + blockedAmount.ToString();
        _playerBlockedAnimatorController.SetTrigger("block");
    }

    private void HandlePlayerDeath()
    {
        OnPlayerDeath?.Invoke();
         _playerAnimator.SetTrigger("Death");
        Debug.Log("Player has died. Returning player to full health");
        _currentHealth = _maxHealth;
    }

    public int GetPlayerHealth()
    {
        return _currentHealth;
    }

    public int GetPlayerShield()
    {
        return _currentShield;
    }

    public void AddBleedStacks(int stackCount)
    {
        _currentBleedStacks += stackCount;
    }
    
    public int GetPlayerBleedStacks()
    {
        return _currentBleedStacks;
    }

    public void TakeBleedDamage()
    {
        if (_currentBleedStacks <= 0) return;

        TakeDamage(_currentBleedStacks);
        _currentBleedStacks--;
    }
}
