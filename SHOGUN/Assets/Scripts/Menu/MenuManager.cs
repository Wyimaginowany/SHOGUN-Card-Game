using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MenuManager
{
   public static GameObject canvas = GameObject.Find("Canvas");
   public static GameObject mainMenu = canvas.transform.Find("MainMenu").gameObject;
   public static GameObject achievementsMenu = canvas.transform.Find("AchievementsMenu").gameObject;
   public static GameObject settingsMenu = canvas.transform.Find("SettingsMenu").gameObject;

    public static void setActiveView(GameObject canva)
    {
        canva.SetActive(true);
    }
}
