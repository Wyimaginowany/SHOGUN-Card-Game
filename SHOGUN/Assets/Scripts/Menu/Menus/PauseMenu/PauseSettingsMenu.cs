using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseSettingsMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text volumeMasterValueText = null;
    [SerializeField] private TMP_Text volumeMusicValueText = null;
    [SerializeField] private TMP_Text volumeSFXValueText = null;

    [SerializeField] private AudioMixer _audioMixer;

    private void Awake() {
        LoadVolume();
    }
    public void SetMasterVolume(float volume)
    {
        volumeMasterValueText.text = volume.ToString("0.00");
        _audioMixer.SetFloat("Master",  Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        volumeMusicValueText.text = volume.ToString("0.00");
        _audioMixer.SetFloat("Music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetSFXVolume(float volume)
    {
        volumeSFXValueText.text = volume.ToString("0.00");
        _audioMixer.SetFloat("SFX",  Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    private void LoadVolume(){
        float master=PlayerPrefs.GetFloat("masterVolume");
        float music=PlayerPrefs.GetFloat("musicVolume");
        float sfx=PlayerPrefs.GetFloat("SFXVolume");
        SetMasterVolume(master);
        SetMusicVolume(music);
        SetSFXVolume(sfx);
        gameObject.transform.Find("MasterVolumeSlider").GetComponent<Slider>().value=master;
        gameObject.transform.Find("MusicVolumeSlider").GetComponent<Slider>().value=music;
        gameObject.transform.Find("SFXVolumeSlider").GetComponent<Slider>().value=sfx;

    }
    
}
