using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;

public class ExitRoomRequest : BaseRequest
{
    private RoomPanel roomPanel;

    public override void Awake()
    {
        roomPanel = GetComponent<RoomPanel>();
        requestCode = RequestCode.Room;
        actionCode = ActionCode.ExitRoom;
        base.Awake();
    }

    public void SendRequest()
    {
        base.SendRequest("r");
    }

    public override void OnResponse(string data)
    {
        ReturnCode returnCode = (ReturnCode)int.Parse(data);
        roomPanel.OnExitResponse(returnCode);
    }
}
