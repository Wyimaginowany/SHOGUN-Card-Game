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
        }else{
        audioSource.clip = mapClip;
        audioSource.Play();
        }
    }


    public void playCombatMusicAudio(){
        if(combatClip!=null&&audioSource!=null&&audioSource.clip!=combatClip){
        audioSource.clip = combatClip;
        audioSource.Play();
        }
    }

    public void playMapMusicAudio(){
        if(mapClip!=null&&audioSource!=null&&audioSource.clip!=mapClip){
        audioSource.clip = mapClip;
        audioSource.Play();
        }
    }

    public void musicPauseToggler(){
        if(audioSource.isPlaying)
                 audioSource.Pause();
           else
                  audioSource.Play();
    }
}
