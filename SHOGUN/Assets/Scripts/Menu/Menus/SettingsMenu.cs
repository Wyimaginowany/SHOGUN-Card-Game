using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text volumeValueText = null;
    [SerializeField] private Slider volumeSlider = null;
    private Button backButton;
    public void Awake()
    {
        backButton = GameObject.Find("BackButton").gameObject.GetComponent<Button>();

        backButton.onClick.AddListener(() =>
        {
            MenuManager.setActiveView(MenuManager.mainMenu);
        });
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeValueText.text = volume.ToString("0.0");
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
    }
}
