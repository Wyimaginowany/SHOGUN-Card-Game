using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffects : MonoBehaviour
{
    [SerializeField] private AudioClip attackClip;
    [SerializeField] private AudioClip takeDamageClip;
    [SerializeField] private AudioClip healClip;
    [SerializeField] private AudioClip armorClip;

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
    
    public void playSingleTargetAttackAudio(){
        audioSource.clip = attackClip;
        audioSource.Play();
    }

    public void playArmorAudio(){
        audioSource.clip = armorClip;
        audioSource.Play();
    }

    public void playHealAudio(){
        audioSource.clip = healClip;
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
