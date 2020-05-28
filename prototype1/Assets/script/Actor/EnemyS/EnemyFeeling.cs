using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFeeling : MonoBehaviour
{
    public GameObject enemy;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "player"){
            if (enemy.GetComponent<EnemyBehavior>().enemyState.stateStr == "wander")
            {
                enemy.GetComponent<EnemyBehavior>().enemyState.stateStr = "chase";
                enemy.GetComponent<EnemyBehavior>().setPlayer(collider.gameObject.transform);
            }
        }
    }


}
