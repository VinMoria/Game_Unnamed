using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlash : MonoBehaviour
{
    private Rigidbody2D slash;

    void Start()
    {
        Invoke("slashEnd", 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.transform.tag=="player"){
            if(PlayerState.Instance.parry){
                Debug.Log("hit the parry");
                PlayerSoundManager.Instance.parrySound();
            }else if(PlayerState.Instance.shield){
                Debug.Log("hit the shield");
                PlayerSoundManager.Instance.shieldSound();
            }else{
                Debug.Log("hit the player");
            }
        }
    }

    private void slashEnd(){
        Destroy(gameObject);
    }
}
