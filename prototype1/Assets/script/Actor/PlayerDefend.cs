using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefend : MonoBehaviour
{
    GameObject parry,shield;
    void Start()
    {
        shield = GetComponentsInChildren<Transform>()[1].gameObject;
        parry = GetComponentsInChildren<Transform>()[2].gameObject;
        shield.SetActive(false);
        parry.SetActive(false);
    }

    void Update()
    {
        
    }

    public void shieldOn(){
        PlayerState.Instance.shield = true;
        shield.SetActive(true);
    }

    public void shieldDown(){
        PlayerState.Instance.shield = false;
        shield.SetActive(false);
    }

    public void parryActive(){
        shield.SetActive(false);
        parry.SetActive(true);
        PlayerState.Instance.parry = true;
        PlayerState.Instance.shield = false;
        Invoke("parryDown",0.3f);
    }

    private void parryDown(){
        parry.SetActive(false);
        PlayerState.Instance.parry = false;
    }
}
