using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;

public class StartPlayRequest:BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.StartPlay;
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        Debug.Log("StartPlay");
        gameFacade.OnStartPlayResponse(data);
    }
}

