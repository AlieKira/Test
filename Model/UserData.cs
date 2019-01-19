using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class UserData
{
    public UserData(string userData)
    {
        string[] strs = userData.Split(',');
        this.userID = int.Parse(strs[0]);
        this.username = strs[1];
        this.totalCount = int.Parse(strs[2]);
        this.winCount = int.Parse(strs[3]);
        this.headProtraitPath = strs[4];
    }

    public UserData(string userID, string username, string totalCount, string winCount)
    {
        this.userID = int.Parse(userID);
        this.username = username;
        this.totalCount = int.Parse(totalCount);
        this.winCount = int.Parse(winCount);
    }

    public UserData(string userID, string username, string totalCount, string winCount,string headProtraitTypeStr)
    {
        this.userID = int.Parse(userID);
        this.username = username;
        this.totalCount = int.Parse(totalCount);
        this.winCount = int.Parse(winCount);
        this.headProtraitPath = headProtraitTypeStr;
    }

    public void IsHouseOwner()
    {
        isHouseOwner = true;
    }

    public int userID;
    public string username;
    public int totalCount;
    public int winCount;
    public string headProtraitPath;
    public bool isHouseOwner=false;
}
