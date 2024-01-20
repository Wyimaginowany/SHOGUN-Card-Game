using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MenuManager
{
   public static GameObject Canvas = GameObject.Find("Canvas");
   public static GameObject MainMenu = Canvas.transform.Find("MainMenu").gameObject;
   public static GameObject CreditsMenu = Canvas.transform.Find("CreditsMenu").gameObject;
   public static GameObject SettingsMenu = Canvas.transform.Find("SettingsMenu").gameObject;

    public static void SetActiveView(GameObject canva)
    {
        canva.SetActive(true);
    }
}
