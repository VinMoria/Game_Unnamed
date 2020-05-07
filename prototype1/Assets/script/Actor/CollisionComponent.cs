using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionComponent : ActorComponent
{
    private bool onGround;
    private Dictionary<string, bool> airActions = new Dictionary<string, bool>();
    private Transform groundCheck;
    private LayerMask ground;
    public override void Init(ActorRoot actor, string actorPath){
        base.Init(actor, actorPath);
        groundCheck = actor.linkerComponent.groundCheck;
        ground = LayerMask.NameToLayer("ground");
        airActions.Add("airJump", false);
        airActions.Add("airDash", false);
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
        onGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, 1<<ground);
        if(onGround){
            airActions["airJump"] = true;
            airActions["airDash"] = true;
        }
    }

    public bool getOnGround(){
        return onGround;
    }

    public Dictionary<string, bool> getAirActions(){
        return airActions;
    }

    public void setAirActions(string name, bool value){
        airActions[name] = value;
    }
}
