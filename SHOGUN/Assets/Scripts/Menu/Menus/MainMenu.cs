using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private Button _playButton, _settingsButton, _creditsButton, _quitButton, _introButton;
    
    [SerializeField] private AudioMixer _audioMixer;
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
    private void Start() {
        LoadAudioSettings();
    }

    private void LoadAudioSettings(){
        float master=PlayerPrefs.GetFloat("masterVolume");
        float music=PlayerPrefs.GetFloat("musicVolume");
        float sfx=PlayerPrefs.GetFloat("SFXVolume");
        _audioMixer.SetFloat("Music", Mathf.Log10(music)*20);
        _audioMixer.SetFloat("Master", Mathf.Log10(master)*20);
        _audioMixer.SetFloat("SFX", Mathf.Log10(sfx)*20);
    }
}