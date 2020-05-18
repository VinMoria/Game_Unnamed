using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : Singleton<PlayerSoundManager>
{
    public AudioSource audioSource;
    public AudioClip slashAudio, jumpAudio, shotAudio;

    public void jumpSound(){
        audioSource.clip = jumpAudio;
        audioSource.Play();
    }

    public void slashSound(){
        audioSource.clip = slashAudio;
        audioSource.Play();
    }

    public void shotSound(){
        audioSource.clip = shotAudio;
        audioSource.Play();
    }
}
