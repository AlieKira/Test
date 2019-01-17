using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;

[Serializable]
public class RoleInfo:ISerializationCallbackReceiver
{
    [NonSerialized]
    public RoleType roleType;

    public string roleTypeString;
    public string prefabPath;
    public string imagePath;
    public string skill_01ImagePath;
    public string skill_02ImagePath;
    public string skill_03ImagePath;
    public string attack;
    public string defence;
    public string maxHP;
    public string maxMP;
    public string skill_01Message;
    public string skill_02Message;
    public string skill_03Message;
     
    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        RoleType temp = (RoleType)Enum.Parse(typeof(RoleType), roleTypeString);
        Debug.Log(2);
        roleType = temp;
    }
}
