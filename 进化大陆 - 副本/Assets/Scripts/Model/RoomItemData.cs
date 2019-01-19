using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RoomItemData
{
    private string username;

    public string Username
    {
        get { return username; }
    }
    private int clientCount;

    public int ClientCount
    {
        get { return clientCount; }
    }
    private int userid;

    public int Userid
    {
        get { return userid; }
    }

    public RoomItemData(int userid, string username, int clientCount)
    {
        this.userid = userid;
        this.username = username;
        this.clientCount=clientCount;
    }
}
