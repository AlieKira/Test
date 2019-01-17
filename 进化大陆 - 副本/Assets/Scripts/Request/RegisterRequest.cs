using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

class RegisterRequest : BaseRequest
{
    private RegisterPanel registerPanel;
    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Register;
        registerPanel = GetComponent<RegisterPanel>();
        base.Awake();
    }

    public void SendRequest(string username,string password)
    {
        string dataBytes = username + ',' + password;
        base.SendRequest(dataBytes);
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode) int.Parse(strs[0]);
        registerPanel.OnRegisterResponse(returnCode);
    }
}
