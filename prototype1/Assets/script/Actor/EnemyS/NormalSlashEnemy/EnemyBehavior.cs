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
    private int randomInt;
    EnemySlash es;
    public GameObject slashObject;
    private float rdFloat;
    public EnemyState enemyState;
    void Start()
    {
        enemyState = new EnemyState();
        nextRd();
        rigidbody = GetComponent<Rigidbody2D>();
        leftX = leftPoint.transform.position.x;
        rightX = rightPoint.transform.position.x;
        Destroy(leftPoint);
        Destroy(rightPoint);
        es = slashObject.GetComponent<EnemySlash>();
        nextMove();
        enemyState.defList.Add(new DmgNDefItem("phy", 0.9f));
        enemyState.defList.Add(new DmgNDefItem("fire", 0.9f));
        enemyState.dmgList.Add(new DmgNDefItem("phy", 20));
        enemyState.dmgList.Add(new DmgNDefItem("fire", 20));
    }

    void Update()
    {
        
    }

    public void hurt(GameObject dmgFrom, List<DmgNDefItem> dmgList){
        if(enemyState.stateIndex == 0)
        {
            player = dmgFrom.transform;
            enemyState.stateIndex = 1;
        }
        float finalDmg = 0;
        foreach (DmgNDefItem dmg in dmgList)
        {
            float thisDmg = dmg.num;
            foreach (DmgNDefItem def in enemyState.defList) 
            {
                if(def.name == dmg.name)
                {
                    thisDmg = thisDmg * def.num;
                    break;
                }
            }

            finalDmg += thisDmg;
        }
        enemyState.HP -= finalDmg;
        Debug.Log("hurt: "+finalDmg);
        
        if(enemyState.HP<=0){
            //Destroy(gameObject);
        }
    }

    void FixedUpdate(){
        coldDown();
        if(enemyState.stateIndex == 0){
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
        }else if(enemyState.stateIndex == 1){
            chase();
        }else if(enemyState.stateIndex == 2){
            if(coldDownTime==0){
                attackStart();
                coldDownTime = -1;
            }else if(coldDownTime>0){
                enemyState.stateIndex = 1;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag=="player"){
            Debug.Log(enemyState.stateIndex);
            if(enemyState.stateIndex == 0){
                enemyState.stateIndex = 1;
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
        Invoke("Attack", 0.5f);
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
            enemyState.stateIndex = 2;
        }
    }

    private void Attack(){
        alert.SetActive(false);
        es.slashOn();
        Invoke("AttackEnd", 0.2f);
    }

    private void AttackEnd(){
        es.slashEnd();
        enemyState.stateIndex = 1;
        coldDownTime = 50+Random.Range(0, 30);
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

    public void setPlayer(Transform playerTransform)
    {
        this.player = playerTransform;
    }
}
