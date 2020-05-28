using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private float moveAxisVal = 0;
    public Dictionary<string, bool> btnsPressed = new Dictionary<string, bool>();
    private Dictionary<string, bool> axisBtn = new Dictionary<string, bool>();

    public void InitManager()
    {
        btnsPressed.Add("jumpBtn", false);
        btnsPressed.Add("dashBtn", false);
        btnsPressed.Add("slashBtn", false);
        btnsPressed.Add("shotBtn", false);
        btnsPressed.Add("defendBtn", false);
        axisBtn.Add("axis3", false);
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

        BtnDownTrigger("btn0","jumpBtn");
        BtnDownTrigger("btn1","dashBtn");
        BtnDownTrigger("btn2","slashBtn");
        BtnDownTrigger("btn3","shotBtn");
        BtnDownAxis("axis3","defendBtn");

        BtnDownTrigger("space","jumpBtn");
        BtnDownTrigger("shift","dashBtn");
        MouseDownTrigger(0,"slashBtn");
        BtnDownTrigger("q","shotBtn");
        MouseDownTrigger(1,"defendBtn");
    }

    private void BtnDownTrigger(string btnName,string ActName){
        if(Input.GetButtonDown(btnName)){
            btnsPressed[ActName] = true;
        }
        if(Input.GetButtonUp(btnName)){
            btnsPressed[ActName] = false;
        }
    }

    private void BtnDownAxis(string axisName, string ActName){
        if(Input.GetAxis(axisName)>0&&!axisBtn[axisName]){
            btnsPressed[ActName] = true;
            axisBtn[axisName] = true;
        }else if(Input.GetAxis(axisName)==0&&axisBtn[axisName]){
            btnsPressed[ActName] = false;
            axisBtn[axisName] = false;
        }
    }

    private void KeyDownTrigger(string KeyName, string ActName){
        if(Input.GetKeyDown(KeyName)){
            btnsPressed[ActName] = true;
        }
        if(Input.GetKeyUp(KeyName)){
            btnsPressed[ActName] = false;
        }
    }

    private void MouseDownTrigger(int MouseName, string ActName){
        if(Input.GetMouseButtonDown(MouseName)){
            btnsPressed[ActName] = true;
        }
        if(Input.GetMouseButtonUp(MouseName)){
            btnsPressed[ActName] = false;
        }
    }
}
