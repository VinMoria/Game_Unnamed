using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    private int HP;
    public Text HPText;
    void Update()
    {
        if(HP != PlayerState.Instance.HP){
            HP = PlayerState.Instance.HP;
            HPText.text = "HP:"+HP;
        }
    }
}
