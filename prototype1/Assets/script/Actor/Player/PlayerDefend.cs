using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefend : MonoBehaviour
{
    GameObject parry,shield;
    public AudioSource audioSource;
    private AudioClip shieldClip, parryClip;
    void Start()
    {
        shield = GetComponentsInChildren<Transform>()[1].gameObject;
        parry = GetComponentsInChildren<Transform>()[2].gameObject;
        shield.SetActive(false);
        parry.SetActive(false);
        parryClip = Resources.Load<AudioClip>("soundRes/parrySound");
        shieldClip = Resources.Load<AudioClip>("soundRes/shieldSound");
    }

    public void parrySound()
    {
        audioSource.clip = parryClip;
        audioSource.Play();
    }

    public void shieldSound()
    {
        audioSource.clip = shieldClip;
        audioSource.Play();
    }

    public void shieldOn(){
        shield.SetActive(true);
        PlayerState.Instance.defendOn = true;
    }

    public void shieldDown(){
        shield.SetActive(false);
        PlayerState.Instance.defendOn = false;
    }

    public void parryActive(){
        shield.SetActive(false);
        parry.SetActive(true);
        Invoke("parryDown",0.3f);
    }

    public void parryDown(){
        parry.SetActive(false);
        PlayerState.Instance.defendOn = false;
        PlayerState.Instance.playerActionsFreezed = false;
        PlayerState.Instance.activedActionTimeKeeperName = "";
    }
}
