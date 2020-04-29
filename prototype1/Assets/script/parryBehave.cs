using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parryBehave : MonoBehaviour
{
    public GameObject parry;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void successParry(GameObject shield){
        parry.SetActive(true);
    }
}
