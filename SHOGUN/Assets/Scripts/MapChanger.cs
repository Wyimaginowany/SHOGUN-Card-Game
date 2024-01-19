using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChanger : MonoBehaviour
{
    [SerializeField] GameObject[] _maps;


    private int _currentMapIndex = 0;

    public void LoadNextMap()
    {
        _currentMapIndex++;
        if (_currentMapIndex >= _maps.Length - 1) _currentMapIndex = 0;

        for (int i = 0; i < _maps.Length; i++)
        {
            if (i == _currentMapIndex)
            {
                _maps[i].SetActive(true);
            }
            else
            {
                _maps[i].SetActive(false);
            }
        }
    }
}
