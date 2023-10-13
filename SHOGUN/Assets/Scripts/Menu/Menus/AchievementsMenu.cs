using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsMenu : MonoBehaviour
{
    private Button backButton;
    public void Awake()
    {
        backButton = GameObject.Find("BackButton").gameObject.GetComponent<Button>();

        backButton.onClick.AddListener(() =>
        {
            MenuManager.setActiveView(MenuManager.mainMenu);
        });
    }

}
