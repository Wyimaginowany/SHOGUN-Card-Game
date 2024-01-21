using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;

    public void SetMusicVolume(float volume)
    {
        _audioMixer.SetFloat("MusicVolume", volume);
    }
    public void SetSoundsVolume(float volume)
    {
        _audioMixer.SetFloat("SoundVolume", volume);
    }
}
