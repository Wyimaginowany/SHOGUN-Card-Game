using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private Button _playButton, _settingsButton, _achievementsButton, _quitButton,_introButton;
    public void Awake()
    {
        _playButton = GameObject.Find("PlayButton").gameObject.GetComponent<Button>();
        _achievementsButton = GameObject.Find("AchievementsButton").gameObject.GetComponent<Button>();
        _settingsButton = GameObject.Find("SettingsButton").gameObject.GetComponent<Button>();
        _quitButton = GameObject.Find("QuitButton").gameObject.GetComponent<Button>();
        _introButton = GameObject.Find("IntroButton").gameObject.GetComponent<Button>();

        _playButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });

        _achievementsButton.onClick.AddListener(() =>
        {
            MenuManager.setActiveView(MenuManager.achievementsMenu);
        });

        _settingsButton.onClick.AddListener(() =>
        {
            MenuManager.setActiveView(MenuManager.settingsMenu);
        });

        _quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        _introButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        });
    }
}