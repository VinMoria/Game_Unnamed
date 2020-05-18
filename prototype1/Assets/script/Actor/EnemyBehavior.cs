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
    Vector2 pos;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        leftX = leftPoint.transform.position.x;
        rightX = rightPoint.transform.position.x;
        Destroy(leftPoint);
        Destroy(rightPoint);
        stateIndex = 0;
        faceRight = true;
        nextMove();
    }

    void Update()
    {
        
    }

    void FixedUpdate(){
        coldDown();
        if(stateIndex == 0){
            switch(randomInt){
                case 0:
                moveDirection(true,4);
                break;
                case 1:
                moveDirection(false,4);
                break;
                case 2:
                break;
            }
        }else if(stateIndex == 1){
            chase();
        }else if(stateIndex == 2){
            if(coldDownTime==0){
                coldDownTime = 200;
                attackStart();
            }else{
                stateIndex = 1;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag=="player"){
            stateIndex = 1;
            player = collider.gameObject.transform;
        }
    }

    private void nextMove(){
        randomInt = Random.Range(0, 3);
        Invoke("nextMove", (0.5f*Random.Range(1, 5)));
    }

    private void attackStart(){
        stateIndex = 1;
        alert.SetActive(true);
        Invoke("Attack", 0.4f);
    }
    
    private void moveDirection(bool faceRight, float speed){
        if(faceRight&&rigidbody.transform.position.x<rightX){
            rigidbody.transform.localScale = new Vector3(System.Math.Abs(rigidbody.transform.localScale.x),rigidbody.transform.localScale.y, rigidbody.transform.localScale.z);
            rigidbody.transform.position = new Vector3(rigidbody.transform.position.x+speed*Time.deltaTime,rigidbody.transform.position.y,rigidbody.transform.position.z);
        }else if(!faceRight&&rigidbody.transform.position.x>leftX){
            rigidbody.transform.localScale = new Vector3(-System.Math.Abs(rigidbody.transform.localScale.x),rigidbody.transform.localScale.y, rigidbody.transform.localScale.z);
            rigidbody.transform.position = new Vector3(rigidbody.transform.position.x-speed*Time.deltaTime,rigidbody.transform.position.y,rigidbody.transform.position.z);
        }
    }

    private void chase(){
        if(System.Math.Abs(rigidbody.transform.position.x-player.position.x)>4){
            if(rigidbody.transform.position.x>player.position.x){
                moveDirection(false, 6);
            }else{
                moveDirection(true, 6);
            }
        }else{
            stateIndex = 2;
        }
    }

    private void Attack(){
        alert.SetActive(false);
        if(rigidbody.transform.localScale.x>0){
            pos = new Vector2(rigidbody.position.x+1.0f, rigidbody.position.y);
        }else{
            pos = new Vector2(rigidbody.position.x-1.0f, rigidbody.position.y);
        }
        GameObject slashObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/ActorPrefabs/enemySlash"));
        slashObject.transform.position = pos;
        slashObject.transform.name = "slash";
        EnemySlash es = slashObject.GetComponent<EnemySlash>();

        if(rigidbody.transform.localScale.x<0){
            slashObject.transform.localScale = new Vector2(-slashObject.transform.localScale.x, slashObject.transform.localScale.y);
        }
    }

    private void coldDown(){
        if(coldDownTime>0){
            coldDownTime--;
        }
    }
}
