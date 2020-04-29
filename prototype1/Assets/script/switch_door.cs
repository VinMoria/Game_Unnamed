using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switch_door : MonoBehaviour
{
    public GameObject door;
    public GameObject hint;
    // Start is called before the first frame update
    
    void Start()
    {
        hint.SetActive(false);
    }

    public void showHint(){
        hint.SetActive(true);
    }

    public void hideHint(){
        hint.SetActive(false);
    }

    public void openDoor(){
        Destroy(door);
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "player"){
            if(door!=null){
                showHint();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.tag == "player"){
            hideHint();
        }
    }
}
