using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : Singleton<ActorManager> {

    public int curObjId;
    public Dictionary<int, ActorRoot> actorDict;

    public void InitManager() {
        curObjId = 0;
        actorDict = new Dictionary<int, ActorRoot>();
    }


    public ActorRoot CreateActor(string actorPath) {
        ActorRoot newActor = new ActorRoot(actorPath);
        newActor.objId = curObjId;
        newActor.Init();
        newActor.Prepare();
        actorDict.Add(curObjId, newActor);
        curObjId++;

        return newActor;
    }

    public ActorRoot GetActor(int objId) {
        ActorRoot actor = null;
        actorDict.TryGetValue(objId, out actor);

        return actor;
    }
}