using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public item itemOnGround;
    public inventory myBag;
    public arrayInventory quickBar;

    private playerController playerController;
    
    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="player"){
            playerController = other.gameObject.GetComponent<playerController>();
            AddNewItem();
            Destroy(gameObject);
        }
    }

    private void AddNewItem(){
        if(!myBag.itemList.Contains(itemOnGround)){
            myBag.itemList.Add(itemOnGround);
        }
        bool inQuick = false;
        for(int i =0;i<5;i++){
            if(quickBar.itemArray[i] == itemOnGround){
                inQuick = true;
            }
        }
        if(!inQuick){
            tryGetInQuickBar(itemOnGround);
        }
        itemOnGround.itemHeld+=1;
        playerController.refreshQuickBar();
    }

    public void tryGetInQuickBar(item pickedItem){
        Debug.Log("try getIN");
        for(int i = 0;i<5;i++){
            if(quickBar.itemArray[i]==null){
                quickBar.itemArray[i] = pickedItem;
                break;
            }
        }
    }
}
