using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel:MonoBehaviour
{
    public UIManager uiMng;
    public GameFacade gameFacade;

    public UIManager UIMng
    {
        set { uiMng = value; }
    }

    public GameFacade GameFacade
    {
        set { gameFacade = value; }
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnPause()
    {
    }

    public virtual void OnResume()
    {
    }

    public virtual void OnExit()
    {
    }
}
