using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    public int stateIndex = 0;
    public float HP;
    public List<DmgNDefItem> dmgList = new List<DmgNDefItem>();
    public List<DmgNDefItem> defList = new List<DmgNDefItem>();
}
