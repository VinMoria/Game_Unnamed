using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private bool[] btnArray = new bool[8];
    private bool[] btnUpArray = new bool[8];
    private bool[] btnDownArray = new bool[8];
    private float[] axisArray = new float[7];
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0;i<7;i++){
            axisArray[i] = Input.GetAxisRaw("axis"+(i+1).ToString());
        }

        for(int i = 0;i<8;i++){
            btnArray[i] = Input.GetButton("btn"+i.ToString());
            btnUpArray[i] = Input.GetButtonUp("btn"+i.ToString());
            btnDownArray[i] = Input.GetButtonDown("btn"+i.ToString());
        }
    }

    public float getAxis(int index){
        return axisArray[index];
    }

    public bool getBtn(int index){
        return btnArray[index];
    }

    public bool getBtnUp(int index){
        return btnUpArray[index];
    }

    public bool getBtnDown(int index){
        return btnDownArray[index];
    }
}
