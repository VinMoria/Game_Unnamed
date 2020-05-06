using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private float moveAxisVal = 0;

    public void InitManager()
    {

    }

    public void Update(float delta)
    {
        if (Input.GetAxisRaw("axis1") != moveAxisVal)
        {
            moveAxisVal = Input.GetAxisRaw("axis1");

            MoveAxisEventParam param;
            param.value = moveAxisVal;

            CGameEventManager.Instance.SendEvent<MoveAxisEventParam>(enGameEvent.MoveAxisEvent, ref param);
        }


    }
}
