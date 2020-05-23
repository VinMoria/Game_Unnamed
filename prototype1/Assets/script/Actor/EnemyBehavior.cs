using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject leftPoint,rightPoint,alert;
    private Rigidbody2D rigidbody;
    private int coldDownTime = 0;
    private Transform player;
    private float leftX,rightX;
    private int stateIndex;
    private bool faceRight;
    private Random random = new Random();
    private int randomInt;
    private float attack1Distance = 1.0f;
    private float attack2Distance = 2.0f;
    Vector3 pos;
    EnemySlash es;
    public GameObject slashObject;
    Random rd = new Random();
    private float rdFloat;
    private int HP = 3;
    void Start()
    {
        nextRd();
        rigidbody = GetComponent<Rigidbody2D>();
        leftX = leftPoint.transform.position.x;
        rightX = rightPoint.transform.position.x;
        Destroy(leftPoint);
        Destroy(rightPoint);
        es = slashObject.GetComponent<EnemySlash>();
        stateIndex = 0;
        faceRight = true;
        nextMove();
    }

    void Update()
    {
        
    }

    public void hurt(){
        HP--;
        if(HP<=0){
            Destroy(gameObject);
        }
    }

    void FixedUpdate(){
        coldDown();
        if(stateIndex == 0){
            switch(randomInt){
                case 0:
                moveDirection(true,4);
                break;
                case 1:
                moveDirection(false,-4);
                break;
                case 2:
                break;
            }
        }else if(stateIndex == 1){
            chase();
        }else if(stateIndex == 2){
            if(coldDownTime==0){
                attackStart();
                coldDownTime = -1;
            }else if(coldDownTime>0){
                stateIndex = 1;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag=="player"){
            Debug.Log(stateIndex);
            if(stateIndex == 0){
                stateIndex = 1;
                player = collider.gameObject.transform;
            }
        }
    }

    private void nextMove(){
        randomInt = Random.Range(0, 3);
        Invoke("nextMove", (0.5f*Random.Range(1, 5)));
    }

    private void attackStart(){
        alert.SetActive(true);
        Invoke("Attack", 0.4f);
    }
    
    private void moveDirection(bool faceRight, float speed){
        if(faceRight&&rigidbody.transform.position.x<rightX){
            rigidbody.transform.localScale = new Vector3(System.Math.Abs(rigidbody.transform.localScale.x),rigidbody.transform.localScale.y, rigidbody.transform.localScale.z);
        }else if(!faceRight&&rigidbody.transform.position.x>leftX){
            rigidbody.transform.localScale = new Vector3(-System.Math.Abs(rigidbody.transform.localScale.x),rigidbody.transform.localScale.y, rigidbody.transform.localScale.z);
        }
        if(rigidbody.transform.position.x+speed*Time.deltaTime>leftX&&rigidbody.transform.position.x+speed*Time.deltaTime<rightX){
            rigidbody.transform.position = new Vector3(rigidbody.transform.position.x+speed*Time.deltaTime,rigidbody.transform.position.y,rigidbody.transform.position.z);
        }
    }

    private void chase(){
        if(System.Math.Abs(rigidbody.transform.position.x-player.position.x)>(2.5+rdFloat)){
            if(rigidbody.transform.position.x>player.position.x){
                moveDirection(false, -6);
            }else{
                moveDirection(true, 6);
            }
        }else if(System.Math.Abs(rigidbody.transform.position.x-player.position.x)<(2+rdFloat)
        &&(rigidbody.transform.position.x-leftX)>0.2
        &&(rightX-rigidbody.transform.position.x)>0.2){
            if(rigidbody.transform.position.x>player.position.x){
                moveDirection(false, 3);
            }else{
                moveDirection(true, -3);
            }
        }else if(System.Math.Abs(rigidbody.transform.position.y-player.position.y)<2){
            stateIndex = 2;
        }
    }

    private void Attack(){
        alert.SetActive(false);
        es.slashOn();
        Invoke("AttackEnd", 0.2f);
    }

    private void AttackEnd(){
        es.slashEnd();
        stateIndex = 1;
        coldDownTime = 100+Random.Range(0, 100);
    }

    private void coldDown(){
        if(coldDownTime>0){
            coldDownTime--;
        }
    }

    private void nextRd(){
        rdFloat = Random.Range(0.0f,1.5f);
        Invoke("nextRd",0.7f);
    }
}
