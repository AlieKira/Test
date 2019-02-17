using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MessagePanel : MonoBehaviour
{
    private Text message;

    private void Start()
    {
        message = GameObject.Find("Text").GetComponent<Text>();
        this.gameObject.SetActive(false);
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
