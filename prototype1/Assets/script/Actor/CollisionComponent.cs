using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionComponent : ActorComponent
{
    private bool onGround;
    private bool airJump;
    private Transform groundCheck;
    private LayerMask ground;
    public override void Init(ActorRoot actor, string actorPath){
        base.Init(actor, actorPath);
        groundCheck = actor.linkerComponent.groundCheck;
        ground = LayerMask.NameToLayer("ground");
    }

    public override void Prepare(){
        base.Prepare();
    }

    public override void UnInit(){
        base.UnInit();
    }

    public override void Update(float deltaTime){
        base.Update(deltaTime);
    }

    public override void FixedUpdate(float fixedDeltaTime){
        base.FixedUpdate(fixedDeltaTime);
        onGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        Debug.Log(onGround);
        if(onGround){
            //Debug.Log("onGround");
            airJump = true;
        }
    }

    public bool getOnGround(){
        return onGround;
    }

    public bool getAirJump(){
        return airJump;
    }

    public void setAirJump(bool value){
        airJump = value;
    }
}
