using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    
    private Rigidbody2D rigidBody;
    public Transform groundCheck;
    private LayerMask ground;
    private Animator animator;
    private Animator curtainAnimator;
    private PlayerDefend playerDefend;
    private PlayerSlash playerSlash;
    
    //标记状态Flag
    private bool onGround = false;
    public int[] coldDownTimes = new int[4];
    private bool airDash, airJump;

    //
    private InputManager inputManager;
    public GameObject defendObject;
    public GameObject slashObject;

    void Start()
    {
        inputManager = GameObject.Find("GameManager").GetComponent<InputManager>();
        inputManager.Start();
        ground = LayerMask.NameToLayer("ground");
        Debug.Log(LayerMask.LayerToName(ground));
        PlayerState.Instance.Init();
        PlayerState.Instance.playerActionsFreezed = false;
        PlayerState.Instance.stingDmgCheckPoint = transform.position;
        curtainAnimator = GameObject.Find("curtain").GetComponent<Animator>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        coldDownTimes[0] = 0; //dash
        coldDownTimes[1] = 0; //bullet
        coldDownTimes[2] = 0; //slash
        coldDownTimes[3] = 0; //defend
        playerDefend = defendObject.GetComponent<PlayerDefend>();
        playerSlash = slashObject.GetComponent<PlayerSlash>();
    }

    private void Update()
    {
        inputManager.Update();
    }

    void FixedUpdate()
    {
        onGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, 1<<ground);
        if (onGround)
        {
            airJump = true;
            airDash = true;
        }

        if (!PlayerState.Instance.dead)
        {
            if (PlayerState.Instance.MP != PlayerState.Instance.MaxMP)
            {
                PlayerState.Instance.useMP(-0.05f);
            }
            if (!PlayerState.Instance.playerActionsFreezed)
            {
                Move();
                Jump();
                Dash();
                Shot();
                Slash();
                Defend();
                if (!onGround)
                {
                    AirAnimation();
                }
            }
            else
            {
                DashTimeKeep();
                ShotTimeKeep();
                SlashTimeKeep();
                DefendEnd();
            }
            coldDown();
        }
        else
        {
            animator.SetInteger("stateIndex", 8);
            moveStop();
            curtainAnimator.SetBool("curtainShow", false);
            Invoke("ReloadScene", 3.0f);
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene("Scene1");
    }

    private void Move()
    {
        if (inputManager.axisValue["moveAxis"] != 0)
        {
            if (onGround)
            {
                animator.SetInteger("stateIndex", 1);
            }
            rigidBody.velocity = new Vector2(inputManager.axisValue["moveAxis"] * PlayerState.Instance.MoveSpeed * Time.deltaTime, rigidBody.velocity.y);
        }
        else
        {
            animator.SetInteger("stateIndex", 0);
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }

        if (inputManager.axisValue["moveAxis"] > 0)
        {
            transform.localScale = new Vector3(System.Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (inputManager.axisValue["moveAxis"] < 0)
        {
            transform.localScale = new Vector3(-System.Math.Abs(transform.localScale.x), transform.localScale.y,transform.localScale.z);
        }
    }

    private void Jump()
    {
        if (inputManager.btnsPressed["jumpBtn"] && (onGround || airJump))
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, PlayerState.Instance.JumpForce);
            inputManager.btnsPressed["jumpBtn"] = false;
            if (!onGround)
            {
                airJump = false;
            }
        }
    }

    private void Slash()
    {
        if (inputManager.btnsPressed["slashBtn"] && coldDownTimes[2] == 0)
        {
            animator.SetInteger("stateIndex", 3);
            inputManager.btnsPressed["slashBtn"] = false;
            PlayerState.Instance.activedActionTimeKeeperName = "Slash";
            coldDownTimes[2] = 20;
            PlayerState.Instance.activedActionTimeKeeper = 0;
            PlayerState.Instance.playerActionsFreezed = true;
            rigidBody.velocity = new Vector2(0, 0);
            playerSlash.slashOn();
        }
    }

    private void SlashTimeKeep()
    {
        if (PlayerState.Instance.activedActionTimeKeeperName == "Slash")
        {
            PlayerState.Instance.activedActionTimeKeeper += 1;
            if (PlayerState.Instance.activedActionTimeKeeper == 10)
            {
                //gravityBack();
                stateNormal();
                PlayerState.Instance.activedActionTimeKeeper = 0;
                PlayerState.Instance.activedActionTimeKeeperName = "";
            }
        }
    }

    private void Shot()
    {
        if (inputManager.btnsPressed["shotBtn"] && coldDownTimes[1] == 0 && PlayerState.Instance.MP >= PlayerState.Instance.shotMP)
        {
            PlayerState.Instance.useMP(PlayerState.Instance.shotMP);
            inputManager.btnsPressed["shotBtn"] = false;
            PlayerState.Instance.activedActionTimeKeeperName = "Shot";
            coldDownTimes[1] = 50;
            PlayerState.Instance.activedActionTimeKeeper = 0;
            PlayerState.Instance.playerActionsFreezed = true;
            rigidBody.velocity = new Vector2(0, 0);
            rigidBody.gravityScale = 0;
            Vector2 pos;
            if (rigidBody.transform.localScale.x > 0)
            {
                pos = new Vector2(rigidBody.position.x + 0.8f, rigidBody.position.y);
            }
            else
            {
                pos = new Vector2(rigidBody.position.x - 0.8f, rigidBody.position.y);
            }

            GameObject bulletObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/ActorPrefabs/playerBullet"));
            bulletObject.transform.position = pos;
            bulletObject.transform.name = "bullet";
            PlayerBullet pbb = bulletObject.GetComponent<PlayerBullet>();
            pbb.shot(rigidBody.transform.localScale.x > 0);

            if (rigidBody.transform.localScale.x < 0)
            {
                bulletObject.transform.localScale = new Vector2(-bulletObject.transform.localScale.x, bulletObject.transform.localScale.y);
            }
        }
    }

    private void ShotTimeKeep()
    {
        if (PlayerState.Instance.activedActionTimeKeeperName == "Shot")
        {
            PlayerState.Instance.activedActionTimeKeeper += 1;
            if (PlayerState.Instance.activedActionTimeKeeper == 10)
            {
                gravityBack();
                stateNormal();
                PlayerState.Instance.activedActionTimeKeeper = 0;
                PlayerState.Instance.activedActionTimeKeeperName = "";
            }
        }
    }

    private void Defend()
    {
        if (inputManager.btnsPressed["defendBtn"] && coldDownTimes[3] == 0 && PlayerState.Instance.MP >= PlayerState.Instance.shieldMP)
        {
            PlayerState.Instance.activedActionTimeKeeperName = "Defend";
            PlayerState.Instance.activedActionTimeKeeper = 0;
            PlayerState.Instance.playerActionsFreezed = true;
            rigidBody.velocity = new Vector2(0, 0);
            playerDefend.shieldOn();
            animator.SetInteger("stateIndex", 4);
            coldDownTimes[3] = 40;
        }
    }

    private void DefendEnd()
    {
        if (PlayerState.Instance.MP == 0)
        {
            playerDefend.shieldDown();
            stateNormal();
        }
        if (PlayerState.Instance.activedActionTimeKeeperName == "Defend" || PlayerState.Instance.activedActionTimeKeeperName == "DefendHit")
        {
            PlayerState.Instance.activedActionTimeKeeper += 1;
            if (!inputManager.btnsPressed["defendBtn"])
            {
                if (PlayerState.Instance.activedActionTimeKeeper > 10)
                {
                    playerDefend.shieldDown();
                    stateNormal();
                }
                else
                {
                    if (PlayerState.Instance.activedActionTimeKeeperName == "Defend")
                    {
                        animator.SetInteger("stateIndex", 5);
                        playerDefend.parryActive();
                    }
                }
            }
        }
    }

    private void DashTimeKeep()
    {
        if (PlayerState.Instance.activedActionTimeKeeperName == "Dash")
        {
            PlayerState.Instance.activedActionTimeKeeper += 1;
            if (PlayerState.Instance.activedActionTimeKeeper == 10)
            {
                moveStop();
            }
            else if (PlayerState.Instance.activedActionTimeKeeper == 20)
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("player"), LayerMask.NameToLayer("enemy"), false);
                gravityBack();
                stateNormal();
                PlayerState.Instance.activedActionTimeKeeper = 0;
                PlayerState.Instance.activedActionTimeKeeperName = "";
            }
        }
    }

    private void Dash()
    {
        if (inputManager.btnsPressed["dashBtn"] && (onGround || airDash) && coldDownTimes[0] == 0)
        {
            PlayerState.Instance.activedActionTimeKeeperName = "Dash";
            animator.SetInteger("stateIndex", 2);
            coldDownTimes[0] = 30;
            PlayerState.Instance.activedActionTimeKeeper = 0;
            inputManager.btnsPressed["dashBtn"] = false;
            PlayerState.Instance.playerActionsFreezed = true;
            rigidBody.gravityScale = 0;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("player"), LayerMask.NameToLayer("enemy"), true);
            if (rigidBody.transform.localScale.x > 0)
            {
                rigidBody.velocity = new Vector2(PlayerState.Instance.DashSpeed * Time.deltaTime, 0);
            }
            else
            {
                rigidBody.velocity = new Vector2(-PlayerState.Instance.DashSpeed * Time.deltaTime, 0);
            }

            if (!onGround)
            {
                airDash = false;
            }
        }
    }

    void moveStop()
    {
        rigidBody.velocity = new Vector2(0, 0);
    }

    void gravityBack()
    {
        rigidBody.gravityScale = PlayerState.Instance.GravityScale;
    }

    void stateNormal()
    {
        PlayerState.Instance.playerActionsFreezed = false;
        PlayerState.Instance.activedActionTimeKeeperName = "";
    }

    void coldDown()
    {
        for (int i = 0; i < 4; i++)
        {
            if (coldDownTimes[i] > 0)
            {
                coldDownTimes[i]--;
            }
        }
    }

    private void AirAnimation()
    {
        if (rigidBody.velocity.y > 0)
        {
            animator.SetInteger("stateIndex", 6);
        }
        else if (rigidBody.velocity.y < 0)
        {
            animator.SetInteger("stateIndex", 7);
        }
    }
}
