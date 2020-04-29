using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    public Image qiuckItemImg;
    public Text quickItemText;
    private Rigidbody2D player;
    public arrayInventory quickUseBar;
    private int quickUseIndex = 0;
    private const float NORMAL_SPEED = 300;
    private const float SLOW_SPEED = 0;
    private const float BOOST_SPEED = 800;
    private const float STANDARD_GRAVITY = 5;
    private Collider2D playerCollider;
    private Animator anim;
    private Collider2D platform;

    public float jumpForce, speed;
    public Transform groundcheck;
    public Transform wallcheck;
    public LayerMask ground;

    public GameObject attackPre;
    public GameObject bulletPre;
    public GameObject shieldPre;

    private float startTime, endTime;

    private GameObject shieldObject=null;
    private GameObject slashObject=null;

    private bool onGround, onWall,leaveWall,needStop,hitLeft, hitRight,hitDown,hitUp;
    private bool jumpPressed, slashPressed, teleportPressed, bulletPressed;
    private bool isJump, playerOnPlatform, haveShield, downIsDown;
    public bool jumpFreeze, walkFreeze,slashFreeze,shieldFreeze,teleportFreeze, bulletFreeze;

    public Image HPImg;

    int jumpCount = 2;

    private bool hitGround;

    private bool AirAttack = true;
    private bool AirTeleport = true;

    void Start()
    {
        leaveWall = false;
        teleportPressed = false;
        slashPressed = false;
        bulletPressed = false;
        onWall = false;
        downIsDown = false;
        onGround = false;
        jumpPressed = false;
        isJump = false;
        playerOnPlatform = false;
        haveShield = false;
        jumpFreeze = false;
        walkFreeze = false;
        shieldFreeze = false;
        teleportFreeze = false;
        bulletFreeze = false;
        startTime = 0;
        endTime = 0;
        hitLeft = false;
        hitRight = false;
        hitUp = false;
        hitDown = false;
        player = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        if(quickUseBar.itemArray[quickUseIndex]==null){
            showNextQuickItem();
        }
        refreshQuickBar();
    }
    
    void Update()
    {
        if(Input.GetButtonDown("btnX")){
            slashPressed = true;
        }
        if(Input.GetButtonUp("btnX")){
            slashPressed = false;
        }
        if(Input.GetButtonDown("btnY")){
            bulletPressed = true;
        }
        if(Input.GetButtonUp("btnY")){
            bulletPressed = false;
        }
        if(Input.GetButtonDown("btnB")){
            teleportPressed = true;
        }
        if(Input.GetButtonUp("btnB")){
            teleportPressed = false;
        }

        if(Input.GetButtonDown("btnA")){
            jumpPressed = true;
        }
        if(Input.GetButtonUp("btnA")){
            jumpPressed = false;
        }
        if(Input.GetAxisRaw("Vertical")<0){
            downIsDown = true;
        }else{
            downIsDown = false;
        }

        if(Input.GetAxis("crossLeft&Right")<0){
            if(!hitRight){
                showNextQuickItem();
                hitRight = true;
                hitLeft = false;
            }
        }else if(Input.GetAxis("crossLeft&Right")>0){
            if(!hitLeft){
                showLastQuickItem();
                hitLeft = true;
                hitRight = false;
            }
        }else{
            hitRight=false;
            hitLeft = false;
        }

        if(Input.GetAxis("crossUP&DOWN")>0){
            if(!hitDown){
                
                hitDown = true;
                hitUp = false;
            }
        }else if(Input.GetAxis("crossUP&DOWN")<0){
            if(!hitUp){
                useItem();
                hitUp = true;
                hitDown = false;
            }
        }else{
            hitDown=false;
            hitUp = false;
        }
    }

    void FixedUpdate (){
        onGround = Physics2D.OverlapCircle(groundcheck.position, 0.1f, ground);
        onWall = false;//Physics2D.OverlapCircle(wallcheck.position, 0.1f, ground);
        if(onWall&&!onGround){
            walkFreeze = true;
            player.gravityScale = 0;
            player.velocity = new Vector2(player.velocity.x, 0);
            leaveWall = true;
        }else if(leaveWall){
            player.gravityScale = 5;
            leaveWall = false;
        }
            
        if(onGround){
            if(hitGround){
                hitGround = false;
                player.velocity = new Vector2(0, 0);
            }
            AirAttack = true;
            AirTeleport = true;
        }
        if(!onGround){
            hitGround = true;
        }
        Attack();
        shieldOn();
        Move();
        jump();
        fire();
        startTeleport();
    }
    
    void jump(){
        if(!jumpFreeze){
            if(downIsDown&&playerOnPlatform&&jumpPressed){
                StartCoroutine(getDown());
            }else{
                if(onGround){
                    jumpCount = 2;
                    isJump = false;
                }
                if(jumpPressed&&onGround){
                    isJump = true;
                    player.velocity = new Vector2(player.velocity.x, jumpForce);
                    jumpCount--;
                    jumpPressed = false;
                }else if(jumpPressed&&onWall){
                    isJump = true;
                    StartCoroutine(wallJump(player.transform.localScale.x>0));
                    jumpPressed = false;
                }else if(jumpPressed && jumpCount>0&&isJump){
                    player.velocity = new Vector2(player.velocity.x, jumpForce);
                    jumpCount--;
                    jumpPressed = false;
                }
            }
        }
    }
    
    void Move(){
        if(!walkFreeze){
            float horizontalMove = Input.GetAxisRaw("Horizontal");
            if(horizontalMove>0){
                horizontalMove = 1;
            }else if(horizontalMove<0){
                horizontalMove = -1;
            }
            if(horizontalMove !=0){
                needStop = true;
                player.velocity = new Vector2(horizontalMove*speed*Time.deltaTime, player.velocity.y);
            }else{
                if(needStop){
                    needStop = false;
                    player.velocity = new Vector2(0, player.velocity.y);
                }
            }
            

            if(horizontalMove!=0){
                transform.localScale = new Vector3(horizontalMove*System.Math.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);
            }
        }
    }

    void Attack(){
        Vector2 pos;
        if(slashPressed&&!slashFreeze&&(onGround||AirAttack)&&slashObject==null){
            walkFreeze = true;
            jumpFreeze = true;
            shieldFreeze = true;
            teleportFreeze = true;
            if(player.transform.localScale.x<0){
                pos = new Vector2(player.position.x-(float)0.5, player.position.y);
            }else{
                pos = new Vector2(player.position.x+(float)0.5, player.position.y);
            }

            
            
            slashObject = Instantiate(attackPre,pos,transform.rotation);
            StartCoroutine(attackDes());


            if(player.transform.localScale.x<0){
                slashObject.transform.localScale = new Vector2(-slashObject.transform.localScale.x, slashObject.transform.localScale.y);
            }

            if(!onGround){
                AirAttack = false;
            }
        }
    }

    void fire(){
        Vector2 pos;
        if(bulletPressed&&!bulletFreeze&&(onGround||AirAttack)){
            bulletPressed = false;
            walkFreeze = true;
            jumpFreeze = true;
            shieldFreeze = true;
            teleportFreeze = true;
            bulletFreeze = true;
            if(player.transform.localScale.x<0){
                pos = new Vector2(player.position.x-(float)0.5, player.position.y);
            }else{
                pos = new Vector2(player.position.x+(float)0.5, player.position.y);
            }
            GameObject bulletObject = Instantiate(bulletPre,pos,transform.rotation);
            playerBulletBehavior pbb = bulletObject.GetComponent<playerBulletBehavior>();
            pbb.shot(player.transform.localScale.x>0);
            StartCoroutine(bulletDes(bulletObject));


            if(player.transform.localScale.x<0){
                bulletObject.transform.localScale = new Vector2(-bulletObject.transform.localScale.x, bulletObject.transform.localScale.y);
            }

            if(!onGround){
                AirAttack = false;
            }
        }
    }
    private IEnumerator attackDes(){
        player.velocity = new Vector2(0, 0);
        
        player.gravityScale = 0;
        yield return new WaitForSeconds((float)0.3);
        player.gravityScale = STANDARD_GRAVITY;
        Destroy(slashObject);
        slashPressed = false;
        walkFreeze = false;
        jumpFreeze = false;
        shieldFreeze = false;
        teleportFreeze = false;
    }

    private IEnumerator bulletDes(GameObject bulletObject){
        player.velocity = new Vector2(0, 0);
        player.gravityScale = 0;
        yield return new WaitForSeconds((float)0.1);
        player.gravityScale = STANDARD_GRAVITY;
        walkFreeze = false;
        jumpFreeze = false;
        shieldFreeze = false;
        teleportFreeze = false;
        yield return new WaitForSeconds((float)1);
        bulletFreeze = false;

        yield return new WaitForSeconds((float)1);
        Destroy(bulletObject);
    }

    private IEnumerator getDown(){
        Physics2D.IgnoreCollision(playerCollider,platform);
        yield return new WaitForSeconds((float)0.5);
        Physics2D.IgnoreCollision(playerCollider,platform,false);
        jumpPressed = false;
    }

    private IEnumerator wallJump(bool faceRight){

        if(faceRight){
            player.velocity = new Vector2(-10,(float)(jumpForce*0.8));
            transform.localScale = new Vector3(-System.Math.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);
        }else{
            player.velocity = new Vector2(10,(float)(jumpForce*0.8));
            transform.localScale = new Vector3(System.Math.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);
        }
        yield return new WaitForSeconds((float)0.25);
        player.velocity = new Vector2(0, 0);
        walkFreeze = false;
        //yield return new WaitForSeconds((float)0.2);
        jumpPressed = false;
    }

    private IEnumerator parryDes(){
        yield return new WaitForSeconds((float)0.2);
        Destroy(shieldObject);
        teleportFreeze = false;
        slashFreeze = false;
        jumpFreeze = false;
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "platform"){
            playerOnPlatform = true;
            platform = collision.gameObject.GetComponent<Collider2D>();
        }else{
            playerOnPlatform = false;
        }
    }
    
    void shieldOn(){
        Vector2 pos;
       
        if((false||Input.GetAxis("LRT")>0)&&!shieldFreeze&&(onGround||AirAttack)&&shieldObject==null){
            teleportFreeze = true;
            slashFreeze = true;
            jumpFreeze = true;

            if(player.transform.localScale.x<0){
                pos = new Vector2(player.position.x-(float)0.5, player.position.y);
            }else{
                pos = new Vector2(player.position.x+(float)0.5, player.position.y);
            }

            shieldObject = Instantiate(shieldPre,pos,transform.rotation);
            startTime = System.Environment.TickCount;
            
            speed = SLOW_SPEED;
            haveShield = true;

            if(player.transform.localScale.x<0){
                shieldObject.transform.localScale = new Vector2(-shieldObject.transform.localScale.x, shieldObject.transform.localScale.y);
            }

            if(!onGround){
                AirAttack = false;
            }
        }else if((false||Input.GetAxis("LRT")<=0)&&haveShield){
            endTime = System.Environment.TickCount;
            if(endTime-startTime<300){
                parryBehave pb = shieldObject.GetComponent<parryBehave>();
                pb.successParry(shieldObject);
                StartCoroutine(parryDes());
            }else{
                Destroy(shieldObject);
                teleportFreeze = false;
                slashFreeze = false;
                jumpFreeze = false;
            }
            speed = NORMAL_SPEED;
            haveShield = false;
        }
        if(shieldObject != null&&haveShield){
            if(player.transform.localScale.x<0){
                shieldObject.transform.position = new Vector2(player.position.x-(float)0.5, player.position.y);
            }else{
                shieldObject.transform.position = new Vector2(player.position.x+(float)0.5, player.position.y);
            }

            if(player.transform.localScale.x<0){
                shieldObject.transform.localScale = new Vector2(-System.Math.Abs(shieldObject.transform.localScale.x), shieldObject.transform.localScale.y);
            }else{
                shieldObject.transform.localScale = new Vector2(System.Math.Abs(shieldObject.transform.localScale.x), shieldObject.transform.localScale.y);
            }
        }        
    }

    public void hurt(float dmg){
        if(HPImg.fillAmount>0){
            HPImg.fillAmount -= (float)(dmg/10.0);
        }
    }

    void startTeleport(){
        if(teleportPressed&&!teleportFreeze&&(onGround||AirTeleport)){
            jumpFreeze = true;
            walkFreeze = true;
            slashFreeze = true;
            shieldFreeze = true;
            teleportFreeze = true;
            bulletFreeze = true;
            StartCoroutine(teleport(player.transform.transform.localScale.x>0));
            if(!onGround){
                AirTeleport = false;
            }
        }
    }
    private IEnumerator teleport(bool faceRight){
        teleportPressed = false;
        speed = BOOST_SPEED;
        player.gravityScale = 0;
        if(faceRight){
            player.velocity = new Vector2(speed*Time.deltaTime,0);
        }else{
            player.velocity = new Vector2(-speed*Time.deltaTime,0);
        }
        
        yield return new WaitForSeconds((float)0.2);
        player.velocity = new Vector2(0,0);
        yield return new WaitForSeconds((float)0.2);
        jumpFreeze = false;
        walkFreeze = false;
        slashFreeze = false;
        shieldFreeze = false;
        bulletFreeze = false;
        player.gravityScale = STANDARD_GRAVITY;
        speed = NORMAL_SPEED;
        yield return new WaitForSeconds((float)0.5);
        teleportFreeze = false;
    }

    public void stop(){
        player.velocity = new Vector2(0,0);
    }

    public void unFreezeController(){
        jumpFreeze = false;
        walkFreeze = false;
        slashFreeze = false;
        shieldFreeze = false;
        bulletFreeze = false;
        teleportFreeze = false;
    }

    public void freezeController(){
        jumpFreeze = true;
        walkFreeze = true;
        slashFreeze = true;
        shieldFreeze = true;
        bulletFreeze = true;
        teleportFreeze = true;
    }

    void showNextQuickItem(){
        int tmp = quickUseIndex;
        do{
            quickUseIndex = (quickUseIndex+1)%5;
        }while(tmp!=quickUseIndex&&quickUseBar.itemArray[quickUseIndex]==null);
        if(quickUseBar.itemArray[quickUseIndex]==null){
            quickItemText.text = "";
            qiuckItemImg.sprite = Resources.Load("imgRes/empty", typeof(Sprite)) as Sprite;
        }else{
            quickItemText.text = quickUseBar.itemArray[quickUseIndex].itemHeld.ToString();;
            qiuckItemImg.sprite = quickUseBar.itemArray[quickUseIndex].itemImage;
        }
    }

    void showLastQuickItem(){
        int tmp = quickUseIndex;
        do{
            quickUseIndex = (quickUseIndex+4)%5;
        }while(tmp!=quickUseIndex&&quickUseBar.itemArray[quickUseIndex]==null);
        if(quickUseBar.itemArray[quickUseIndex]==null){
            quickItemText.text = "";
            qiuckItemImg.sprite = Resources.Load("imgRes/empty", typeof(Sprite)) as Sprite;
        }else{
            quickItemText.text = quickUseBar.itemArray[quickUseIndex].itemHeld.ToString();;
            qiuckItemImg.sprite = quickUseBar.itemArray[quickUseIndex].itemImage;
        }
    }

    public void refreshQuickBar(){
        if(quickUseBar.itemArray[quickUseIndex]==null){
            quickItemText.text = "";
            qiuckItemImg.sprite = Resources.Load("imgRes/empty", typeof(Sprite)) as Sprite;
        }else{
            quickItemText.text = quickUseBar.itemArray[quickUseIndex].itemHeld.ToString();;
            qiuckItemImg.sprite = quickUseBar.itemArray[quickUseIndex].itemImage;
        }
    }

    public void useItem(){
        if(quickUseBar.itemArray[quickUseIndex].itemHeld>0){
            quickUseBar.itemArray[quickUseIndex].itemHeld--;
        }
        if(quickUseBar.itemArray[quickUseIndex].itemHeld==0){
            quickUseBar.itemArray[quickUseIndex] = null;
            showNextQuickItem();
        }
        refreshQuickBar();
    }
}

