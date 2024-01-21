using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    private void Awake() {
        LoadVolume();
    }

    public void SetMusicVolume(float volume)
    {
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicVolume",volume);
    }
    public void SetSoundsVolume(float volume)
    {
        _audioMixer.SetFloat("SoundVolume", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("soundsVolume", volume);
    }

    private void LoadVolume(){

        float music=PlayerPrefs.GetFloat("musicVolume");
        float sound=PlayerPrefs.GetFloat("soundsVolume");
        

        SetMusicVolume(music);
        SetSoundsVolume(sound);
        gameObject.transform.Find("MusicSlider").GetComponent<Slider>().value=music;
        gameObject.transform.Find("SoundsSlider").GetComponent<Slider>().value=sound;
    }
}
