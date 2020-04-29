using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainMenuController : MonoBehaviour
{
    public GameObject selectedImg;
    public Animator anim;
    public GameObject parentGameObject;
    private bool hitUp = false;
    private bool hitDown = false;
    public int selectedIndex = 0;
    void Start(){
        selectedIndex = 0;
        Debug.Log("start");
    }
    void Update(){
        if(Input.GetButtonDown("btnA")){
            pressFunction(selectedIndex);
        }
        if(Input.GetAxis("crossUP&DOWN")<0||Input.GetAxisRaw("Vertical")>0){
            if(!hitDown){
                if(selectedIndex>0){
                    selectedIndex--;
                    moveSelectedImg(selectedIndex);
                }
                Debug.Log(selectedIndex);
                hitDown = true;
                hitUp = false;
            }
        }else if(Input.GetAxis("crossUP&DOWN")>0||Input.GetAxisRaw("Vertical")<0){
            if(!hitUp){
                if(selectedIndex<3){
                    selectedIndex++;
                    moveSelectedImg(selectedIndex);
                }
                Debug.Log(selectedIndex);
                hitUp = true;
                hitDown = false;
            }
        }else{
            hitDown=false;
            hitUp = false;
        }
    }
    public void startNewGame(){
        SceneManager.LoadScene("Scene1");
    }

    public void continueGame(){
        SceneManager.LoadScene("Scene1");
    }

    public void quitGame(){
        Debug.Log("quit");
    }

    public void option(){
        Debug.Log("option");
    }
    private float[] positionY = {23,-22,-68,-115};

    public void moveSelectedImg(int index){
        selectedImg.SetActive(false);
        selectedImg.gameObject.transform.localPosition = new Vector3(0, positionY[index], 0);
        selectedImg.SetActive(true);
    }

    public void pressFunction(int index){
        switch(index){
            case 0:
            startNewGame();
            break;
            case 1:
            continueGame();
            break;
            case 2:
            option();
            break;
            case 3:
            quitGame();
            break;
        }
    }
    
}
