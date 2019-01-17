using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;
using UnityEngine.UI;

class RegisterPanel : BasePanel
{
    private InputField usernameInput;
    private InputField passwordInput;
    private InputField rePasswordInput;
    private Button closeButton;
    private Button registerButton;
    private RegisterRequest registerRequest;
    private bool isExit=false;

    private void Start()
    {
        registerRequest = GetComponent<RegisterRequest>();
        usernameInput = GameObject.Find("UsernameLabel/UsernameInput").GetComponent<InputField>();
        passwordInput = GameObject.Find("PasswordLabel/PasswordInput").GetComponent<InputField>();
        rePasswordInput = GameObject.Find("RePasswordLabel/RePasswordInput").GetComponent<InputField>();
        closeButton = GameObject.Find("CloseButton").GetComponent<Button>();
        registerButton = GameObject.Find("RegisterButton").GetComponent<Button>();
        closeButton.onClick.AddListener(OnCloseClick);
        registerButton.onClick.AddListener(OnRegisterClick);
    }

    private void Update()
    {
        if (isExit==true)
        {
            uiMng.ShowMessage("注册成功");
            Invoke("ExitAction",2);
            isExit = false;
        }
    }

    private void ExitAction()
    {
        uiMng.PopPanel();
    }
    public void OnRegisterResponse(ReturnCode returnCode)
    {
        if (returnCode==ReturnCode.Fail)
        {
            gameFacade.ShowMessageSync("注册失败");
        }
        else
        {
            isExit = true;
        }
    }
    private void OnCloseClick()
    {
        uiMng.PopPanel();
    }

    private void OnRegisterClick()
    {
        string msg=null;
        if (usernameInput.text==null)
        {
            msg += "用户名不能为空";
        }

        if (passwordInput.text==null)
        {
            msg += "密码不能为空";
        }
        else if (rePasswordInput.text != passwordInput.text)
        {
            msg += "两次密码不相同";
        }

        if (msg!=null)
        {
            gameFacade.ShowMessage(msg);
            return;
        }
        registerRequest.SendRequest(usernameInput.text,passwordInput.text);
    }

    public override void OnEnter()
    {
        this.gameObject.SetActive(true);
    }

    public override void OnExit()
    {
        this.gameObject.SetActive(false);
    }
}