﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlash : MonoBehaviour
{
    private GameObject slash;
    private bool hit = false;

    void Start()
    {
        slash = GetComponent<Transform>().gameObject;
        slash.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag=="Enemy"&&!hit){
            hit = true;
            Debug.Log("hit enemy");
            collision.gameObject.GetComponent<EnemyBehavior>().hurt();
        }
    }

    private void slashEnd(){
        slash.SetActive(false);
        hit = false;
    }

    public void slashOn(){
        slash.SetActive(true);
        Invoke("slashEnd", 0.2f);
    }
}
