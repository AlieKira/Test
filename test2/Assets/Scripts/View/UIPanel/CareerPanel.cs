using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class CareerPanel : BasePanel
{
    #region CareerBtn
    private Button archerButton;

    private Button barbarianButton;

    private Button casualButton;

    private Button knightButton;

    private Button mageButton;
    #endregion

    private RoleType roleType;

    private GameObject panel;

    private Text timer;

    private int time;

    private bool isShowTimer = false;

    private void Start()
    {
        panel = transform.Find("Panel").gameObject;
        archerButton = transform.Find("Panel/Layout/Archer").GetComponent<Button>();
        barbarianButton = transform.Find("Panel/Layout/Barbarian").GetComponent<Button>();
        casualButton = transform.Find("Panel/Layout/Casual").GetComponent<Button>();
        knightButton = transform.Find("Panel/Layout/Knight").GetComponent<Button>();
        mageButton = transform.Find("Panel/Layout/Mage").GetComponent<Button>();
        timer = transform.Find("Timer").GetComponent<Text>();

        archerButton.onClick.AddListener(OnArcherBtnClick);
        barbarianButton.onClick.AddListener(OnBarbarianBtnClick);
        casualButton.onClick.AddListener(OnCasualBtnClick);
        knightButton.onClick.AddListener(OnKnightBtnClick);
        mageButton.onClick.AddListener(OnMageBtnClick);
    }

    private void Update()
    {
        if (isShowTimer)
        {
            timer.gameObject.SetActive(true);
            timer.text = time.ToString();
            TimerAnim();
            isShowTimer = false;
            EventCenter.Broadcast(EventType.PlayNormalSound, Audios.Sound_Timer);
            if (time<=0)
            {
                Hide();
            }
        }
    }


    private void Hide()
    {
        panel.SetActive(true);
        uiMng.PopPanel();
        EventCenter.Broadcast(EventType.StartPlay,roleType);
    }

    public void ShowTimer()
    {
        panel.SetActive(false);
        Thread run = new Thread(TimerRun);
        run.Start();
    }

    private void TimerRun()
    {
        for (int i = 3; i > -1; i--)
        {
            time = i;
            isShowTimer = true;
            Thread.Sleep(1000);
        }
    }

    private void TimerAnim()
    {

        timer.transform.localScale = Vector3.one;
        Color tempColor = timer.color;
        tempColor.a = 1;
        timer.color = tempColor;
        timer.transform.DOScale(2, 0.3f).SetDelay(0.3f);
        timer.DOFade(0, 0.3f).SetDelay(0.3f).OnComplete(() => timer.gameObject.SetActive(false));
    }

    #region OnBtnClick

    private void OnArcherBtnClick()
    {
        roleType = RoleType.Archer;
        ShowTimer();
    }

    private void OnBarbarianBtnClick()
    {
        OnClick();
        roleType = RoleType.Barbarian;
        ShowTimer();
    }

    private void OnCasualBtnClick()
    {
        OnClick();
        roleType = RoleType.Casual;
        ShowTimer();
    }

    private void OnKnightBtnClick()
    {
        OnClick();
        roleType = RoleType.Knight;
        ShowTimer();
    }

    private void OnMageBtnClick()
    {
        OnClick();
        roleType = RoleType.Mage;
        ShowTimer();
    }

    #endregion


    #region Pre_Treatment

    public override void OnEnter()
    {
        this.gameObject.SetActive(true);
    }

    public override void OnPause()
    {
        this.gameObject.SetActive(false);
    }

    public override void OnResume()
    {
        this.gameObject.SetActive(true);
    }

    public override void OnExit()
    {
        this.gameObject.SetActive(false);
    }

    #endregion

}

