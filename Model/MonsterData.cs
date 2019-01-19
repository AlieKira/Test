using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;
using UnityEngine.UI;


public  class MonsterData:MonoBehaviour
{
    public Slider slider;
    public int number;
    private MonsterType monsterType = MonsterType.none;
    private string prefabPath = null;
    private int maxHP = 0;
    private int attack = 0;
    private bool isTakeDamage = false;
    private bool isDead = false;
    private string buffer;

    public int currentHP = -1;

    

    public int Number
    {
        get { return number; }
    }

    public int MaxHP
    {
        get { return maxHP; }
    }

    public int Attack
    {
        get { return attack; }
    }

    public MonsterType MonsterType
    {
        get { return monsterType; }
    }

    public string PrefabPath
    {
        get { return prefabPath; }
    }



    private void Update()
    {
        if (isTakeDamage==true)
        {
            currentHP = int.Parse(buffer);
            slider.value = currentHP / (float)maxHP;
            isTakeDamage = false;
        }
        if (isDead == true)
        {
            this.gameObject.GetComponent<Animator>().SetTrigger("Die");
            slider.value = 0;
            Invoke("Hide",1.5f);
        }
    }

    public MonsterData(MonsterType type, string prefabPath)
    {
        this.monsterType = type;
        this.prefabPath = prefabPath;

    }

    public void SetAttri(int num,int hp,int attack)
    {
        this.number = num;
        this.maxHP = hp;
        currentHP = maxHP;
        this.attack = attack;

    }

    public void OnTakeDamagerResponse(string data)
    {
        isTakeDamage = true;
        buffer = data;
    }

    public void OnDeadResponse()
    {
        Debug.Log(1);
        isDead = true;
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}


