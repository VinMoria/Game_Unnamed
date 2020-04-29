using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class speakerController : MonoBehaviour
{
    [Header("UI组件")]
    public Text TextLable;
    bool touchPlayer = false;
    public GameObject textPanel;
    
    public GameObject hint;
    private GameObject player;

    [Header("UI文本")]
    public TextAsset textFile;
    public int index;
    List<string> textList = new List<string>();
    private playerController playerController;
    void Start()
    {
        index = 0;
        processTXT(textFile);
        TextLable.text = textList[0];   
    }

    void Update()
    {
        if(Input.GetButtonDown("btnA")&&touchPlayer){
            if(index == 0){
                textPanel.SetActive(true);
                playerController.jumpFreeze = true;
                playerController.shieldFreeze = true;
                playerController.walkFreeze = true;
                playerController.teleportFreeze = true;
                playerController.slashFreeze = true;
                playerController.bulletFreeze = true;
                playerController.stop();
            }
            if(index<textList.Count){
                TextLable.text = textList[index];
                index++;
            }else if(index==textList.Count){
                textPanel.SetActive(false);
                playerController.shieldFreeze = false;
                playerController.walkFreeze = false;
                playerController.teleportFreeze = false;
                playerController.slashFreeze = false;
                playerController.bulletFreeze = false;
                index++;
            }
        }else if(Input.GetButtonDown("btnA")&&index>textList.Count){
            playerController.jumpFreeze = false;
            index = 0;
        }
    }

     private void OnTriggerEnter2D(Collider2D collision){
         if(collision.gameObject.tag=="player"){
             playerController = collision.gameObject.GetComponent<playerController>();
             player = collision.gameObject;
             touchPlayer = true;
             hint.SetActive(true);
         }
     }

     private void OnTriggerExit2D(Collider2D collision){
         if(collision.gameObject.tag=="player"){
             touchPlayer = false;
             hint.SetActive(false);
         }
     }

     void processTXT(TextAsset textFile){
        textList.Clear();

        var textArray = textFile.text.Split('\n');

        foreach(var i in textArray){
            textList.Add(i);
        }
    }
}
