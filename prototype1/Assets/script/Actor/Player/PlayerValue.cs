using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerValue : Singleton<PlayerValue>
{
    public  float MoveSpeed = 600.0f;
    public  float JumpForce = 20.0f;
    public  float GravityScale = 6.0f;
    public  float DashSpeed = 1700.0f;
}
