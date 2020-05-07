using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ValueComponent : ActorComponent
{
    private const float tempMoveSpeed = 600.0f;
    private const float tempJumpForce = 20.0f;
    private const float tempGravityScale = 6.0f;
    private const float tempDashSpeed = 1700.0f;

    private float moveSpeed = 0.0f;
    private float jumpForce = 0.0f;
    private float gravityScale = 0.0f;
    private float dashSpeed = 0.0f;

    public override void Prepare()
    {
        base.Prepare();
        moveSpeed = tempMoveSpeed;
        jumpForce = tempJumpForce;
        gravityScale = tempGravityScale;
        dashSpeed = tempDashSpeed;
    }

    
    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }

        set
        {
            moveSpeed = value;
        }
    }

    public float JumpForce
    {
        get
        {
            return jumpForce;
        }

        set
        {
            jumpForce = value;
        }
    }

    public float GravityScale
    {
        get
        {
            return gravityScale;
        }

        set
        {
            gravityScale = value;
        }
    }

    public float DashSpeed
    {
        get
        {
            return dashSpeed;
        }

        set
        {
            dashSpeed = value;
        }
    }
}