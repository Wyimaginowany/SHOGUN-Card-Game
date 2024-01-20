using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsOni : MonoBehaviour
{
    [SerializeField] private AudioClip takeDamageClip;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip strongAttackClip;
    [SerializeField] private AudioClip stunAttackClip;
    [SerializeField] private AudioClip berserkClip;
    [SerializeField] private AudioClip buffDamageClip;
    
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
    
     public void playTakeDamageAudio(){
        audioSource.clip = takeDamageClip;
        audioSource.Play();
    }

    public void playDeathClipAudio(){
        audioSource.clip = deathClip;
        audioSource.Play();
    }
    public void playStrongAtackAudio(){
        audioSource.clip = strongAttackClip;
        audioSource.Play();
    }

    public void playStunAtackAudio(){
        audioSource.clip = stunAttackClip;
        audioSource.Play();
    }
    public void playBerserkAtackAudio(){
        audioSource.clip = berserkClip;
        audioSource.Play();
    }
    
    public void playBuffDamageAudio(){
        audioSource.clip = buffDamageClip;
        audioSource.Play();
    }



}
