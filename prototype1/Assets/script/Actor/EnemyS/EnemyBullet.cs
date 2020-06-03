using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Vector3 targetPos;
    private bool findPlayer = false;
    private float angle;
    void Start()
    {
        Invoke("End", 1.0f);
    }

    public void setPlayerPosition(Vector3 playerPosition)
    {
        targetPos = playerPosition+10000*(playerPosition-transform.position);
        float x = (playerPosition - transform.position).x;
        float y = (playerPosition - transform.position).y;
        if(x == 0)
        {
            if (y > 0){angle = 0;}
            else{angle = 180;}
        }
        else
        {
            if (x < 0)
            {
                angle = Mathf.Atan(y / x) * 180 / Mathf.PI + 90;
                Debug.Log(angle);
            }
            else
            {
                angle = Mathf.Atan(y / x) * 180 / Mathf.PI - 90;
                Debug.Log(angle);
            }
            
        }
        findPlayer = true;
    }

    private void FixedUpdate()
    {
        if (findPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 0.4f);

            transform.localEulerAngles = new Vector3(0,0,angle);
        }
    }

    private void End()
    {
        Destroy(gameObject);
    }
}
