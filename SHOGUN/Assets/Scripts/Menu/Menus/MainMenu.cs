using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private Button _playButton, _settingsButton, _creditsButton, _quitButton, _introButton;
    public void Awake()
    {
        _playButton = GameObject.Find("PlayButton").gameObject.GetComponent<Button>();
        _creditsButton = GameObject.Find("CreditsButton").gameObject.GetComponent<Button>();
        _settingsButton = GameObject.Find("SettingsButton").gameObject.GetComponent<Button>();
        _quitButton = GameObject.Find("QuitButton").gameObject.GetComponent<Button>();
        _introButton = GameObject.Find("IntroButton").gameObject.GetComponent<Button>();

        _playButton.onClick.AddListener(() =>
        {
            LevelLoaderManager.LevelLoaderInstance.GetComponent<LevelLoaderManager>().LoadNextScene();
        });

        _creditsButton.onClick.AddListener(() =>
        {
            MenuManager.SetActiveView(MenuManager.CreditsMenu);
        });

        _settingsButton.onClick.AddListener(() =>
        {
            MenuManager.SetActiveView(MenuManager.SettingsMenu);
        });

        _quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        _introButton.onClick.AddListener(() =>
        {
            LevelLoaderManager.LevelLoaderInstance.GetComponent<LevelLoaderManager>().LoadIntro();
        });
    }
}