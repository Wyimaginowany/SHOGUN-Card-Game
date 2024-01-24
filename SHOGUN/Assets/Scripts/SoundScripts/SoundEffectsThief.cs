using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsThief : MonoBehaviour
{
    [SerializeField] private AudioClip takeDamageClip;
    [SerializeField] private AudioClip blockClip;
    [SerializeField] private AudioClip kickClip;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip comboClip;

    [SerializeField] private GameObject blockFxEffect;
    
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

    public void playblockClipAudio(){
        audioSource.clip = blockClip;
        audioSource.Play();
    }

    public void playkickClipAudio(){
        audioSource.clip = kickClip;
        audioSource.Play();
    }
    public void playDeathClipAudio(){
        audioSource.clip = deathClip;
        audioSource.Play();
    }
    public void playComboClipAudio(){
        audioSource.clip = comboClip;
        audioSource.Play();
    }

    public void SpawnBuffBlockFX()
    {
        if(blockFxEffect==null)return;
        Instantiate(blockFxEffect, transform.position, Quaternion.identity);
    }

}
