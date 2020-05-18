using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : Singleton<PlayerSoundManager>
{
    private AudioSource audioSource;
    private AudioClip slashAudio, parryAudio, shieldAudio;

    public void InitManager(AudioSource audioSource){
        this.audioSource = audioSource;
        slashAudio = Resources.Load<AudioClip>("soundRes/slashSound");
        parryAudio = Resources.Load<AudioClip>("soundRes/parrySound");
        shieldAudio = Resources.Load<AudioClip>("soundRes/shieldSound");
    }

    public void slashSound(){
        audioSource.clip = slashAudio;
        audioSource.Play();
    }

    public void parrySound(){
        audioSource.clip = parryAudio;
        audioSource.Play();
    }

    public void shieldSound(){
        audioSource.clip = shieldAudio;
        audioSource.Play();
    }
}
