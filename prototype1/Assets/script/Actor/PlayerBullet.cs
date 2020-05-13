using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Rigidbody2D bullet;

    void Start()
    {
        Invoke("bulletEnd", 2);
    }

    public void shot(bool faceRight){
        bullet = GetComponent<Rigidbody2D>();
        if(faceRight){
            bullet.velocity = new Vector2(30, 0);
        }else{
            bullet.velocity = new Vector2(-30, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        Debug.Log("hit sth");
        Destroy(gameObject);
    }

    private void bulletEnd(){
        Debug.Log("bullet time up");
        Destroy(gameObject);
    }
}
