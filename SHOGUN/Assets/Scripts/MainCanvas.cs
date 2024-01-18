using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    public static MainCanvas MainCanvasInstance;

    private void Awake()
    {
        if (MainCanvasInstance != null)
        {
            Destroy(gameObject);
        }
        else
        {

            MainCanvasInstance = this;

            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        LevelLoaderManager.OnMainMenuLoading += DestroyThisGameObject;
    }

    private void OnDestroy()
    {
        LevelLoaderManager.OnMainMenuLoading -= DestroyThisGameObject;
    }

    private void DestroyThisGameObject()
    {
        Destroy(gameObject);
    }
}
