using System;
using System.Collections.Generic;
using System.Text;
using Common;
using UnityEngine;
using UnityEngine.UI;


public class MonsterTakeDamageRequest : BaseRequest
{
    private MonsterData monsterData;

    public override void Awake()
    {
        monsterData = GetComponent<MonsterData>();
        requestCode = RequestCode.Game;
        actionCode = ActionCode.MonsterTakeDamage;
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        Debug.Log("damage");
        string[] strs = data.Split(',');
        gameFacade.GetMonsterList()[int.Parse(strs[0])].OnTakeDamagerResponse(strs[1]);
    }
}

