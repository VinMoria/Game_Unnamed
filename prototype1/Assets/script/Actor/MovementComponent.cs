using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementComponent : ActorComponent
{
    private Rigidbody2D rigidBody;
    GameObject defendObject,slashObject;
    Vector2 pos;
    private PlayerDefend playerDefend;
    private PlayerSlash playerSlash;
    private float moveAxisVal = 0.0f;
    
    public int[] coldDownTimes = new int[4];

    public override void Init(ActorRoot actor, string actorPath)
    {
        base.Init(actor, actorPath);
        PlayerState.Instance.playerActionsFreezed = false;
        AddEventListener();
    }

    public override void Prepare()
    {
        base.Prepare();
        rigidBody = actor.GetComponent<Rigidbody2D>();
        coldDownTimes[0] = 0; //dash
        coldDownTimes[1] = 0; //bullet
        coldDownTimes[2] = 0; //slash
        coldDownTimes[3] = 0; //defend
        defendObject = rigidBody.GetComponentsInChildren<Transform>()[2].gameObject;
        slashObject = rigidBody.GetComponentsInChildren<Transform>()[5].gameObject;
        playerDefend = defendObject.GetComponent<PlayerDefend>();
        playerSlash = slashObject.GetComponent<PlayerSlash>();
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
        if(!PlayerState.Instance.playerActionsFreezed){
            Move(fixedUpdateTime);
            Jump(fixedUpdateTime);
            Dash(fixedUpdateTime);
            Shot(fixedUpdateTime);
            Slash(fixedUpdateTime);
            Defend(fixedUpdateTime);
        }else{
            DashTimeKeep(fixedUpdateTime);
            ShotTimeKeep(fixedUpdateTime);
            SlashTimeKeep(fixedUpdateTime);
            DefendEnd(fixedUpdateTime);
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
            InputManager.Instance.btnsPressed["btn2"] = false;
            PlayerState.Instance.activedActionTimeKeeperName = "Slash";
            coldDownTimes[2] = 20;
            PlayerState.Instance.activedActionTimeKeeper = 0;
            PlayerState.Instance.playerActionsFreezed = true;
            rigidBody.velocity = new Vector2(0,0);
            //rigidBody.gravityScale = 0;
            PlayerSoundManager.Instance.slashSound();
            playerSlash.slashOn();
        }
    }

    private void SlashTimeKeep(float deltaTime){
        if(PlayerState.Instance.activedActionTimeKeeperName=="Slash"){
            PlayerState.Instance.activedActionTimeKeeper+=1;
            if(PlayerState.Instance.activedActionTimeKeeper == 10){
                //gravityBack();
                stateNormal();
                PlayerState.Instance.activedActionTimeKeeper = 0;
                PlayerState.Instance.activedActionTimeKeeperName = "";
            }
        }
    }

    private void Shot(float deltaTime){
        if(InputManager.Instance.btnsPressed["btn3"]&&coldDownTimes[1]==0){
            InputManager.Instance.btnsPressed["btn3"] = false;
            PlayerState.Instance.activedActionTimeKeeperName = "Shot";
            coldDownTimes[1] = 50;
            PlayerState.Instance.activedActionTimeKeeper = 0;
            PlayerState.Instance.playerActionsFreezed = true;
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
        if(PlayerState.Instance.activedActionTimeKeeperName=="Shot"){
            PlayerState.Instance.activedActionTimeKeeper+=1;
            if(PlayerState.Instance.activedActionTimeKeeper == 10){
                gravityBack();
                stateNormal();
                PlayerState.Instance.activedActionTimeKeeper = 0;
                PlayerState.Instance.activedActionTimeKeeperName = "";
            }
        }
    }
    
    private void Defend(float deltaTime){
        if(InputManager.Instance.btnsPressed["axis3"]&&coldDownTimes[3]==0){
            PlayerState.Instance.activedActionTimeKeeperName = "Defend";
            PlayerState.Instance.activedActionTimeKeeper = 0;
            PlayerState.Instance.playerActionsFreezed = true;
            rigidBody.velocity = new Vector2(0,0);
            playerDefend.shieldOn();
            coldDownTimes[3] = 40;
        }
    }

    private void DefendEnd(float deltaTime){
        if(PlayerState.Instance.activedActionTimeKeeperName=="Defend"){
            PlayerState.Instance.activedActionTimeKeeper+=1;
            if(!InputManager.Instance.btnsPressed["axis3"]){
                if(PlayerState.Instance.activedActionTimeKeeper>10){
                    playerDefend.shieldDown();
                }else{
                    playerDefend.parryActive();
                }
                stateNormal();
            }
        }
    }

    private void DashTimeKeep(float deltaTime){
        if(PlayerState.Instance.activedActionTimeKeeperName=="Dash"){
            PlayerState.Instance.activedActionTimeKeeper+=1;
            if(PlayerState.Instance.activedActionTimeKeeper==10){
                moveStop();
            }else if(PlayerState.Instance.activedActionTimeKeeper==20){
                gravityBack();
                stateNormal();
                PlayerState.Instance.activedActionTimeKeeper = 0;
                PlayerState.Instance.activedActionTimeKeeperName = "";
            }
        }
    }
    
    private void Dash(float deltaTime){
        if(InputManager.Instance.btnsPressed["btn1"]&&(actor.collisionComponent.getOnGround()||actor.collisionComponent.getAirActions()["airDash"])&&coldDownTimes[0]==0){
            PlayerState.Instance.activedActionTimeKeeperName = "Dash";
            coldDownTimes[0] = 30;
            PlayerState.Instance.activedActionTimeKeeper = 0;
            InputManager.Instance.btnsPressed["btn1"] = false;
            PlayerState.Instance.playerActionsFreezed = true;
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
        PlayerState.Instance.playerActionsFreezed = false;
        PlayerState.Instance.activedActionTimeKeeperName = "";
    }
    
    void coldDown(){
        for(int i = 0;i<4;i++){
            if(coldDownTimes[i]>0){
                coldDownTimes[i]--;
            }
        }
    }

}