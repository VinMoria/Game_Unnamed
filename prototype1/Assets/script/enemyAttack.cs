using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAttack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider){
        Debug.Log(collider.tag);
        playerController pc = collider.gameObject.GetComponent<playerController>();
        if(collider.tag == "shield"){
            pc.hurt(0);
        }else if(collider.tag == "parry"){
            pc.hurt(-1);
        }else if(collider.tag=="player"){
            pc.hurt(1);
        }
    }
}
