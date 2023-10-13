using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private Button playButton, settingsButton, achievementsButton, quitButton;
    public void Awake()
    {
        playButton = GameObject.Find("PlayButton").gameObject.GetComponent<Button>();
        achievementsButton = GameObject.Find("AchievementsButton").gameObject.GetComponent<Button>();
        settingsButton = GameObject.Find("SettingsButton").gameObject.GetComponent<Button>();
        quitButton = GameObject.Find("QuitButton").gameObject.GetComponent<Button>();

        playButton.onClick.AddListener(() =>
        {
            //play
        });

        achievementsButton.onClick.AddListener(() =>
        {
            MenuManager.setActiveView(MenuManager.achievementsMenu);
        });

        settingsButton.onClick.AddListener(() =>
        {
            MenuManager.setActiveView(MenuManager.settingsMenu);
        });

        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}