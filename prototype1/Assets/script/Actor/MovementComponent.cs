using UnityEngine;
using System.Collections;

public class MovementComponent : ActorComponent
{
    private Rigidbody2D rigidBody;
    private float moveAxisVal = 0.0f;
    private int timeKeeper = 0;
    private string timeKeeperName; 
    private bool playerActionsFreezed;

    public override void Init(ActorRoot actor, string actorPath)
    {
        base.Init(actor, actorPath);
        playerActionsFreezed = false;
        AddEventListener();
    }


    public override void Prepare()
    {
        base.Prepare();
        rigidBody = actor.GetComponent<Rigidbody2D>();
    }

    public override void UnInit()
    {
        base.UnInit();

        RmvEventListener();
    }

    private void AddEventListener()
    {
        CGameEventManager.Instance.AddEventHandler<MoveAxisEventParam>(enGameEvent.MoveAxisEvent, OnMoveAxisEvent);
    }

    private void RmvEventListener()
    {
        CGameEventManager.Instance.RmvEventHandler<MoveAxisEventParam>(enGameEvent.MoveAxisEvent, OnMoveAxisEvent);
    }


    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
    }

    public override void FixedUpdate(float fixedUpdateTime)
    {
        base.FixedUpdate(fixedUpdateTime);
        if(!playerActionsFreezed){
            Move(fixedUpdateTime);
            Jump(fixedUpdateTime);
            Dash(fixedUpdateTime);
        }else{
            DashTimeKeep(fixedUpdateTime);
        }
    }

    private void Move(float deltaTime)
    {
        float sign = moveAxisVal > 0 ? 1 : -1;

        if (moveAxisVal > 0)
        {
            rigidBody.velocity = new Vector2(sign * actor.valueComponent.MoveSpeed * deltaTime, rigidBody.velocity.y);
        }
        else if (moveAxisVal < 0)
        {
            rigidBody.velocity = new Vector2(sign * actor.valueComponent.MoveSpeed * deltaTime, rigidBody.velocity.y);
        }
        else
        {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }

        if (moveAxisVal > 0)
        {
            actor.GetTransform().localScale = new Vector3(System.Math.Abs(actor.GetTransform().localScale.x), actor.GetTransform().localScale.y, actor.GetTransform().localScale.z);
        }
        else if (moveAxisVal < 0)
        {
            actor.GetTransform().localScale = new Vector3(-System.Math.Abs(actor.GetTransform().localScale.x), actor.GetTransform().localScale.y, actor.GetTransform().localScale.z);
        }
    }

    private void Jump(float deltaTime){
        if(InputManager.Instance.btnsPressed["btn0"]&&(actor.collisionComponent.getOnGround()||actor.collisionComponent.getAirActions()["airJump"])){
            rigidBody.velocity = new Vector2(rigidBody.velocity.x,actor.valueComponent.JumpForce);
            InputManager.Instance.btnsPressed["btn0"] = false;
            if(!actor.collisionComponent.getOnGround()){
                actor.collisionComponent.setAirActions("airJump", false);
            }
        }
    }

    private void DashTimeKeep(float deltaTime){
        if(timeKeeperName=="Dash"){
            timeKeeper+=1;
            if(timeKeeper==10){
                moveStop();
            }else if(timeKeeper==20){
                gravityBack();
                stateNormal();
                timeKeeper = 0;
                timeKeeperName = "";
            }
        }
    }
    private void Dash(float deltaTime){
        if(InputManager.Instance.btnsPressed["btn1"]&&(actor.collisionComponent.getOnGround()||actor.collisionComponent.getAirActions()["airDash"])){
            timeKeeperName = "Dash";
            timeKeeper = 0;
            InputManager.Instance.btnsPressed["btn1"] = false;
            playerActionsFreezed = true;
            rigidBody.gravityScale = 0;
            if(rigidBody.transform.localScale.x>0){
                rigidBody.velocity = new Vector2(actor.valueComponent.DashSpeed*Time.deltaTime, 0);
            }else{
                rigidBody.velocity = new Vector2(-actor.valueComponent.DashSpeed*Time.deltaTime, 0);
            }

            if(!actor.collisionComponent.getOnGround()){
                actor.collisionComponent.setAirActions("airDash", false);
            }
        }
    }
    public void OnMoveAxisEvent(ref MoveAxisEventParam param)
    {
        if (actor == GameManager.Instance.hostActor)
        {
            moveAxisVal = param.value;
        }
    }

    void moveStop(){
         rigidBody.velocity = new Vector2(0,0);
    }

    void gravityBack(){
        rigidBody.gravityScale = actor.valueComponent.GravityScale;
    }

    void stateNormal(){
        playerActionsFreezed = false;
    }
}