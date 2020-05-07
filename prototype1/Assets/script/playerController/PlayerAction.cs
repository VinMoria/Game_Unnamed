using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    InputController ic;
    InputState inputState;
    private Rigidbody2D player;
    public GameObject mainController;
    public bool teleportPressed;
    private const float STANDARD_GRAVITY = 6;
    public float TELEPORT_SPEED;
    void Start()
    {
        player = gameObject.GetComponent<Rigidbody2D>();
        ic = mainController.GetComponent<InputController>();
        inputState = new InputState();
    }

    void Update()
    {
        if(inputState.getState()=="normal"){
            teleportPress();
        }
    }

    void FixedUpdate(){
        teleport();
    }

    void teleportPress(){
        if(ic.getBtnDown(1)){
            teleportPressed = true;
        }
        if(ic.getBtnUp(1)){
            teleportPressed = false;
        }
    }

    void teleport(){
        if(teleportPressed){
            teleportPressed = false;
            inputState.setState("action");
            player.gravityScale = 0;
            if(player.transform.localScale.x>0){
                player.velocity = new Vector2(TELEPORT_SPEED*Time.deltaTime, 0);
            }else{
                player.velocity = new Vector2(-TELEPORT_SPEED*Time.deltaTime, 0);
            }
            Invoke("moveStop",(float)0.2);
            Invoke("gravityBack",(float)0.4);
            Invoke("stateNormal",(float)0.5);
        }
    }

    void moveStop(){
         player.velocity = new Vector2(0,0);
    }

    void gravityBack(){
        player.gravityScale = STANDARD_GRAVITY;
    }

    void stateNormal(){
        inputState.setState("normal");
    }
}
