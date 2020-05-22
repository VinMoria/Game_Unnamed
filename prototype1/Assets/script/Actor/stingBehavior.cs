using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stingBehavior : MonoBehaviour
{
    private Rigidbody2D player;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.transform.tag=="player"){
            player = collision.transform.GetComponent<Rigidbody2D>();
            player.gravityScale = 6.0f;
            PlayerState.Instance.playerActionsFreezed = true;
            PlayerState.Instance.activedActionTimeKeeperName = "shockUp";
            Invoke("shockUp", 0.1f);
            player.velocity = new Vector2(0, 600*Time.deltaTime);
        }
    }

    private void shockUp(){
        PlayerState.Instance.activedActionTimeKeeperName = "";
        PlayerState.Instance.playerActionsFreezed = false;
        player.gravityScale = 6.0f;
    }
}
