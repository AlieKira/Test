using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;


public class MonsterDeadRequest : BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.MonsterDead;
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        Debug.Log("dead");
        string[] strs = data.Split(',');
        gameFacade.GetMonsterList()[int.Parse(strs[0])].OnDeadResponse();
    }
}

