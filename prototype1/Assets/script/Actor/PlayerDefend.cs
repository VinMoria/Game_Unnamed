using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefend : MonoBehaviour
{
    GameObject parry,shield;
    void Start()
    {
        Debug.Log("defend onStart");
        shield = GetComponentsInChildren<Transform>()[1].gameObject;
        parry = GetComponentsInChildren<Transform>()[2].gameObject;
        shield.SetActive(false);
        parry.SetActive(false);
    }

    void Update()
    {
        
    }

    public void shieldOn(){
        shield.SetActive(true);
    }

    public void shieldDown(){
        shield.SetActive(false);
    }

    public void parryActive(){
        shield.SetActive(false);
        parry.SetActive(true);
        Invoke("parryDown",0.1f);
    }

    private void parryDown(){
        parry.SetActive(false);
    }
}
