using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffects : MonoBehaviour
{
    [SerializeField] private AudioClip _attackClip;
    [SerializeField] private AudioClip _takeDamageClip;
    [SerializeField] private AudioClip _healClip;
    [SerializeField] private AudioClip _armorClip;
    [SerializeField] private AudioClip _deathClip;

    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource not found");
        }
    }
  
    public void playSingleTargetAttackAudio(){
        if (_attackClip == null) return;
        _audioSource.PlayOneShot(_attackClip);
    }

    public void playArmorAudio(){
        if (_armorClip == null) return;
        _audioSource.PlayOneShot(_armorClip);
    }

    public void playHealAudio(){
        if (_healClip == null) return;
        _audioSource.PlayOneShot(_healClip);
    }

    public void playTakeDamageAudio(){
        if (_takeDamageClip == null) return;
        _audioSource.PlayOneShot(_takeDamageClip);
    }

    public void playDeathAudio(){
        if (_deathClip == null) return;
        _audioSource.PlayOneShot(_deathClip);
    }

}
