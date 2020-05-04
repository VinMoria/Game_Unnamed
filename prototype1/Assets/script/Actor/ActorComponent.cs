using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorComponent
{
    public ActorRoot actor;

    void Init(ActorRoot actor, string actorPath) {
        this.actor = actor;
    }

    void Prepare() {
    }

    void UnInit() {
    }

    void Update(float deltaTime) {
    }
}
