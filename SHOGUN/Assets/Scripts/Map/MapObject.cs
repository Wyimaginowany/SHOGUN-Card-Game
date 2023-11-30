using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    [SerializeField] private GameObject _mapCanvas;
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
        _mapCanvas.SetActive(false);
    }
    public void ShowMap(){
        _mapCanvas.SetActive(true);
    }
    public void SetCamera(Camera camera){
        _mapCanvas.GetComponent<Canvas>().worldCamera=camera;
    }

   
}
