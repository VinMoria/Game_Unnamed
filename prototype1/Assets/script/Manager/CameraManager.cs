using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : Singleton<CameraManager>
{
    private ActorRoot targetActor;
    private Camera mainCam;
    private CinemachineBrain brain;
    private ICinemachineCamera virtualCam;

    public void InitManager(Transform virtualCamTrans)
    {
        mainCam = Camera.main;
        brain = mainCam.GetComponent<CinemachineBrain>();
        virtualCam = virtualCamTrans.GetComponent<CinemachineVirtualCamera>();
    }

    public void LookAtActor(ActorRoot actor)
    {
        if (virtualCam != null)
        {
            virtualCam.Follow = actor.GetTransform();
            targetActor = actor;
        }
        
    }

}
