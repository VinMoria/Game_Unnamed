using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StingCheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
            PlayerState.Instance.stingDmgCheckPoint = transform.position;
        }
    }
}
