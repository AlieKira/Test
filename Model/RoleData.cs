using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;
using UnityEngine.UI;


public class RoleData : MonoBehaviour
{
    public int userID;
    public bool isLocal = false;
    private RoleType roleType;
    private int maxHP = 0;
    private int currentHP;
    private int lv = 0;
    private bool isTakeDamage = false;
    private bool isDead = false;
    private string buffer;
    public Slider slider;


    public int UserID
    {
        get { return userID; }
    }

    public void SetRoleData(int userID, RoleType roleType)
    {
        this.userID = userID;
        this.roleType = roleType;
    }



    public void SetAttri( int hp)
    {
        this.maxHP = hp;
        currentHP = maxHP;

    }

    public void SetLevel(int lv)
    {
        this.lv = lv;
    }

    public int Lv
    {
        get { return lv; }
    }

    private void Update()
    {
        if (isTakeDamage == true)
        {
            //GetComponent<PlayerControl>().TakeDamage();
            currentHP = int.Parse(buffer);
            slider.value = currentHP / (float)maxHP;
            isTakeDamage = false;
        }
        if (isDead == true)
        {
            this.gameObject.GetComponent<Animator>().SetTrigger("Die");
            slider.value = 0;
            this.gameObject.tag = "Finish";
            isDead = false;
            
        }
    }


    public void OnTakeDamagerResponse(string data)
    {
        Debug.Log(userID+"被攻击");
        isTakeDamage = true;
        buffer = data;
        
    }

    public void OnDeadResponse()
    {
        isDead = true;
    }


    
}
