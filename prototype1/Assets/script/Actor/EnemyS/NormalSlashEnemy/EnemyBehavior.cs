using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject leftPoint,rightPoint,alert;
    private new Rigidbody2D rigidbody;
    private Transform player;
    private float leftX,rightX;
    private int randomInt;
    EnemySlash es;
    public GameObject slashObject;
    public GameObject dizzyObject;
    private float rdFloat;
    public EnemyState enemyState;
    void Start()
    {
        enemyState = new EnemyState();
        enemyState.stateStr = "wander";
        enemyState.coldDownTime[0] = 0;
        enemyState.coldDownTime[1] = 0;
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

    public void LinkPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    public void hurt(List<DmgNDefItem> dmgList, string dmgType){
        if(enemyState.stateStr == "wander")
        {
            enemyState.stateStr = "chase";
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
            Destroy(gameObject);
        }

        if(dmgType == "bullet")
        {
            if (enemyState.stateStr == "attackPre")
            {
                alert.SetActive(false);
                enemyState.coldDownTime[0] = 0;
                startDizzy();
            }
        }
    }

    void FixedUpdate(){
        coldDown();
        if(enemyState.stateStr == "wander"){
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
        }else if(enemyState.stateStr == "chase"){
            chase();
        }else if(enemyState.stateStr == "attackPre"){
            if(enemyState.coldDownTime[0]==0){
                attackStart();
                enemyState.coldDownTime[0] = -1;
            }else if(enemyState.coldDownTime[0] > 0){
                enemyState.stateStr = "chase";
            }
        }else if(enemyState.stateStr == "dizzy")
        {

        }
        
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag=="player"){
            if(enemyState.stateStr == "wander"){
                enemyState.stateStr = "chase";
            }
        }
    }

    public void startDizzy()
    {
        dizzyObject.SetActive(true);
        enemyState.stateStr = "dizzy";
        Invoke("endDizzy",2.0f);
    }

    public void endDizzy()
    {
        dizzyObject.SetActive(false);
        enemyState.stateStr = "chase";
    }

    private void nextMove(){
        randomInt = Random.Range(0, 3);
        Invoke("nextMove", (0.5f*Random.Range(1, 5)));
    }

    private void attackStart(){
        if(rigidbody.transform.position.x < player.position.x)
        {
            rigidbody.transform.localScale = new Vector3(System.Math.Abs(rigidbody.transform.localScale.x), rigidbody.transform.localScale.y, rigidbody.transform.localScale.z);
        }
        else
        {
            rigidbody.transform.localScale = new Vector3(-System.Math.Abs(rigidbody.transform.localScale.x), rigidbody.transform.localScale.y, rigidbody.transform.localScale.z);
        }
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
        if(System.Math.Abs(rigidbody.transform.position.x - player.position.x) > (15 + rdFloat))
        {
            enemyState.stateStr = "wander";
        }else if(System.Math.Abs(rigidbody.transform.position.x-player.position.x)>(2.5+rdFloat)){
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
            enemyState.stateStr = "attackPre";
        }
    }

    private void Attack(){
        if(enemyState.stateStr == "attackPre")
        {
            enemyState.stateStr = "attack";
            alert.SetActive(false);
            es.slashOn();
            Invoke("AttackEnd", 0.2f);
        }
    }

    private void AttackEnd(){
        es.slashEnd();
        if (enemyState.stateStr == "attack")
        {
            enemyState.stateStr = "chase";
        }
        enemyState.coldDownTime[0] = 50+Random.Range(0, 30);
    }

    private void coldDown(){
        for(int i = 0; i < 1; i++)
        {
            if (enemyState.coldDownTime[i] > 0)
            {
                enemyState.coldDownTime[i]--;
            }
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
