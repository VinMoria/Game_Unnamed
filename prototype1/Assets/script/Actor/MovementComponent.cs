using UnityEngine;
using System.Collections;

public class MovementComponent : ActorComponent
{
    private Rigidbody2D rigidBody;
    private float moveAxisVal = 0.0f;

    public override void Init(ActorRoot actor, string actorPath)
    {
        base.Init(actor, actorPath);

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
        Move(fixedUpdateTime);
        Jump(fixedUpdateTime);
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
        if(InputManager.Instance.jumpBtnPressed&&(actor.collisionComponent.getOnGround()||actor.collisionComponent.getAirJump())){
            rigidBody.velocity = new Vector2(rigidBody.velocity.x,actor.valueComponent.JumpForce);
            InputManager.Instance.jumpBtnPressed = false;
            if(!actor.collisionComponent.getOnGround()){
                actor.collisionComponent.setAirJump(false);
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

}