using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlash : MonoBehaviour
{
    private Rigidbody2D slash;

    void Start()
    {
        Invoke("slashEnd", 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D collision){
        Debug.Log("hit "+collision.transform.tag);
    }

    private void slashEnd(){
        Debug.Log("slash time up");
        Destroy(gameObject);
    }
}
