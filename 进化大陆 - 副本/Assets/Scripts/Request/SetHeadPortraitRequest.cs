using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class SetHeadPortraitRequest : BaseRequest
{
    private HeadPortraitPanel headPortraitPanel;
    public override void Awake()
    {
        headPortraitPanel = GetComponent<HeadPortraitPanel>();
        requestCode = RequestCode.User;
        actionCode = ActionCode.SetHeadPortrait;
        base.Awake();
    }

    public void SendRequest(string userid, string path)
    {
        string data = userid + ',' + path;
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        gameFacade.SetHeadPortraitPath(strs[1]);
        headPortraitPanel.OnSetHeadPortraitResponse(returnCode);
    }
}
