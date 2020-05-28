using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    public string stateStr = "";
    public float HP = 120;
    public List<DmgNDefItem> dmgList = new List<DmgNDefItem>();
    public List<DmgNDefItem> defList = new List<DmgNDefItem>();
    public int[] coldDownTime = new int[10];
}
