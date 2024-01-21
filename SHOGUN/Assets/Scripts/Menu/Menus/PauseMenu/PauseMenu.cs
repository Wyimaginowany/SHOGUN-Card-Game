using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel, _settingsPanel,_pauseButtonObject,_musicObject;

    public UnityEvent onGamePause;
    public UnityEvent onGameResume;
    private Button _resumeButton, _settingsButton, _mainMenuButton, _backButton,_pauseButton;

    public void Start()
    {
        _resumeButton = _pausePanel.transform.Find("Resume Button").gameObject.GetComponent<Button>();
        _settingsButton = _pausePanel.transform.Find("Settings Button").gameObject.GetComponent<Button>();
        _mainMenuButton = _pausePanel.transform.Find("Main Menu Button").gameObject.GetComponent<Button>();
        _pauseButton = _pauseButtonObject.GetComponent<Button>();
        _backButton = _settingsPanel.transform.Find("Back Button").gameObject.GetComponent<Button>();
        


        _resumeButton.onClick.AddListener(() =>
        {
            _musicObject.GetComponent<MusicScript>().musicPauseToggler();
            onGameResume.Invoke();
            Time.timeScale=1;
            _pausePanel.SetActive(false);
            _pauseButtonObject.SetActive(true);
        });

        _pauseButton.onClick.AddListener(() =>
        {
            onGamePause.Invoke();
            Time.timeScale=0;
            _musicObject.GetComponent<MusicScript>().musicPauseToggler();
            _pausePanel.SetActive(true);
            _pauseButtonObject.SetActive(false);
        });

        _settingsButton.onClick.AddListener(() =>
        {
            _settingsPanel.SetActive(true);
            _pausePanel.SetActive(false);
        });

        _mainMenuButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            LevelLoaderManager.LevelLoaderInstance.GetComponent<LevelLoaderManager>().LoadMainMenu();
        });

        _backButton.onClick.AddListener(() =>
        {
            _pausePanel.SetActive(true);
            _settingsPanel.SetActive(false);
        });



    }

}
