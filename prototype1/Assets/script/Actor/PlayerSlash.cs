using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlash : MonoBehaviour
{
    public GameObject player;
    public AudioSource audioSource;
    private AudioClip slashClip;
    private GameObject slash;
    private bool hit = false;

    void Start()
    {
        slash = GetComponentsInChildren<Transform>()[1].gameObject;
        slash.SetActive(false);
        slashClip = Resources.Load<AudioClip>("soundRes/slashSound");
        audioSource.clip = slashClip;
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag=="Enemy"&&!hit){
            hit = true;
            Debug.Log("hit enemy");
            collision.gameObject.GetComponent<EnemyBehavior>().hurt(PlayerState.Instance.slashDmgList, "slash");
        }
    }

    private void slashEnd(){
        slash.SetActive(false);
        hit = false;
    }

    public void slashOn(){
        audioSource.Play();
        slash.SetActive(true);
        Invoke("slashEnd", 0.3f);
    }
}
