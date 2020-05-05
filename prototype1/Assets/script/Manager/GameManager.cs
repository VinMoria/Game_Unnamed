using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //TEMP Spawn point
    public Transform spawnPoint;
    //TEMP actor Path
    public string actorPath;

    public ActorRoot hostActor;

    public static GameManager Instance;

    public void Start() {
        DontDestroyOnLoad(this);
        Instance = this;

        CGameEventManager.Instance.InitManager();
        ActorManager.Instance.InitManager();


    }
}
