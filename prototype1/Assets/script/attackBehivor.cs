using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackBehivor : MonoBehaviour
{
    // Start is called before the first frame update
    private Collider2D attCollider;
    bool hitTarget;
    void Start()
    {
        attCollider = GetComponent<Collider2D>();
        hitTarget = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag=="Enemy"&&!hitTarget){
            enemyEvent ee = collider.gameObject.GetComponent<enemyEvent>();
            ee.hurt(1);
            hitTarget = true;
            
        }
    }
}
