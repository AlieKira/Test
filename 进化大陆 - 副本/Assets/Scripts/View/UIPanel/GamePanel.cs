using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class GamePanel : BasePanel
{
    private GameObject rolePrefab;

    private Button CharacterBtn;
    private Button packageBtn;
    private Button shopBtn;
    private Button pauseBtn;

    private RoleType roleType;
    private GameObject careerPanel;

    private Image headPortrait;

    private Text timer;

    private Text gameTime;

    private string gameTimeString;

    private bool isShowTimer = false;

    private bool isUpdateGameTime = false;

    private int time;

    private Slider hpSlider;
    private Slider expSlider;
    private Text lv;

    public GameObject[] toolTips;

    private void Start()
    {
        uiMng.gamePanel = this;
        headPortrait = transform.Find("Interface/RoleInfo/HeadPortrait/Image").GetComponent<Image>();
        timer = transform.Find("Timer").GetComponent<Text>();
        gameTime = transform.Find("GameTimer").GetComponent<Text>();
        packageBtn = transform.Find("Package").GetComponent<Button>();
        shopBtn = transform.Find("Shop").GetComponent<Button>();
        pauseBtn = transform.Find("PauseButton").GetComponent<Button>();
        CharacterBtn = transform.Find("Character").GetComponent<Button>();
        CharacterBtn.onClick.AddListener(OnCharacterClick);
        pauseBtn.onClick.AddListener(OnPauseClick);
        packageBtn.onClick.AddListener(OnPackageClick);
        shopBtn.onClick.AddListener(OnShopClick);

        hpSlider = transform.Find("Interface/RoleInfo/HP").GetComponentInChildren<Slider>();
        expSlider = transform.Find("Interface/RoleInfo/EXP").GetComponentInChildren<Slider>();
        lv = transform.Find("Interface/RoleInfo/LV").GetComponent<Text>();
    }


    private void Update()
    {
        if (isShowTimer)
        {
            timer.gameObject.SetActive(true);
            timer.text = time.ToString();
            TimerAnim();
            isShowTimer = false;
            GameManager.Instance.PlayNormalSound(Audios.Sound_Timer);
        }

        if (isUpdateGameTime)
        {
            gameTime.text = gameTimeString;
            isUpdateGameTime = false;
        }
    }

    public void ShowTimer()  
    {
        Thread run = new Thread(TimerRun);
        run.Start();
    }

    private void TimerRun()
    {
        for (int i = 3; i > 0; i--)
        {
            time = i;
            isShowTimer = true;
            Thread.Sleep(1000);
        }
    }

    public void UpdateGameTimer(int time)
    {
        int minute = time / 60;
        int second = (int)(time % 60f);
        gameTimeString = minute.ToString("00") + ":" + second.ToString("00");
        isUpdateGameTime = true;
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


    public void SetHeadPoriait(string path)
    {
        headPortrait.sprite = Resources.Load<Sprite>(path);
    }

    //点击背包按键
    public void OnPackageClick()
    {
        uiMng.ShowInventory(UIPanelType.KnapsackPanel);
    }

    //点击角色按键
    public void OnCharacterClick()
    {
        uiMng.ShowInventory(UIPanelType.CharacterPanel);
    }

    //点击商店按键
    public void OnShopClick()
    {
        uiMng.ShowInventory(UIPanelType.ShopPanel);
    }

    //点击暂停按键
    public void OnPauseClick()
    {
        Debug.Log(1);
        uiMng.PushPanel(UIPanelType.PausePanel);
    }

    public override void OnEnter()
    {
        gameObject.SetActive(true);
    }
    public override void OnExit()
    {
        gameObject.SetActive(false);
    }

    public override void OnPause()
    {
        if (shopBtn==null||packageBtn==null|| pauseBtn==null)
        {
            packageBtn = transform.Find("Package").GetComponent<Button>();
            shopBtn = transform.Find("Shop").GetComponent<Button>();
            pauseBtn = transform.Find("PauseButton").GetComponent<Button>();
        }
        shopBtn.enabled = false;
        packageBtn.enabled = false;
        pauseBtn.enabled = false;
        GameManager.Instance.PauseRole();
    }

    public void PauseButton(UIPanelType uiPanelType)
    {
        switch (uiPanelType)
        {
            case UIPanelType.KnapsackPanel:
                packageBtn.enabled = false;
                CharacterBtn.enabled = true;
                shopBtn.enabled = true;
                break;
            case UIPanelType.CharacterPanel:
                CharacterBtn.enabled = false;
                shopBtn.enabled = true;
                packageBtn.enabled = true;
                break;
            case UIPanelType.ShopPanel:
                shopBtn.enabled = false;
                packageBtn.enabled = true;
                CharacterBtn.enabled = true;
                break;
        }
    }

    public override void OnResume()
    {
        shopBtn.enabled = true;
        CharacterBtn.enabled = true;
        packageBtn.enabled = true;
        pauseBtn.enabled = true;
        GameManager.Instance.ResumeRole();
    }

    public void UpdateRoleInterface(int hp, int maxHP, int exp, int lv)
    {
        hpSlider.value = hp / (float) maxHP;
        expSlider.value = exp / ((float) (lv * 100));
        this.lv.text = lv.ToString();
    }
}
