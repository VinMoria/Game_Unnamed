using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBulletBehavior : MonoBehaviour
{
    private Rigidbody2D bullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        
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
}
