using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using Common;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class LoginPanel :BasePanel{
    private InputField usernameInput;
    private InputField passworeInput;
    private Button loginButton;
    private Button registerButton;
    private Button closeButton;
    private LoginRequest loginRequest;
    public bool setHeadPortrait=false;

    private void Start()
    {
        loginRequest = GetComponent<LoginRequest>();
        usernameInput = GameObject.Find("UsernameLabel/UsernameInput").GetComponent<InputField>();
        passworeInput = GameObject.Find("PasswordLabel/PasswordInput").GetComponent<InputField>();
        loginButton = GameObject.Find("LoginButton").GetComponent<Button>();
        registerButton = GameObject.Find("RegisterButton").GetComponent<Button>();
        closeButton = GameObject.Find("CloseButton").GetComponent<Button>();
        loginButton.onClick.AddListener(OnLoginClick);
        registerButton.onClick.AddListener(OnRegisterClick);
        closeButton.onClick.AddListener(OnCloseClick);
    }

    private void OnLoginClick()
    {
        string msg = null;
        if (usernameInput==null)
        {
            msg+="用户名不能为空 ";
        }
        if (passworeInput==null)
        {
            msg += "密码不能为空";
        }
        if (msg!=null)
        {
            uiMng.ShowMessage(msg);
            return;
        }
        loginRequest.SendRequest(usernameInput.text, passworeInput.text);
    }
    
    private void OnRegisterClick()
    {
        uiMng.PushPanel(UIPanelType.RegisterPanel);
    }

    private void OnCloseClick()
    {
        uiMng.PopPanel();
    }
    public void OnLoginResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Fail)
        {
            gameFacade.ShowMessageSync("登录失败，用户名或密码错误");
        }
        else
        {
            if (setHeadPortrait==false)
            {
                uiMng.PushPanelSync(UIPanelType.RoomListPanel);
            }
            else
            {
                uiMng.PushPanelSync(UIPanelType.HeadPortraitPanel);
                setHeadPortrait = false;
            }
            
        }
    }

    public override void OnEnter()
    {
        AppearAnim();
    }

    public override void OnPause()
    {
        HideAnim();
    }

    public override void OnResume()
    {
        AppearAnim();
    }

    public override void OnExit()
    {
        HideAnim();
    }

    private void AppearAnim()
    {
        this.gameObject.transform.localScale=Vector3.one;
        this.gameObject.SetActive(true);
        this.gameObject.transform.DOScale(1, 0.5f);
    }

    public void HideAnim()
    {
        this.gameObject.SetActive(false);
            
    }

}

