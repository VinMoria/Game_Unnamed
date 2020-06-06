using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlash : MonoBehaviour
{
    public GameObject player;
    public AudioSource audioSource;
    private AudioClip slashClip;
    private GameObject slash;

    void Start()
    {
        slash = GetComponentsInChildren<Transform>()[1].gameObject;
        slash.SetActive(false);
        slashClip = Resources.Load<AudioClip>("soundRes/slashSound");
        audioSource.clip = slashClip;
    }

    private void slashEnd(){
        slash.SetActive(false);
    }

    public void slashOn(){
        audioSource.Play();
        slash.SetActive(true);
        Invoke("slashEnd", 0.3f);
    }
}
