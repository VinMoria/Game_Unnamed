using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Transform groundcheck;
    public LayerMask ground;
    public float NORMAL_SPEED;
    public float jumpForce;
    private bool airJump;
    private const float SLOW_SPEED = 0;
    private const float BOOST_SPEED = 800;
    private const float STANDARD_GRAVITY = 6;
    private bool jumpPressed;
    private float moveAxis;
    private bool onGround;
    private Rigidbody2D player;
    InputController ic;
    InputState inputState;
    public GameObject mainController;
    void Start()
    {
        player = gameObject.GetComponent<Rigidbody2D>();
        ic = mainController.GetComponent<InputController>();
        inputState = new InputState();
    }

    void Update()
    {
        if(inputState.getState()=="normal"){
            if(ic.getBtnDown(0)){
                jumpPressed = true;
            }
            if(ic.getBtnUp(0)){
                jumpPressed = false;
            }
            moveAxis = ic.getAxis(0);
        }else{
            jumpPressed = false;
            moveAxis = 0;
        }
    }

    void FixedUpdate(){
        onGround = Physics2D.OverlapCircle(groundcheck.position, 0.1f, ground);
        if(onGround){
            airJump = true;
        }
        if(inputState.getState()=="normal"){
            Move(moveAxis);
            Jump();
        }
    }

    private void Move(float hori){
       if(hori>0){
           player.velocity = new Vector2(NORMAL_SPEED*Time.deltaTime, player.velocity.y);
       }else if(hori<0){
           player.velocity = new Vector2(-NORMAL_SPEED*Time.deltaTime, player.velocity.y);
       }else{
           player.velocity = new Vector2(0, player.velocity.y);
       }

       if(hori>0){
            transform.localScale = new Vector3(System.Math.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);
        }else if(hori<0){
            transform.localScale = new Vector3(-System.Math.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);
        }
    }

    private void Jump(){
        if(jumpPressed&&(onGround||airJump)){
            player.velocity = new Vector2(player.velocity.x, jumpForce);
            jumpPressed = false;
            if(!onGround){
                airJump = false;
            }
        }
    }
}
