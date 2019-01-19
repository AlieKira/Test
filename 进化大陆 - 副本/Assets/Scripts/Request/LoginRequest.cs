using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class LoginRequest : BaseRequest
{
    private LoginPanel loginPanel;
    public override void Awake()
    {
        loginPanel = GetComponent<LoginPanel>();
        requestCode = RequestCode.User;
        actionCode = ActionCode.Login;
        base.Awake();
    }

    public void SendRequest(string username,string password)
    {
        string data = username + "," + password;
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        //Debug.Log(strs[1]+","+strs[2] + "," + strs[3] + "," + strs[4] + "," + strs[5]);
        if (returnCode==ReturnCode.Success)
        {
            if (strs[5] == "n")
            {
                loginPanel.setHeadPortrait = true;
            }
                UserData userData = new UserData(strs[1], strs[2], strs[3], strs[4], strs[5]);
                gameFacade.SetUserData(userData);
        }
        loginPanel.OnLoginResponse(returnCode);
        
    }
}
