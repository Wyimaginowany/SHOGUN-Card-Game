using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsMenu : MonoBehaviour
{
    private Button _backButton;
    public void Awake()
    {
        _backButton = GameObject.Find("BackButton").gameObject.GetComponent<Button>();

        _backButton.onClick.AddListener(() =>
        {
            MenuManager.SetActiveView(MenuManager.MainMenu);
        });
    }

}
