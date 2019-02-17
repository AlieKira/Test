using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 解析Json文件，整合各种怪物的信息
/// </summary>
public class MonsterTypeInfos
{
    private List<MonsterTypeInfo> monsterTypeInfoList;

    public List<MonsterTypeInfo> ParseMonsterJson()
    {
        monsterTypeInfoList = new List<MonsterTypeInfo>();
        TextAsset monsterText = Resources.Load<TextAsset>("MonsterTypeInfos");
        string monsterJson = monsterText.ToString();
        JSONObject j=new JSONObject(monsterJson);
        foreach (JSONObject temp in j.list)
        {
            MonsterTypeInfo monsterTypeInfo = new MonsterTypeInfo();
            monsterTypeInfo.monsterType = (MonsterType)Enum.Parse(typeof(MonsterType),temp["monsterTypeString"].str);
            monsterTypeInfo.prefabPath = temp["prefabPath"].str;
            monsterTypeInfo.maxHP = (int) temp["maxHP"].n;
            monsterTypeInfo.attack = (int)temp["attack"].n;
            monsterTypeInfo.attackDis = (int)temp["attackDis"].n;
            monsterTypeInfo.skillDis = (int)temp["skillDis"].n;
            monsterTypeInfo.attackEffect = temp["attackEffect"].str;
            monsterTypeInfo.skillEffect = temp["skillEffect"].str;
            monsterTypeInfo.rangeMin = (int)temp["rangeMin"].n;
            monsterTypeInfo.rangeMax = (int)temp["rangeMax"].n;
            monsterTypeInfo.exp = (int) temp["exp"].n;
            monsterTypeInfoList.Add(monsterTypeInfo);
        }
        return monsterTypeInfoList;
    }
}

