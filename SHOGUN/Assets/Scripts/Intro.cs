using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    private Button _startGameButton;

    void Awake()
    {
        _startGameButton = GameObject.Find("StartGameButton").gameObject.GetComponent<Button>();

        _startGameButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        });
    }
}
