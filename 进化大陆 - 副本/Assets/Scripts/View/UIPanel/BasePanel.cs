using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel:MonoBehaviour
{
    protected UIManager uiMng;

    public UIManager UIMng
    {
        set { uiMng = value; }
    }

    protected View view;

    public View View
    {
        set { view = value; }
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
