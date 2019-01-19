using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;


public class RoleTypeData
{
    public RoleTypeData(string userData)
    {
        string[] strs = userData.Split(',');
        this.roleType = (RoleType) Enum.Parse(typeof(RoleType),strs[0]);
        this.prefabPath = strs[1];
        this.imagePath = strs[2];
        this.skill_01ImagePath = strs[3];
        this.skill_02ImagePath = strs[4];
        this.skill_03ImagePath = strs[5];
        this.attack = int.Parse(strs[6]);
        this.defence = int.Parse(strs[7]);
        this.maxHP = int.Parse(strs[8]);
        this.maxMP = int.Parse(strs[9]);
        this.skill_01Message = strs[10];
        this.skill_02Message = strs[11];
        this.skill_03Message = strs[12];
    }

    public RoleTypeData(string roleTypeString,string prefabPath,string imagePath,string skill_01ImagePath, string skill_02ImagePath, string skill_03ImagePath, 
        string attack, string defence,string maxHP, string maxMP,string skill_01Message, string skill_02Message, string skill_03Message)
    {
        this.roleType = (RoleType)Enum.Parse(typeof(RoleType), roleTypeString);
        this.prefabPath = prefabPath;
        this.imagePath = imagePath;
        this.skill_01ImagePath = skill_01ImagePath;
        this.skill_02ImagePath = skill_02ImagePath;
        this.skill_03ImagePath = skill_03ImagePath;
        this.attack = int.Parse(attack);
        this.defence = int.Parse(defence);
        this.maxHP = int.Parse(maxHP);
        this.maxMP = int.Parse(maxMP);
        this.skill_01Message = skill_01Message;
        this.skill_02Message = skill_02Message;
        this.skill_03Message = skill_03Message;
    }


    private RoleType roleType;
    private string prefabPath;
    private string imagePath;
    private string skill_01ImagePath;
    private string skill_02ImagePath;
    private string skill_03ImagePath;
    private int attack;
    private int defence;
    private int maxHP;
    private int maxMP;
    private string skill_01Message;
    private string skill_02Message;
    private string skill_03Message;


    public RoleType RoleType
    {
        get { return roleType; }
    }
    public string PrefabPath
    {
        get { return prefabPath; }
    }

    public string ImagePath
    {
        get { return imagePath; }
    }

    public string Skill_01ImagePath
    {
        get { return skill_01ImagePath; }
    }

    public string Skill_02ImagePath
    {
        get { return skill_02ImagePath; }
    }

    public string Skill_03ImagePath
    {
        get { return skill_03ImagePath; }
    }

    public int Attack
    {
        get { return attack; }
    }

    public int Defence
    {
        get { return defence; }
    }

    public int MaxHP
    {
        get { return maxHP; }
    }

    public int MaxMP
    {
        get { return maxMP; }
    }

    public string Skill_01Message
    {
        get { return skill_01Message; }
    }
    public string Skill_02Message
    {
        get { return skill_02Message; }
    }
    public string Skill_03Message
    {
        get { return skill_03Message; }
    }
}

