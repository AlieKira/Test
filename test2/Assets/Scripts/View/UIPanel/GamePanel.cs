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

    private Dictionary<UIPanelType, Button> btnDic=new Dictionary<UIPanelType, Button>();

    private Text gameTime;

    private string gameTimeString;

    private bool hasHeadPortrait=true;

    private bool isUpdateGameTime = false;

    private Slider hpSlider;
    private Slider expSlider;
    private Text lv;

    public GameObject[] toolTips;

    private void Start()
    {
        //uiMng.gamePanel = this;
        headPortrait = transform.Find("Interface/RoleInfo/HeadPortrait/Image").GetComponent<Image>();

        gameTime = transform.Find("GameTimer").GetComponent<Text>();
        packageBtn = transform.Find("Package").GetComponent<Button>();
        shopBtn = transform.Find("Shop").GetComponent<Button>();
        pauseBtn = transform.Find("PauseButton").GetComponent<Button>();
        CharacterBtn = transform.Find("Character").GetComponent<Button>();


        btnDic.Add(UIPanelType.KnapsackPanel,packageBtn);
        btnDic.Add(UIPanelType.CharacterPanel, CharacterBtn);
        btnDic.Add(UIPanelType.ShopPanel, shopBtn);
        hpSlider = transform.Find("Interface/RoleInfo/HP").GetComponentInChildren<Slider>();
        expSlider = transform.Find("Interface/RoleInfo/EXP").GetComponentInChildren<Slider>();
        lv = transform.Find("Interface/RoleInfo/LV").GetComponent<Text>();

        AddListener();
    }

    private void Update()
    {
        if (!hasHeadPortrait)
        {
            string path = Model.Instance.GetHeadPortrait();
            headPortrait.sprite = Resources.Load<Sprite>(path);
        }
        if (isUpdateGameTime)
        {
            int time = Model.Instance.GetGameTime();
            int minute = time / 60;
            int second = (int)(time % 60f);
            gameTimeString = minute.ToString("00") + ":" + second.ToString("00");
            gameTime.text = gameTimeString;
            isUpdateGameTime = false;
        }
    }

    private void AddListener()
    {
        EventCenter.AddListener(EventType.OverGame, UpdateInterfaceInfo);
        EventCenter.AddListener(EventType.UpdateGameTime, UpdateGameTime);
        EventCenter.AddListener(EventType.UpdateInterfaceInfo, UpdateInterfaceInfo);
        pauseBtn.onClick.AddListener(delegate()
        {
            OnClick();
            EventCenter.Broadcast(EventType.PushPanel, UIPanelType.PausePanel);
        });
        packageBtn.onClick.AddListener(delegate ()
        {
            OnClick();
            EventCenter.Broadcast<UIPanelType>(EventType.HidePanel, UIPanelType.All);
            EventCenter.Broadcast<UIPanelType>(EventType.ShowPanel, UIPanelType.KnapsackPanel);
        });
        CharacterBtn.onClick.AddListener(delegate ()
        {
            OnClick();
            EventCenter.Broadcast<UIPanelType>(EventType.HidePanel,UIPanelType.All);
            EventCenter.Broadcast<UIPanelType>(EventType.ShowPanel, UIPanelType.KnapsackPanel);
            EventCenter.Broadcast<UIPanelType>(EventType.ShowPanel, UIPanelType.CharacterPanel);
        });
        shopBtn.onClick.AddListener(delegate ()
        {
            OnClick();
            EventCenter.Broadcast<UIPanelType>(EventType.HidePanel, UIPanelType.All);
            EventCenter.Broadcast<UIPanelType>(EventType.ShowPanel, UIPanelType.KnapsackPanel);
            EventCenter.Broadcast<UIPanelType>(EventType.ShowPanel, UIPanelType.ShopPanel);
        });
    }

    public void UpdateGameTime()
    {
        isUpdateGameTime = true;
    }

    public override void OnEnter()
    {
        if (hpSlider == null || expSlider == null || lv == null) 
        {
            hpSlider = transform.Find("Interface/RoleInfo/HP").GetComponentInChildren<Slider>();
            expSlider = transform.Find("Interface/RoleInfo/EXP").GetComponentInChildren<Slider>();
            lv = transform.Find("Interface/RoleInfo/LV").GetComponent<Text>();
        }

        gameObject.SetActive(true);
        hpSlider.value = 1;
        expSlider.value = 0;
        lv.text = "1";
    }
    public override void OnExit()
    {
        gameObject.SetActive(false);
    }

    public override void OnPause()
    {
        if (shopBtn == null || packageBtn == null || pauseBtn == null)
        {
            packageBtn = transform.Find("Package").GetComponent<Button>();
            shopBtn = transform.Find("Shop").GetComponent<Button>();
            pauseBtn = transform.Find("PauseButton").GetComponent<Button>();
        }
        shopBtn.enabled = false;
        packageBtn.enabled = false;
        pauseBtn.enabled = false;

    }

    public void PauseButton(UIPanelType type)
    {
        OnClick();
        foreach (Button temp in btnDic.Values)
        {
            temp.enabled = true;
        }

        Button btn;
        btnDic.TryGetValue(type, out btn);
        btn.enabled = false;
    }

    public override void OnResume()
    {
        shopBtn.enabled = true;
        CharacterBtn.enabled = true;
        packageBtn.enabled = true;
        pauseBtn.enabled = true;
        EventCenter.Broadcast(EventType.ResumeGame);
    }

    public void UpdateInterfaceInfo()
    {
        hasHeadPortrait = false;
        RoleData roledata = Model.Instance.GetRoleData();
        hpSlider.value = roledata.currentHP / (float)roledata.MaxHP;
        expSlider.value = roledata.exp / ((float) (roledata.Lv * 100));
        this.lv.text = roledata.Lv.ToString();
    }
}
