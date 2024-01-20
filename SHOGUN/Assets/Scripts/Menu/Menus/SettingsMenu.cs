using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text volumeMasterValueText = null;
    [SerializeField] private TMP_Text volumeMusicValueText = null;
    [SerializeField] private TMP_Text volumeSFXValueText = null;
    [SerializeField] private AudioMixer _audioMixer;
    private Button _backButton;
    public void Awake()
    {
        _backButton = GameObject.Find("BackButton").gameObject.GetComponent<Button>();

        _backButton.onClick.AddListener(() =>
        {
            MenuManager.SetActiveView(MenuManager.MainMenu);
        });
    }

    
    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeMasterValueText.text = volume.ToString("0.0");
        _audioMixer.SetFloat("Master", AudioListener.volume);
    }
    public void SetMusicVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeMusicValueText.text = volume.ToString("0.0");
        _audioMixer.SetFloat("Music", AudioListener.volume);
    }
    public void SetSFXVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeSFXValueText.text = volume.ToString("0.0");
        _audioMixer.SetFloat("SFX", AudioListener.volume);
    }
}
