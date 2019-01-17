using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;


public class RoleDeadRequest : BaseRequest
{
    private bool isGetResponse = false;
    private string buffer;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.RoleDead;
        base.Awake();
    }

    private void Update()
    {
        if (isGetResponse)
        {
            foreach (Transform temp in gameFacade.GetRoleList())
            {
                if (temp.GetComponent<RoleData>().UserID== int.Parse(buffer))
                {
                    temp.GetComponent<RoleData>().OnDeadResponse();
                }
            }

            isGetResponse = false;
        }
    }

    public override void OnResponse(string data)
    {
        Debug.Log("RoleDead:"+data);
        buffer = data;
        isGetResponse = true;
        
    }
}

