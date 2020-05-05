using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorRoot {

    public int objId;

    public ActorLinkerComponent linkerComponent;

    private string actorPath;

    public ActorRoot(string actorPath) {
        this.actorPath = actorPath;

        linkerComponent = new ActorLinkerComponent();
    }

    public void Init() {
        linkerComponent.Init(this, actorPath);
    }

    public void Prepare() {
        linkerComponent.Prepare();
    }

    public void UnInit() {
        linkerComponent.UnInit();
    }

    public void Update(float deltaTime) {
        linkerComponent.Update(deltaTime);
    }

    public void SetLocation(Vector3 newLocation) {
        if (linkerComponent != null) {
            linkerComponent.SetLocation(newLocation);
        }
    }

    public Vector3 GetLocation() {
        if (linkerComponent != null) {

        }

        Debug.Log("Actor Linker Component is Null");
        return Vector3.zero;
    }
}
