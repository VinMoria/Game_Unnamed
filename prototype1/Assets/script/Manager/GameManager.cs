using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //TEMP Spawn point
    public Transform spawnPoint;
    //TEMP actor Path
    public string actorPath;
    public AudioSource audioSource;
    public Transform virtualCam;


    public ActorRoot hostActor;

    public static GameManager Instance;

    public void Start() {
        DontDestroyOnLoad(this);
        Instance = this;
        PlayerSoundManager.Instance.InitManager(audioSource);
        CGameEventManager.Instance.InitManager();
        InputManager.Instance.InitManager();
        ActorManager.Instance.InitManager();
        CameraManager.Instance.InitManager(virtualCam);

        StartGame();
    }

    public void Update()
    {
        InputManager.Instance.Update(Time.deltaTime);
        ActorManager.Instance.Update(Time.deltaTime);
    }

    public void FixedUpdate()
    {
        ActorManager.Instance.FixedUpdate(Time.deltaTime);
    }

    private void StartGame()
    {
        ActorRoot actor = ActorManager.Instance.CreateActor(actorPath);
        actor.SetLocation(spawnPoint.transform.position);
        hostActor = actor;
        CameraManager.Instance.LookAtActor(actor);

        LinkEnemy(actor);
    }

    private void LinkEnemy(ActorRoot actor)
    {
        GameObject[] obj = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject child in obj)    //遍历所有gameobject
        {
            if (child.gameObject.tag == "Enemy")
            {
                child.GetComponent<EnemyBehavior>().LinkPlayer(actor.GetTransform());
            }
        }
    }
}
