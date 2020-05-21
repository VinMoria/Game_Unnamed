using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlash : MonoBehaviour
{
    private GameObject slash;
    public Transform enemy;
    private Rigidbody2D player;

    void Start()
    {
        slash = GetComponent<Transform>().gameObject;
        slash.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.transform.tag=="player"&&!PlayerState.Instance.defendOn){
            player = collider.GetComponent<Rigidbody2D>();
            PlayerState.Instance.activedActionTimeKeeperName = "shockBack";
            player.gravityScale = 6.0f;
            Invoke("shockEnd", 0.1f);
            Debug.Log("state:shock");
            PlayerState.Instance.playerActionsFreezed = true;
            
            if(enemy.localScale.x>0){
                player.velocity = new Vector2(600*Time.deltaTime, 600*Time.deltaTime);
            }else{
                player.velocity = new Vector2(-600*Time.deltaTime, 600*Time.deltaTime);
            }

        }
        if(collider.transform.tag=="shield"){
            PlayerSoundManager.Instance.shieldSound();
        }
        if(collider.transform.tag=="parry"){
            PlayerSoundManager.Instance.parrySound();
        }
    }

    private void slashEnd(){
        slash.SetActive(false);
    }

    public void slashOn(){
        slash.SetActive(true);
        Invoke("slashEnd", 0.2f);
    }

    private void shockEnd(){
        Debug.Log("state:back");
        PlayerState.Instance.activedActionTimeKeeperName = "";
        PlayerState.Instance.playerActionsFreezed = false;
        player.gravityScale = 6.0f;
    }
}
