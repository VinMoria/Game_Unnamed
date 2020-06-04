using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stingBehavior : MonoBehaviour
{
    private Rigidbody2D player;
    List<DmgNDefItem> dmgList = new List<DmgNDefItem>();
    void Start()
    {
        dmgList.Add(new DmgNDefItem("phy", 20.0f));
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.transform.tag=="player"){
            player = collision.transform.GetComponent<Rigidbody2D>();
            player.gravityScale = 6.0f;
            player.velocity = new Vector2(0, 0);
            PlayerState.Instance.playerActionsFreezed = true;
            PlayerState.Instance.activedActionTimeKeeperName = "shockUp";
            Invoke("shockUp", 0.5f);
            PlayerState.Instance.hurt(dmgList);
        }
    }

    private void shockUp(){
        PlayerState.Instance.activedActionTimeKeeperName = "";
        PlayerState.Instance.playerActionsFreezed = false;
        player.gravityScale = 6.0f;
        if (!PlayerState.Instance.dead)
        {
            player.transform.position = PlayerState.Instance.stingDmgCheckPoint;
        }
    }
}
