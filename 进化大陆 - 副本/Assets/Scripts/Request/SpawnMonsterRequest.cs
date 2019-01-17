using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;


public class SpawnMonsterRequest:BaseRequest
{



    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.SpawnMonster;
        base.Awake();
    }

    public void SendRequest()
    {
        base.SendRequest("r");
    }

    public override void OnResponse(string data)
    {
        gameFacade.SpawnMonster(data);
    }
}
