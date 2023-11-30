using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishScreen: MonoBehaviour
{
    private Button _returnButton;
    [SerializeField] private GameObject _playerDeathScreen;

    private void OnDestroy()
    {
        PlayerHealth.OnPlayerDeath -= DisplayDeathScreen;
    }

    void Start()
    {
        _returnButton = GameObject.Find("ReturnButton").gameObject.GetComponent<Button>();
        _returnButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        });
        
        PlayerHealth.OnPlayerDeath += DisplayDeathScreen;
        _playerDeathScreen.SetActive(false);
    }

    void DisplayDeathScreen()
    {
        _playerDeathScreen.SetActive(true);
    }
    
    
}
