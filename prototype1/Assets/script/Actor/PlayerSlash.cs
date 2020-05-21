using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlash : MonoBehaviour
{
    private GameObject slash;

    void Start()
    {
        slash = GetComponent<Transform>().gameObject;
        slash.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision){
        Debug.Log("hit: "+collision.transform.tag);
    }

    private void slashEnd(){
        slash.SetActive(false);
    }

    public void slashOn(){
        slash.SetActive(true);
        Invoke("slashEnd", 0.2f);
    }
}
