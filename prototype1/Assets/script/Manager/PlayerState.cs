using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : Singleton<PlayerState>
{
    public int activedActionTimeKeeper = 0;
    public string activedActionTimeKeeperName = "";
    public bool playerActionsFreezed = false;

    public bool defendOn = false;
}
