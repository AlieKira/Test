using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;


[Serializable]
public class MonsterInfo :ISerializationCallbackReceiver
{
    [NonSerialized]
    public MonsterType monsterType;

    public string monsterTypeString;
    public string prefabPath;

    public void OnAfterDeserialize()
    {
        MonsterType temp = (MonsterType)Enum.Parse(typeof(MonsterType), "Cactus");
        Debug.Log(1);
        monsterType = MonsterType.Cactus;
    }

    public void OnBeforeSerialize() { }



    

}
