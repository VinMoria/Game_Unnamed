using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ValueComponent : ActorComponent
{
    private const float tempMoveSpeed = 600.0f;

    private float moveSpeed = 0.0f;

    public override void Prepare()
    {
        base.Prepare();

        moveSpeed = tempMoveSpeed;
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
}