using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    [SerializeField] private GameObject _mapCanvas;
    [SerializeField] private GameObject _gameCanvas;
    public static GameObject MapInstance;
    private void Start() {
        
    }
    private void Awake() {
        if (MapInstance != null){
            Destroy(gameObject);
        }else{
            
            MapInstance=gameObject;

            DontDestroyOnLoad(gameObject);
        }
    }

    public void HideMap(){
        _gameCanvas.SetActive(true);
        _mapCanvas.SetActive(false);
    }
    public void ShowMap(){
        _mapCanvas.SetActive(true);
        _gameCanvas.SetActive(false);
    }
    


    // public void SetCamera(Camera camera){
    //     _mapCanvas.GetComponent<Canvas>().worldCamera=camera;
    // }

   
}
