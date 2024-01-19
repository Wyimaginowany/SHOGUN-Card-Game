using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointsGetter : MonoBehaviour
{
    [SerializeField] Transform[] _spawnPoints;

    public static SpawnPointsGetter Instance;

    private void Awake()
    {
        LevelLoaderManager.OnSceneLoaded += SetNewInstance;
    }

    private void OnDestroy()
    {
        LevelLoaderManager.OnSceneLoaded -= SetNewInstance;
    }

    private void SetNewInstance()
    {
        Instance = this;
    }

    public Transform[] GetNewSpawnPoints()
    {
        return _spawnPoints;
    }
}
