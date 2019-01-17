using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;

public class LevelUpRequest : BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.LevelUp;
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        Debug.Log("levelup");
        gameFacade.LevelUp(data);

    }
}
