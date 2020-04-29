using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuController : MonoBehaviour
{
    public GameObject player;

    public GameObject inventoryMenuPanel;
    public GameObject equipmentMenuPanel;
    public GameObject journalMenuPanel;
    private bool menuOpen = false;

    private bool menuPressed = false;

    private int menuIndex = 0;

    playerController pc;

    equipmentMenuController emc;

    
    void Start()
    {
        pc = player.GetComponent<playerController>();
        emc = equipmentMenuPanel.GetComponent<equipmentMenuController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("menu")){
            menuPressed = true;
        }

        if(Input.GetButtonDown("LB")&&menuOpen){
            menuIndex = (menuIndex+2)%3;
            showMenu(menuIndex);
            Debug.Log(menuIndex);
        }

        if(Input.GetButtonDown("RB")&&menuOpen){
            menuIndex = (menuIndex+1)%3;
            showMenu(menuIndex);
            Debug.Log(menuIndex);
        }
    }

    void FixedUpdate(){
        if(menuPressed){
            menuPressed = false;
            if(menuOpen){
                closeMenu();
                menuOpen = !menuOpen;
                pc.unFreezeController();
            }else{
                showMenu(menuIndex);
                menuOpen = !menuOpen;
                pc.freezeController();
            }
        }
    }

    void showMenu(int index){
        switch(index){
            case 0:
            equipmentMenuPanel.SetActive(true);
            emc.refreshQuickBar();
            inventoryMenuPanel.SetActive(false);
            journalMenuPanel.SetActive(false);
            break;
            case 1:
            inventoryMenuPanel.SetActive(true);
            equipmentMenuPanel.SetActive(false);
            journalMenuPanel.SetActive(false);
            break;
            case 2:
            journalMenuPanel.SetActive(true);
            equipmentMenuPanel.SetActive(false);
            inventoryMenuPanel.SetActive(false);
            break;
        }
    }

    void closeMenu(){
        journalMenuPanel.SetActive(false);
        equipmentMenuPanel.SetActive(false);
        inventoryMenuPanel.SetActive(false);
    }
}
