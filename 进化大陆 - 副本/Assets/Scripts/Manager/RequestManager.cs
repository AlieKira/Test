using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class RequestManager : BaseManager {
    public RequestManager(GameFacade gameFacade) : base(gameFacade) { }
    public Dictionary<ActionCode, BaseRequest> requestDic = new Dictionary<ActionCode, BaseRequest>();

    public void AddRequest(ActionCode actionCode,BaseRequest request)
    {
        if (requestDic.ContainsKey(actionCode))
        {
            Debug.LogWarning("ActionCode:["+actionCode+" has been exist]");
            return;
        }
        requestDic.Add(actionCode,request);
    }

    public void RemoveRequest(ActionCode actionCode)
    {
        requestDic.Remove(actionCode);
    }

    public void HandleResponse(ActionCode actionCode, string data)
    {
        BaseRequest request;
        bool isGot = requestDic.TryGetValue(actionCode, out request);
        if (isGot==false)
        {
            Debug.Log(actionCode.ToString());
            Debug.LogWarning("Can't find the request with ActionCode:["+actionCode+"]");
        }
        request.OnResponse(data);
    }
}
