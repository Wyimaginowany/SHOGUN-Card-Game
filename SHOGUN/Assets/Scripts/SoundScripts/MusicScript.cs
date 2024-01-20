using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    [SerializeField] private AudioClip combatClip;
    [SerializeField] private AudioClip mapClip;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource not found");
        }
        audioSource.clip = mapClip;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playCombatMusicAudio(){
        audioSource.clip = combatClip;
        audioSource.Play();
    }

    public void playMapMusicAudio(){
        if(mapClip!=null) audioSource.clip = mapClip;
        audioSource.Play();
    }

    public void musicPauseToggler(){
        if(audioSource.isPlaying)
                 audioSource.Pause();
           else
                  audioSource.Play();
    }
}
