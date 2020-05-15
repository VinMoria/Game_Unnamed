using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private float moveAxisVal = 0;
    public Dictionary<string, bool> btnsPressed = new Dictionary<string, bool>();

    public void InitManager()
    {
        btnsPressed.Add("btn0", false);
        btnsPressed.Add("btn1", false);
        btnsPressed.Add("btn2", false);
        btnsPressed.Add("btn3", false);
        btnsPressed.Add("axis3", false);
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

        BtnDownTrigger("btn0");
        BtnDownTrigger("btn1");
        BtnDownTrigger("btn2");
        BtnDownTrigger("btn3");
        BtnDownAxis("axis3");
    }

    private void BtnDownTrigger(string btnName){
        if(Input.GetButtonDown(btnName)){
            btnsPressed[btnName] = true;
        }
        if(Input.GetButtonUp(btnName)){
            btnsPressed[btnName] = false;
        }
    }

    private void BtnDownAxis(string axisName){
        if(Input.GetAxis(axisName)>0){
            btnsPressed[axisName] = true;
        }else{
            btnsPressed[axisName] = false;
        }
    }
}
