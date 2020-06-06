using NPOI.SS.Formula.Functions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : Singleton<PlayerState>
{
    public int activedActionTimeKeeper = 0;
    public string activedActionTimeKeeperName = "";
    public bool playerActionsFreezed = false;
    public float HP;
    public float MaxHP = 100;
    public float MP;
    public float MaxMP = 50;
    public bool defendOn = false;
    public bool dead = false;
    public List<DmgNDefItem> slashDmgList;
    public List<DmgNDefItem> bulletDmgList;
    public List<DmgNDefItem> parryDmgList;
    public List<DmgNDefItem> defList;
    public List<DmgNDefItem> shieldDefList;
    public Vector3 stingDmgCheckPoint;
    private Image HPImg, MPImg;
    public float shotMP = 20.0f;
    public float MoveSpeed = 600.0f;
    public float JumpForce = 20.0f;
    public float GravityScale = 6.0f;
    public float DashSpeed = 1700.0f;
    public int dashCD = 30;
    public int bulletCD = 50;
    public int slashCD = 20;
    public int defendCD = 40;
    public float shieldMP = 17.0f;

    public void Init()
    {
        activedActionTimeKeeper = 0;
        activedActionTimeKeeperName = "";
        playerActionsFreezed = false;
        HP = MaxHP;
        MP = MaxMP;
        defendOn = false;
        dead = false;
        slashDmgList = new List<DmgNDefItem>();
        bulletDmgList = new List<DmgNDefItem>();
        parryDmgList = new List<DmgNDefItem>();
        defList = new List<DmgNDefItem>();
        shieldDefList = new List<DmgNDefItem>();
    slashDmgList.Add(new DmgNDefItem("phy", 20));
        slashDmgList.Add(new DmgNDefItem("fire", 20));
        slashDmgList.Add(new DmgNDefItem("ice", 5));
        bulletDmgList.Add(new DmgNDefItem("phy", 10));
        parryDmgList.Add(new DmgNDefItem("phy", 30));
        parryDmgList.Add(new DmgNDefItem("ice", 10));
        defList.Add(new DmgNDefItem("phy", 0.8f));
        defList.Add(new DmgNDefItem("fire",0.5f));
        HPImg = GameObject.Find("HP").GetComponent<Image>();
        MPImg = GameObject.Find("MP").GetComponent<Image>();
    }

    public void hurt(List<DmgNDefItem> dmgList)
    {
        float finalDmg = 0;
        foreach(DmgNDefItem dmg in dmgList)
        {
            float thisDmg = dmg.num;
            foreach(DmgNDefItem def in defList)
            {
                if(def.name == dmg.name)
                {
                    thisDmg = thisDmg * def.num;
                    break;
                }
            }

            finalDmg += thisDmg;
        }

        if (HP - finalDmg < 0)
        {
            HP = 0;
        }
        else
        {
            HP -= finalDmg;
        }
        HPImg.fillAmount = HP / MaxHP;
        Debug.Log("player get hurt: " + finalDmg);
        if (HP == 0)
        {
            dead = true;
            playerActionsFreezed = true;
        }
    }

    public void useMP(float useMPNum)
    {
        MP -= useMPNum;
        if (MP > MaxMP)
        {
            MP = MaxMP;
        }
        if (MP < 0)
        {
            MP = 0;
        }
        MPImg.fillAmount = MP / MaxMP;
    }

}
