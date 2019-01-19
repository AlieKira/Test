using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;


public class RoleTakeDamageRequest : BaseRequest
{
    private MonsterData monsterData;
    private bool isOnResponse = false;
    private string buffer;


    private void Update()
    {
        if (isOnResponse)
        {
            string[] strs = buffer.Split(',');
            foreach (Transform temp in gameFacade.GetRoleList())
            {
                if (temp.GetComponent<RoleData>().UserID==int.Parse(strs[0]))
                {
                    temp.GetComponent<RoleData>().OnTakeDamagerResponse(strs[1]);
                }
            }

            isOnResponse = false;
        }
    }

    public override void Awake()
    {
        monsterData = GetComponent<MonsterData>();
        requestCode = RequestCode.Game;
        actionCode = ActionCode.RoleTakeDamage;
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        isOnResponse = true;
        buffer = data;
        
    }
}

