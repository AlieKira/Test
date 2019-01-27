using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 挂载在怪物上，作为单个怪物的数据
/// </summary>
public  class MonsterData:MonoBehaviour
{
    public Slider slider;

    public int currentHP = -1;

    public MonsterType monsterType;
    public int maxHP;
    public int attack;
    public int attackDis;
    public int skillDis;
    public string attackEffect;
    public string skillEffect;
    public int rangeMin;
    public int rangeMax;

    private bool isTakeDamage = false;
    private bool isDead = false;

    private void Start()
    {
        Canvas canvas = transform.GetComponentInChildren<Canvas>();
        slider = canvas.transform.Find("Slider").GetComponent<Slider>();
    }

    private void Update()
    {
        if (isTakeDamage == true)
        {
            GameManager.Instance.PlayNormalSound(Audios.Sound_ShootPerson);
            slider.value = currentHP / (float)maxHP;
            isTakeDamage = false;
        }
        if (isDead == true)
        {
            this.gameObject.GetComponent<Animator>().SetTrigger("Die");
            slider.value = 0;
            this.gameObject.tag = "Finish";
            GameManager.Instance.MonsterDead(this.gameObject);
            isDead = false;
        }
    }

    public void SetAttri(MonsterTypeInfo monsterTypeInfo,int degree)
    {
        monsterType = monsterTypeInfo.monsterType;
        maxHP = monsterTypeInfo.maxHP*degree;
        currentHP = maxHP;
        attack = monsterTypeInfo.attack* degree;
        attackDis = monsterTypeInfo.attackDis;
        skillDis = monsterTypeInfo.skillDis;
        attackEffect = monsterTypeInfo.attackEffect;
        skillEffect = monsterTypeInfo.skillEffect;
        rangeMin = monsterTypeInfo.rangeMin;
        rangeMax = monsterTypeInfo.rangeMax;
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void reduceHP(int hp)
    {
        currentHP -= hp;
        if (currentHP <= 0)
        {
            currentHP = 0;
            isDead = true;
        }
        else
        {
            isTakeDamage = true;
        }
    }

}


