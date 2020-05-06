using UnityEngine;
using System.Collections;

public class ActorLinkerComponent : ActorComponent {

    public GameObject actorObj;

    public override void Init(ActorRoot actor, string actorPath) {
        base.Init(actor, actorPath);

        GameObject actorObjTemplate = Resources.Load<GameObject>(actorPath);
        if (!actorObjTemplate) {
            Debug.LogError("ActorLinkerComponent, Can not Find Actor Resources. " + actorPath);
        }


        actorObj = Object.Instantiate(actorObjTemplate);
        actorObj.transform.name = "HostPlayer";
        actorObj.transform.position = Vector3.zero;

    }

    public override void Prepare() {
        base.Prepare();
    }

    public override void UnInit() {
        base.UnInit();
        Object.Destroy(this.actorObj);
    }

    public override void Update(float deltaTime) {
        base.Update(deltaTime);
    }

    public void SetLocation(Vector3 newLocation) {
        if (this.actorObj != null) {
            this.actorObj.transform.position = newLocation;
        }
    }

    public Vector3 GetLocation() {
        if (this.actorObj != null) {
            return this.actorObj.transform.position;
        }

        Debug.LogError("Player Obj is NULL");
        return Vector3.zero;
    }

    public Transform GetTransform()
    {
        if (this.actorObj != null)
        {
            return this.actorObj.transform;
        }

        Debug.LogError("Player Obj is NULL");
        return null;
    }
}
