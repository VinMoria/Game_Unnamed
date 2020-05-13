using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementComponent : ActorComponent
{
    private Rigidbody2D rigidBody;
    private float moveAxisVal = 0.0f;
    private int activedActionTimeKeeper = 0;
    private string activedActionTimeKeeperName; 
    private bool playerActionsFreezed;
    public int[] coldDownTimes = new int[3];

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
        coldDownTimes[0] = 0; //dash
        coldDownTimes[1] = 0; //bullet
        coldDownTimes[2] = 0; //slash
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
            Shot(fixedUpdateTime);
            Slash(fixedUpdateTime);
        }else{
            DashTimeKeep(fixedUpdateTime);
            ShotTimeKeep(fixedUpdateTime);
            SlashTimeKeep(fixedUpdateTime);
        }
        coldDown();
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

    private void Slash(float deltaTime){
        if(InputManager.Instance.btnsPressed["btn2"]&&coldDownTimes[2]==0){
            Vector2 pos;
            InputManager.Instance.btnsPressed["btn2"] = false;
            activedActionTimeKeeperName = "Slash";
            coldDownTimes[2] = 20;
            activedActionTimeKeeper = 0;
            playerActionsFreezed = true;
            rigidBody.velocity = new Vector2(0,0);
            rigidBody.gravityScale = 0;
            if(rigidBody.transform.localScale.x>0){
                pos = new Vector2(rigidBody.position.x+1.0f, rigidBody.position.y);
            }else{
                pos = new Vector2(rigidBody.position.x-1.0f, rigidBody.position.y);
            }

            GameObject slashObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/ActorPrefabs/playerSlash"));
            slashObject.transform.position = pos;
            slashObject.transform.name = "slash";
            PlayerSlash pbb = slashObject.GetComponent<PlayerSlash>();

            if(rigidBody.transform.localScale.x<0){
                slashObject.transform.localScale = new Vector2(-slashObject.transform.localScale.x, slashObject.transform.localScale.y);
            }
        }
    }

    private void SlashTimeKeep(float deltaTime){
        if(activedActionTimeKeeperName=="Slash"){
            activedActionTimeKeeper+=1;
            if(activedActionTimeKeeper == 10){
                gravityBack();
                stateNormal();
                activedActionTimeKeeper = 0;
                activedActionTimeKeeperName = "";
            }
        }
    }

    private void Shot(float deltaTime){
        if(InputManager.Instance.btnsPressed["btn3"]&&coldDownTimes[1]==0){
            Vector2 pos;
            InputManager.Instance.btnsPressed["btn3"] = false;
            activedActionTimeKeeperName = "Shot";
            coldDownTimes[1] = 50;
            activedActionTimeKeeper = 0;
            playerActionsFreezed = true;
            rigidBody.velocity = new Vector2(0,0);
            rigidBody.gravityScale = 0;
            if(rigidBody.transform.localScale.x>0){
                pos = new Vector2(rigidBody.position.x+0.8f, rigidBody.position.y);
            }else{
                pos = new Vector2(rigidBody.position.x-0.8f, rigidBody.position.y);
            }

            GameObject bulletObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/ActorPrefabs/playerBullet"));
            bulletObject.transform.position = pos;
            bulletObject.transform.name = "bullet";
            PlayerBullet pbb = bulletObject.GetComponent<PlayerBullet>();
            pbb.shot(rigidBody.transform.localScale.x>0);

            if(rigidBody.transform.localScale.x<0){
                bulletObject.transform.localScale = new Vector2(-bulletObject.transform.localScale.x, bulletObject.transform.localScale.y);
            }
        }
    }

    private void ShotTimeKeep(float deltaTime){
        if(activedActionTimeKeeperName=="Shot"){
            activedActionTimeKeeper+=1;
            if(activedActionTimeKeeper == 10){
                gravityBack();
                stateNormal();
                activedActionTimeKeeper = 0;
                activedActionTimeKeeperName = "";
            }
        }
    }
    
    private void DashTimeKeep(float deltaTime){
        if(activedActionTimeKeeperName=="Dash"){
            activedActionTimeKeeper+=1;
            if(activedActionTimeKeeper==10){
                moveStop();
            }else if(activedActionTimeKeeper==20){
                gravityBack();
                stateNormal();
                activedActionTimeKeeper = 0;
                activedActionTimeKeeperName = "";
            }
        }
    }
    
    private void Dash(float deltaTime){
        if(InputManager.Instance.btnsPressed["btn1"]&&(actor.collisionComponent.getOnGround()||actor.collisionComponent.getAirActions()["airDash"])&&coldDownTimes[0]==0){
            activedActionTimeKeeperName = "Dash";
            coldDownTimes[0] = 30;
            activedActionTimeKeeper = 0;
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

    void coldDown(){
        for(int i = 0;i<3;i++){
            if(coldDownTimes[i]>0){
                coldDownTimes[i]--;
            }
        }
    }
}