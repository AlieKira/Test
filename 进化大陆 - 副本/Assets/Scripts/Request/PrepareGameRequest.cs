using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;

public class PrepareGameRequest:BaseRequest
{
    private RoomPanel roomPanel;

    public override void Awake()
    {
        roomPanel = transform.GetComponent<RoomPanel>();
        requestCode = RequestCode.Room;
        actionCode = ActionCode.PrepareGame;
        base.Awake();
    }

    public void SendRequest()
    {
        base.SendRequest("r");
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        roomPanel.OnPrepareGameResponse(returnCode,int.Parse(strs[1]));
    }
}