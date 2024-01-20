using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsPaperNinja : MonoBehaviour
{
    [SerializeField] private AudioClip vanishClip;
    [SerializeField] private AudioClip paperCutClip;
    [SerializeField] private AudioClip shurikenThrowClip;
    [SerializeField] private AudioClip takeDamageClip;
    [SerializeField] private AudioClip deathClip;
    
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource not found");
        }
    }

    void Update()
    {
        
    }
    
    public void playPaperCutAudio(){
        audioSource.clip = paperCutClip;
        audioSource.Play();
    }
    public void playShurikenThrowAudio(){
        audioSource.clip = shurikenThrowClip;
        audioSource.Play();
    }

    public void playVanishAudio(){
        audioSource.clip = vanishClip;
        audioSource.Play();
    }

    public void playTakeDamageAudio(){
        audioSource.clip = takeDamageClip;
        audioSource.Play();
    }
    
    public void playDeathAudio(){
        audioSource.clip = deathClip;
        audioSource.Play();
    }
}
