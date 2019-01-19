using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;


public class JoinRoomRequest : BaseRequest
{
    private RoomListPanel roomListPanel;
    private RoomItem roomItem;
    

    public override void Awake()
    {
        roomListPanel = transform.GetComponent<RoomListPanel>();
        roomItem = transform.GetComponent<RoomItem>();
        requestCode = RequestCode.Room;
        actionCode = ActionCode.JoinRoom;
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split('^');
       
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        roomListPanel.OnJoinRoomResponse(returnCode,strs[1]);
    }
}
