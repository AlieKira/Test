using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;


public class ListRoomRequest:BaseRequest
{
    private List<RoomItemData> roomItemList;
    private RoomListPanel roomListPanel;
    public override void Awake()
    {
        roomListPanel = GetComponent<RoomListPanel>();
        requestCode = RequestCode.Room;
        actionCode = ActionCode.ListRoom;
        base.Awake();
    }

    public void SendRequest()
    {
        base.SendRequest("r");
    }

    public override void OnResponse(string data)
    {
        roomItemList = new List<RoomItemData>();
        if (data != "0")
        {
            string[] countArray = data.Split('|');
            foreach (string ca in countArray)
            {
                string[] strs = ca.Split(',');
                roomItemList.Add(new RoomItemData(int.Parse(strs[0]),strs[1],int.Parse(strs[2])));
            }
        }

        roomListPanel.LoadRoomItemSync(roomItemList);
    }
}

