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
        this.gameObject.SetActive(true);
    }

    public virtual void OnPause()
    {
        this.gameObject.SetActive(false);
    }

    public virtual void OnResume()
    {
        this.gameObject.SetActive(true);
    }

    public virtual void OnExit()
    {
        this.gameObject.SetActive(false);
    }

    protected void OnClick()
    {
        EventCenter.Broadcast(EventType.PlayNormalSound,Audios.Sound_ButtonClick);
    }
}
