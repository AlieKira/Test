using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;


public class AttackRequest : BaseRequest
{
    private bool isOnResponse = false;
    private string buffer;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Attack;

        base.Awake();
    }

    public void Update()
    {
        if (isOnResponse)
        {
            foreach (Transform temp in gameFacade.GetRoleList())
            {
                if (temp.GetComponent<RoleData>().UserID == int.Parse(buffer))
                {
                    temp.GetComponent<PlayerControl>().OnAttackResponse();
                }


            }

            isOnResponse = false;
        }
    }

    public void SendRequest(int targetID,bool isPlayer)
    {
        if (isPlayer)
        {
            base.SendRequest("0" + ',' + targetID);
        }
        if (!isPlayer)
        {
            base.SendRequest("1" + ',' + targetID);
        }
    }

    public override void OnResponse(string data)
    {
        buffer = data;
        isOnResponse = true;
    }
}

