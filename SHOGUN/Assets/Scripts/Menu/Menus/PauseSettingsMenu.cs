using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseSettingsMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text volumeValueText = null;
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeValueText.text = volume.ToString("0.0");
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
    }
}
