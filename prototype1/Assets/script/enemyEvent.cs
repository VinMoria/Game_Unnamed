using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyEvent : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    public Transform leftpoint,rightpoint;
    public float speed;
    public float leftx, rightx;
    public GameObject attackPre;

    private GameObject Sword_Attack;
    private bool Faceleft = true;
    private bool wait = false;

    private GameObject player;

    private bool Alert = false;

    public int HP;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        leftx = leftpoint.position.x;
        rightx = rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void FixedUpdate(){
        if(Alert){
            chase();
        }else{
            Movement();
        }
    }

    void Movement(){
        if(!wait){
            if(Faceleft){
                rb.velocity = new Vector2(-speed*Time.deltaTime, rb.velocity.y);
                if(transform.position.x<leftx){
                    wait = true;
                    StartCoroutine(turnRight());
                }
            }else{
                rb.velocity = new Vector2(speed*Time.deltaTime, rb.velocity.y);
                if(transform.position.x>rightx){
                    wait = true;
                    StartCoroutine(turnLeft());
                }
            }
        }
    }

    void chase(){

        if(transform.position.x>player.transform.position.x){
            transform.localScale = new Vector3(System.Math.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);
        }else{
            transform.localScale = new Vector3(-1*System.Math.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);
        }

        transform.localScale = new Vector3(transform.localScale.x,(float)0.06,transform.localScale.z);
        if(System.Math.Abs(transform.position.x-player.transform.position.x)<1){
            if(Sword_Attack==null){
                attack();
            }
        }else if(System.Math.Abs(transform.position.x-player.transform.position.x)>7){
            transform.localScale = new Vector3(transform.localScale.x,(float)0.13,transform.localScale.z);
            Alert = false;
        }
        else{
            if(transform.position.x>player.transform.position.x){
                rb.velocity = new Vector2(-speed*Time.deltaTime, rb.velocity.y);
            }else{
                rb.velocity = new Vector2(speed*Time.deltaTime, rb.velocity.y);
            }
        }
    }

    private IEnumerator turnRight(){
        rb.velocity = new Vector2(0, rb.velocity.y);
        yield return new WaitForSeconds((float)3);
        transform.localScale = new Vector3(-1*System.Math.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);
        Faceleft = false;
        wait = false;
    }

    private IEnumerator turnLeft(){
        rb.velocity = new Vector2(0, rb.velocity.y);
        yield return new WaitForSeconds((float)3);
        transform.localScale = new Vector3(System.Math.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);
        Faceleft = true;
        wait = false;
    }
    
    public void hurt(int dmg){
        rb.velocity = new Vector2(10, rb.velocity.y);
        HP-=dmg;
        if(HP<=0){
            Destroy(gameObject);
            Destroy(Sword_Attack);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "player"){
            Alert = true;
            player = collision.gameObject;
        }
    }

    void attack(){
        Vector2 pos;
        if(rb.transform.localScale.x>0){
                pos = new Vector2(rb.position.x-(float)0.5, rb.position.y);
            }else{
                pos = new Vector2(rb.position.x+(float)0.5, rb.position.y);
            }

            
            
            Sword_Attack = Instantiate(attackPre,pos,transform.rotation);

            if(rb.transform.localScale.x>0){
                Sword_Attack.transform.localScale = new Vector2(-Sword_Attack.transform.localScale.x, Sword_Attack.transform.localScale.y);
            }
            
            StartCoroutine(attackDes());
    }

    private IEnumerator attackDes(){
        rb.velocity = new Vector2(0, 0);
        //float tmp = rb.gravityScale;
        //rb.gravityScale = 0;
        yield return new WaitForSeconds((float)0.3);
        //rb.gravityScale = tmp;
        Sword_Attack.SetActive(false);
        yield return new WaitForSeconds((float)0.2);
        Destroy(Sword_Attack);
    }
}

