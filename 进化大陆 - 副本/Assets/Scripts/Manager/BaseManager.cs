using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseManager
{
    protected GameFacade gameFacade;
    public BaseManager(GameFacade gameFacade)
    {
        this.gameFacade = gameFacade;
    }

    public virtual void OnInit()
    {
        
    }

    public virtual void OnUpdate(){}
    public virtual void OnDestroy() { }
}
