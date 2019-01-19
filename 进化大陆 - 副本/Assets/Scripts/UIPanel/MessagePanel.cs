using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MessagePanel : BasePanel
{
    private Text message;
    private string data = null;

    private void Start()
    {
        message = GameObject.Find("Text").GetComponent<Text>();
    }

    private void Update()
    {
        if (data==null)
        {
            return;
        }
        ShowMessage(data);
        data = null;
    }

    public override void OnEnter()
    {
        if (message==null)
        {
            message = GameObject.Find("Text").GetComponent<Text>();
        }

        message.enabled = false;
        uiMng.InjectMsgPanel(this);
    }

    public void ShowMessageSync(string data)
    {
        this.data = data;
    }

    public void ShowMessage(string msg)
    {
        message.CrossFadeAlpha(1, 0.2f, false);
        message.text = msg;
        message.enabled = true;
        Invoke("Hide", 2f);
    }
    private void Hide()
    {
        message.CrossFadeAlpha(0, 1, false);
    }

}
