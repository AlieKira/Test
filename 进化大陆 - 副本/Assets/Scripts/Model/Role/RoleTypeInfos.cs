using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


/// <summary>
/// 解析Json文件，整合各种角色的信息
/// </summary>
public class RoleTypeInfos
{
    private List<RoleTypeInfo> roleTypeInfoList;

    public List<RoleTypeInfo> ParseRoleJson()
    {
        roleTypeInfoList = new List<RoleTypeInfo>();
        TextAsset roleText = Resources.Load<TextAsset>("RoleTypeInfos");
        string roleJson = roleText.ToString();
        JSONObject j = new JSONObject(roleJson);
        foreach (JSONObject temp in j.list)
        {
            RoleTypeInfo roleTypeInfo = new RoleTypeInfo();
            roleTypeInfo.roleType = (RoleType)Enum.Parse(typeof(RoleType), temp["roleTypeString"].str);
            roleTypeInfo.prefabPath = temp["prefabPath"].str;
            roleTypeInfo.maxHP = (int)temp["maxHP"].n;
            roleTypeInfo.strength = (int)temp["strength"].n;
            roleTypeInfo.intellect = (int)temp["intellect"].n;
            roleTypeInfo.agility = (int)temp["agility"].n;
            roleTypeInfo.stamina = (int)temp["stamina"].n;
            roleTypeInfo.attack = (int)temp["attack"].n;
            roleTypeInfo.attackDis = (int)temp["attackDis"].n;
            roleTypeInfo.attackPrefab = temp["attackPrefab"].str;
            roleTypeInfo.headPortrait = temp["headPortrait"].str;
            roleTypeInfoList.Add(roleTypeInfo);
        }
        return roleTypeInfoList;
    }
}
