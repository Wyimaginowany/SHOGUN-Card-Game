using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    [SerializeField] private GameObject _mapCanvas;
    [SerializeField] private GameObject _gameCanvas;

    public static MapObject MapInstance;

    public static event Action OnMapShow;

    private void Awake() {
        if (MapInstance != null){
            Destroy(gameObject);
        }else{
            
            MapInstance = this;

            DontDestroyOnLoad(gameObject);
        }
    }

    public void HideMap(){
        _gameCanvas.SetActive(true);
        _mapCanvas.SetActive(false);
        //if(!_startButton.active){
        //_gameCanvas.transform.Find("Card System Manager").GetComponent<CombatManager>().ResetMana();
        //_gameCanvas.transform.Find("Card System Manager").GetComponent<CombatManager>().SpawnNewEnemies();
        //_gameCanvas.transform.Find("Card System Manager").GetComponent<HandManager>().DrawFullHand();}
    }
    public void ShowMap(){
        _mapCanvas.SetActive(true);
        _gameCanvas.SetActive(false);
    }

   
}
