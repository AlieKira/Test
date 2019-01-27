using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class RoleData : MonoBehaviour
{
    public bool isLocal = false;
    public RoleType roleType;
    private int attackDis;
    public string attackPrefab;
    public int currentHP;
    private int lv = 1;
    public int exp = 0;
    private bool isTakeDamage = false;
    private bool isDead = false;
    public Slider slider;

    #region basic property

    private int basicHP;
    private int basicStrength = 10;
    private int basicIntellect = 10;
    private int basicAgility = 10;
    private int basicStamina = 10;
    private int basicAttack = 10;

    public int BasicStrength
    {
        get
        {
            return basicStrength;
        }
    }
    public int BasicIntellect
    {
        get
        {
            return basicIntellect;
        }
    }
    public int BasicAgility
    {
        get
        {
            return basicAgility;
        }
    }
    public int BasicStamina
    {
        get
        {
            return basicStamina;
        }
    }
    public int BasicAttack
    {
        get
        {
            return basicAttack;
        }
    }
    #endregion

    #region current property

    public int strength ;
    public int intellect ;
    public int agility ;
    public int stamina ;
    public int attack ;
    public int MaxHP
    {
        get { return stamina * 3 + basicHP; }
    }
    public int Damage
    {
        get { return attack + (strength + intellect + agility) / 2; }
    }

    #endregion

    /// <summary>
    /// 初始化角色属性
    /// </summary>
    /// <param name="roleTypeInfo"></param>
    public void SetAttri( RoleTypeInfo roleTypeInfo)
    {
        this.roleType = roleTypeInfo.roleType;
        this.attackDis = roleTypeInfo.attackDis;
        this.attackPrefab = roleTypeInfo.attackPrefab;
        this.basicHP = roleTypeInfo.maxHP;
        currentHP = basicHP;
        strength= this.basicStrength = roleTypeInfo.strength;
        intellect=this.basicIntellect = roleTypeInfo.intellect;
        agility=this.basicAgility = roleTypeInfo.agility;
        stamina=this.basicStamina = roleTypeInfo.stamina;
        attack=this.basicAttack = roleTypeInfo.attack;
    }

    public void UpdateAttri(int strength, int intellect, int agility, int stamina, int attack)
    {
        this.strength = basicStrength+strength;
        this.intellect = basicIntellect + intellect;
        this.agility = basicAgility + agility;
        this.stamina = basicStamina + stamina;
        this.attack = basicAttack + attack;
    }

    public int Lv
    {
        get { return lv; }
    }

    private void Update()
    {
        if (isTakeDamage == true)
        {
            slider.value = currentHP / (float)MaxHP;
            isTakeDamage = false;
        }
        if (isDead == true)
        {
            this.gameObject.GetComponent<Animator>().SetTrigger("Die");
            slider.value = 0;
            this.gameObject.tag = "Finish";
            GameManager.Instance.GameOver();
            isDead = false;
        }
    }

    public void reduceHP(int hp)
    {
        GameManager.Instance.UpdateRoleInterface();
        currentHP -= hp;
        if (currentHP<=0)
        {
            currentHP = 0;
            isDead = true;
        }
        else
        {
            isTakeDamage = true;
        }
    }


    public void AddHp(int hp)
    {
        this.currentHP += hp;
        if (currentHP> MaxHP)
        {
            currentHP = MaxHP;
        }
        GameManager.Instance.UpdateRoleInterface();
    }

    public void AddExp(int exp)
    {
        this.exp += exp;
        if (this.exp>=lv*100)
        {
            LevelUp();
            this.exp = 0;
        }
        GameManager.Instance.UpdateRoleInterface();
    }

    public void LevelUp()
    {
        lv++;
        switch (roleType)
        {
            case RoleType.Archer:
                basicAgility += 3;
                basicAttack += 5;
                basicHP += 20;
                break;
            case RoleType.Barbarian:
                basicStrength += 3;
                basicAttack += 3;
                basicStamina += 2;
                basicHP += 50;
                break;
            case RoleType.Casual:
                basicStrength += 1;
                basicStamina += 1;
                basicAgility += 1;
                basicAttack += 3;
                basicHP += 20;
                break;
            case RoleType.Knight:
                basicStrength += 3;
                basicStamina += 2;
                basicAttack += 4;
                basicHP += 30;
                break;
            case RoleType.Mage:
                basicIntellect += 5;
                basicAttack += 10;
                basicHP += 20;
                break;
        }
    }
}
