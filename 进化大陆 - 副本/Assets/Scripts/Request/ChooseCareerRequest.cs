using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;


public class ChooseCareerRequest:BaseRequest
{
    private GamePanel gamePanel;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.ChooseCareer;
        gamePanel = GetComponent<GamePanel>();
        base.Awake();
    }

    public void SendRequest(RoleType roleType)
    {
       base.SendRequest(((int)roleType).ToString());
    }

    public override void OnResponse(string data)
    {
        Debug.Log(data);
        if (data!="-1")
        {
            RoleType roleType = (RoleType)int.Parse(data);
            gamePanel.OnChooseCareer(roleType);
        }
    }
}
