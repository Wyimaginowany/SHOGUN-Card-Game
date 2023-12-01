using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject _pausePanel;
    public bool IsPaused => _pausePanel.activeSelf;

    public UnityEvent onGamePause;
    public UnityEvent onGameResume;


    public void Pause(){
        onGamePause.Invoke();
    }

    public void Resume(){
        onGameResume.Invoke();
    }

    public void MainMenu(){
        SceneManager.LoadScene("Menu");

    }
}
