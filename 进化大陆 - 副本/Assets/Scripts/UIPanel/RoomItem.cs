using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem:MonoBehaviour
{
    public Text username;
    public Text menberCount;
    public Button joinButton;

    private int userid;
    private RoomListPanel roomListPanel;

    public int UserID
    {
        get { return userid; }
    }

    void Start()
    {
        if (joinButton != null)
        {
            joinButton.onClick.AddListener(OnJoinClick);
        }
    }
    public void SetRoomInfo(int id, string username, int clientCount, RoomListPanel panel)
    {
        this.userid = id;
        this.username.text = username;
        this.menberCount.text = clientCount + "/6";
        this.roomListPanel = panel;
    }

    private void OnJoinClick()
    {
        roomListPanel.OnJoinClick(userid);
    }

    public void DestroySelf()
    {
        GameObject.Destroy(this.gameObject);
    }
}
