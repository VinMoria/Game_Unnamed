using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class equipmentMenuController : MonoBehaviour
{
    public GameObject grid;
    public arrayInventory quickMenuBar;
    void Start()
    {
        refreshQuickBar();
    }

    void Update()
    {
        
    }

    public void refreshQuickBar(){
        Text text;
        Image image;
        for(int i = 0;i<5;i++){
                text = GameObject.Find("Canvas/equipmentMenu/grid/slot"+i.ToString()+"/num").GetComponent<Text>();
                image = GameObject.Find("Canvas/equipmentMenu/grid/slot"+i.ToString()).GetComponent<Image>();
            if(quickMenuBar.itemArray[i]!=null){
                text.text =quickMenuBar.itemArray[i].itemHeld.ToString();
                image.sprite = quickMenuBar.itemArray[i].itemImage;
            }else{
                text.text = "";
                image.sprite = Resources.Load("imgRes/empty", typeof(Sprite)) as Sprite;
            }
            
        }
    }
}
