using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerState : Singleton<PlayerState>
{
    public int activedActionTimeKeeper = 0;
    public string activedActionTimeKeeperName = "";
    public bool playerActionsFreezed = false;
    public float HP = 100;
    public bool defendOn = false;
    public bool dead = false;
    public List<DmgNDefItem> slashDmgList = new List<DmgNDefItem>();
    public List<DmgNDefItem> bulletDmgList = new List<DmgNDefItem>();
    public List<DmgNDefItem> parryDmgList = new List<DmgNDefItem>();
    public List<DmgNDefItem> defList = new List<DmgNDefItem>();
    public List<DmgNDefItem> shieldDefList = new List<DmgNDefItem>();

    public void Init()
    {
        slashDmgList.Add(new DmgNDefItem("phy", 20));
        slashDmgList.Add(new DmgNDefItem("fire", 20));
        slashDmgList.Add(new DmgNDefItem("ice", 5));
        bulletDmgList.Add(new DmgNDefItem("phy", 10));
        parryDmgList.Add(new DmgNDefItem("phy", 30));
        parryDmgList.Add(new DmgNDefItem("ice", 10));
        defList.Add(new DmgNDefItem("phy", 0.2f));
        defList.Add(new DmgNDefItem("fire",0.5f));
        shieldDefList.Add(new DmgNDefItem("phy", 0.8f));
        shieldDefList.Add(new DmgNDefItem("ice", 0.5f));
    }

    public void hurt(List<DmgNDefItem> dmgList, bool haveShield)
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

            if (haveShield)
            {
                foreach (DmgNDefItem def in shieldDefList)
                {
                    if (def.name == dmg.name)
                    {
                        thisDmg = thisDmg * def.num;
                        break;
                    }
                }
            }

            finalDmg += thisDmg;
        }

        HP -= finalDmg;
        Debug.Log("player get hurt: " + finalDmg);
        if (HP <= 0)
        {
            dead = true;
            playerActionsFreezed = true;
        }
    }
}
