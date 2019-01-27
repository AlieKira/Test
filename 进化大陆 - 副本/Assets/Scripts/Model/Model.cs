using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Model:MonoBehaviour
{
    private RoleTypeInfos roleTypeInfos;
    private MonsterTypeInfos monsterTypeInfos;
    private Items items;
    private RecordData recordData;

    private List<RoleTypeInfo> roleTypeList;
    private List<MonsterTypeInfo> monsterTypeList;
    private List<Item> itemList;

    public RoleData roleData; 

    private void Start()
    {
        roleTypeInfos = new RoleTypeInfos();
        monsterTypeInfos = new MonsterTypeInfos();
        items = new Items();
        recordData=new RecordData();
        recordData.OnStart();

        roleTypeList = roleTypeInfos.ParseRoleJson();
        monsterTypeList = monsterTypeInfos.ParseMonsterJson();
        itemList = items.ParseItemJson();
    }

    public string GetHeadPortrait()
    {
        return GetRoleTypeInfo(roleData.roleType).headPortrait;
    }

    public MonsterTypeInfo GetMonsterTypeInfo(MonsterType monsterType)
    {
        foreach (MonsterTypeInfo temp in monsterTypeList)
        {
            if (temp.monsterType == monsterType)
            {
                return temp;
            }
        }
        return null;
    }

    public RoleTypeInfo GetRoleTypeInfo(RoleType roleType)
    {
        foreach (RoleTypeInfo temp in roleTypeList)
        {
            if (temp.roleType == roleType)
            {
                return temp;
            }
        }
        return null;
    }

    public Item GetItemById(int id)
    {
        foreach (Item item in itemList)
        {
            if (item.ID == id)
            {
                return item;
            }
        }
        return null;
    }

    public string[] GetEasyRecords()
    {
        return recordData.GetEasyRecords();
    }

    public string[] GetCommonRecords()
    {
        return recordData.GetCommonRecords();
    }

    public string[] GetDifficultRecords()
    {
        return recordData.GetDifficultRecords();
    }

    public Item GetRandomItem(MonsterType monsterType)
    {
        int rangeMin = GetMonsterTypeInfo(monsterType).rangeMin;
        int rangeMax= GetMonsterTypeInfo(monsterType).rangeMax;
        Item item= items.GetRandomItem(rangeMin, rangeMax);
        if (item==null)
        {
            Debug.Log(1);
        }
        return item;
    }

    public int UpdateRecord(int level,int minute,int second)
    {
        return recordData.UpdateRecord(level,minute,second);
    }

    public void SetEditor(string s)
    {
        recordData.SetEditor(s);
    }

    public int GetDifficultyDegree()
    {
        return recordData.GetDifficultyDegree();
    }

    public Difficulty GetDifficulty()
    {
        return recordData.GetDifficulty();
    }

    public void LoadRecord()
    {
        recordData.LoadRecord();
    }

    public string LoadSetting()
    {
        return recordData.LoadSetting();
    }
}

