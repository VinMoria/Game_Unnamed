using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorComponent
{
    public ActorRoot actor;

    public virtual void Init(ActorRoot actor, string actorPath) {
        this.actor = actor;
    }

    public virtual void Prepare() {
    }

    public virtual void UnInit() {
        this.actor = null;
    }

    public virtual void Update(float deltaTime) {

    }

    public virtual void FixedUpdate(float fixedUpdateTime)
    {

    }
}
