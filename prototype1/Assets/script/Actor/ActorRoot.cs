using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorRoot {

    public int objId;

    public ActorLinkerComponent linkerComponent;
    public ValueComponent valueComponent;
    public MovementComponent movementComponent;

    private string actorPath;

    public ActorRoot(string actorPath) {
        this.actorPath = actorPath;

        linkerComponent = new ActorLinkerComponent();
        valueComponent = new ValueComponent();
        movementComponent = new MovementComponent();
    }

    public void Init() {
        linkerComponent.Init(this, actorPath);
        valueComponent.Init(this, actorPath);
        movementComponent.Init(this, actorPath);
    }

    public void Prepare() {
        linkerComponent.Prepare();
        valueComponent.Prepare();
        movementComponent.Prepare();
    }

    public void UnInit() {
        linkerComponent.UnInit();
        valueComponent.UnInit();
        movementComponent.UnInit();
    }

    public void Update(float deltaTime) {
        linkerComponent.Update(deltaTime);
        valueComponent.Update(deltaTime);
        movementComponent.Update(deltaTime);
    }

    public void FixedUpdate(float fixedUpdateTime)
    {
        linkerComponent.FixedUpdate(fixedUpdateTime);
        valueComponent.FixedUpdate(fixedUpdateTime);
        movementComponent.FixedUpdate(fixedUpdateTime);
    }


    public void SetLocation(Vector3 newLocation) {
        if (linkerComponent != null) {
            linkerComponent.SetLocation(newLocation);
        }
    }

    public Vector3 GetLocation() {
        if (linkerComponent != null) {
            return linkerComponent.GetLocation();
        }

        Debug.Log("Actor Linker Component is Null");
        return Vector3.zero;
    }

    public Transform GetTransform()
    {
        if (linkerComponent != null)
        {
            return linkerComponent.GetTransform();
        }

        Debug.Log("Actor Linker Component is Null");
        return null;
    }

    public T GetComponent<T>() where T : Component
    {
        if (linkerComponent != null)
        {
            return linkerComponent.actorObj.GetComponent<T>();
        }

        Debug.Log("Actor Linker Component is Null");
        return null;
    } 
}
