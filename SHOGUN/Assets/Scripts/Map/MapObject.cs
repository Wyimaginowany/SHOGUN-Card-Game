using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    [SerializeField] private GameObject _mapCanvas, _gameCanvas, _musicObject;

    public static MapObject MapInstance;

    public static event Action OnMapShow;
    
    private AudioSource audioSource;

    private void Awake() {
        if (MapInstance != null){
            Destroy(gameObject);
        }else{
            
            MapInstance = this;

            DontDestroyOnLoad(gameObject);
        }
        
        audioSource = GetComponent<AudioSource>();
    }

    public void HideMap(){
        _musicObject.GetComponent<MusicScript>().playCombatMusicAudio();
        _gameCanvas.SetActive(true);
        _mapCanvas.SetActive(false);
    }
    public void ShowTreasure(){
        _gameCanvas.SetActive(true);
        _mapCanvas.SetActive(false);
    }
    public void ShowMap(){
        _musicObject.GetComponent<MusicScript>().playMapMusicAudio();
        _mapCanvas.SetActive(true);
        _gameCanvas.SetActive(false);
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

    internal void PlayEventSound(AudioClip clip)
    {
        if(audioSource!=null){
            audioSource.PlayOneShot(clip);
        }
        
    }
}
