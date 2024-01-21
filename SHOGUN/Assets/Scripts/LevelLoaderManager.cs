using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderManager : MonoBehaviour
{
    [SerializeField] private Animator _crossfadeCanvasAnimator;
    [SerializeField] private float _transitionTime = 1f;
    

    public static event Action OnSceneLoaded;
    public static event Action OnSceneReadyToPlay;
    public static event Action OnSceneTransitionBegin;
    public static event Action OnMainMenuLoading;

    public static GameObject LevelLoaderInstance;

    private void Awake()
    {
        if (LevelLoaderInstance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            LevelLoaderInstance = this.gameObject;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += ActiveSceneChanged;
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void OnDestroy()
    {
        if (LevelLoaderInstance != this.gameObject) return;

        SceneManager.activeSceneChanged -= ActiveSceneChanged;
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    private void ActiveSceneChanged(Scene currentScene, Scene nextScene)
    {
        if (nextScene.buildIndex == 0)
        {
            OnMainMenuLoading?.Invoke();
        }
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OnSceneLoaded?.Invoke();
        StartCoroutine(PrepareLevel());
    }

    IEnumerator PrepareLevel()
    {
        _crossfadeCanvasAnimator.SetBool("show", false);

        yield return new WaitForSeconds(_transitionTime);

        OnSceneReadyToPlay?.Invoke();
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadLevel(0));
    }

    public void LoadIntro()
    {
        StartCoroutine(LoadIntroScene());
    }

    public void LoadFirstLevel()
    {
        StartCoroutine(LoadLevel(1));
    }

    

    IEnumerator LoadIntroScene()
    {
        OnSceneTransitionBegin?.Invoke();
        _crossfadeCanvasAnimator.SetBool("show", true);

        yield return new WaitForSeconds(_transitionTime);

        SceneManager.LoadScene("Intro");
    }

    public void LoadNextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex + 2 >= SceneManager.sceneCountInBuildSettings)
        {
            LoadMainMenu();
            return;
        }

        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        OnSceneTransitionBegin?.Invoke();
        _crossfadeCanvasAnimator.SetBool("show", true);

        yield return new WaitForSeconds(_transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}