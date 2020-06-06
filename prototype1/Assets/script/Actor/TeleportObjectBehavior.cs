using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportObjectBehavior : MonoBehaviour
{
    public GameObject hint;
    private Transform player;
    public Transform teleportPoint;
    private InputManager inputManager;

    private void Start()
    {
        inputManager = GameObject.Find("GameManager").GetComponent<InputManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "player")
        {
            hint.SetActive(true);
            player = collision.gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
            hint.SetActive(false);
        }
    }

    private void Update()
    {
        if (hint.activeSelf)
        {
            if (inputManager.btnsPressed["interactBtn"])
            {
                player.position = teleportPoint.position;
            }
        }
    }
}
