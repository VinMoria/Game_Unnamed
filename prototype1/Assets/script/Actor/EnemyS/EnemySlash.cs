using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlash : MonoBehaviour
{
    private GameObject slash;
    public GameObject enemy;
    private Rigidbody2D player;
    private bool hit = false;

    void Start()
    {
        slash = GetComponent<Transform>().gameObject;
        slash.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.transform.tag=="player"&&!PlayerState.Instance.defendOn&&!hit){
            hit = true;
            player = collider.GetComponent<Rigidbody2D>();
            PlayerState.Instance.activedActionTimeKeeperName = "shockBack";
            player.gravityScale = 6.0f;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("player"), LayerMask.NameToLayer("enemy"), false);
            Invoke("shockEnd", 0.1f);
            PlayerState.Instance.playerActionsFreezed = true;
            
            if(enemy.transform.localScale.x>0){
                player.velocity = new Vector2(600*Time.deltaTime, 600*Time.deltaTime);
            }else{
                player.velocity = new Vector2(-600*Time.deltaTime, 600*Time.deltaTime);
            }

            PlayerState.Instance.hurt(enemy.GetComponent<EnemyBehavior>().enemyState.dmgList, false);

        }
        if(collider.transform.tag=="shield"&&!hit){
            hit = true;
            PlayerSoundManager.Instance.shieldSound();
            PlayerState.Instance.hurt(enemy.GetComponent<EnemyBehavior>().enemyState.dmgList, true);
        }
        if(collider.transform.tag=="parry"&&!hit){
            hit = true;
            PlayerSoundManager.Instance.parrySound();
        }
    }

    public void slashEnd(){
        slash.SetActive(false);
        hit = false;
    }

    public void slashOn(){
        slash.SetActive(true);
    }

    private void shockEnd(){
        PlayerState.Instance.activedActionTimeKeeperName = "";
        PlayerState.Instance.playerActionsFreezed = false;
        player.gravityScale = 6.0f;
    }
}
