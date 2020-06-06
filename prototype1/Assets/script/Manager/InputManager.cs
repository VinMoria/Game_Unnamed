using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager:MonoBehaviour
{
    public Dictionary<string, bool> btnsPressed = new Dictionary<string, bool>();
    private Dictionary<string, bool> axisBtn = new Dictionary<string, bool>();//获取LRT时的Flag
    public Dictionary<string, int> axisValue = new Dictionary<string, int>();
    private bool initFinish = false;

    public void Start()
    {
        if (!initFinish)
        {
            btnsPressed.Add("jumpBtn", false);
            btnsPressed.Add("dashBtn", false);
            btnsPressed.Add("slashBtn", false);
            btnsPressed.Add("shotBtn", false);
            btnsPressed.Add("defendBtn", false);
            btnsPressed.Add("interactBtn", false);
            axisBtn.Add("axis3", false);
            axisBtn.Add("axis3Rev", false);
            axisValue.Add("moveAxis", 0);
            initFinish = true;
        }
    }

    public void Update()
    {
        AxisSetValue("axis1", "moveAxis");
        BtnDownTrigger("btn0","jumpBtn");
        BtnDownTrigger("btn1","dashBtn");
        BtnDownTrigger("btn2","slashBtn");
        BtnDownTrigger("btn3","shotBtn");
        BtnDownAxis("axis3","defendBtn");
        BtnDownAxisRev("axis3Rev", "interactBtn");

        BtnDownTrigger("space","jumpBtn");
        BtnDownTrigger("shift","dashBtn");
        MouseDownTrigger(0,"slashBtn");
        BtnDownTrigger("e","shotBtn");
        BtnDownTrigger("x", "interactBtn");
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

    private void BtnDownAxisRev(string axisName, string ActName)
    {
        if (Input.GetAxis(axisName) < 0 && !axisBtn[axisName])
        {
            btnsPressed[ActName] = true;
            axisBtn[axisName] = true;
        }
        else if (Input.GetAxis(axisName) == 0 && axisBtn[axisName])
        {
            btnsPressed[ActName] = false;
            axisBtn[axisName] = false;
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

    private void AxisSetValue(string axisName, string ActName)
    {
        if (Input.GetAxisRaw(axisName) > 0)
        {
            axisValue[ActName] = 1;
        }else if (Input.GetAxisRaw(axisName) < 0)
        {
            axisValue[ActName] = -1;
        }
        else
        {
            axisValue[ActName] = 0;
        }
    }
}
