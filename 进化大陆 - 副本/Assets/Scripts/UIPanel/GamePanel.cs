using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class GamePanel : BasePanel
{
    private Text timer;
    private Text careerTimer;
    private int careerTime=-1;
    private int time = -1;
    private Button successBtn;
    private Button failBtn;
    private Button exitBtn;
    private bool isChooseCareer;
    private bool isShowCareerPanel = false;
    private SpawnMonsterRequest spawnMonsterRequest;
    private GameObject rolePrefab;
    private Image headPoriait;
    private Transform skill_01;
    private Transform skill_02;
    private Transform skill_03;
    private RoleType roleType;
    private GameObject careerPanel;


    public GameObject[] toolTips;

    private void Awake()
    {

    }

    private void Start()
    {
        spawnMonsterRequest = GetComponent<SpawnMonsterRequest>();
        careerPanel =GameObject.Find("CareerPanel");
        careerPanel.SetActive(false);
        timer = transform.Find("Timer").GetComponent<Text>();
        timer.gameObject.SetActive(false);
        careerTimer = transform.Find("CareerTimer").GetComponent<Text>();
       careerTimer.gameObject.SetActive(false);
        successBtn = transform.Find("SuccessButton").GetComponent<Button>();
        successBtn.onClick.AddListener(OnResultClick);
        successBtn.gameObject.SetActive(false);
        failBtn = transform.Find("FailButton").GetComponent<Button>();
        failBtn.onClick.AddListener(OnResultClick);
        failBtn.gameObject.SetActive(false);
        toolTips = GameObject.FindGameObjectsWithTag("ToolTip");
        headPoriait = transform.Find("Interface/RoleInfo/HeadPortrait/Image").GetComponent<Image>();
        skill_01 = transform.Find("Interface/Skills/Skill_01");
        skill_02 = transform.Find("Interface/Skills/Skill_02");
        skill_03 = transform.Find("Interface/Skills/Skill_03");
        foreach (GameObject temp in toolTips)
        {
            ToolTipItem x = temp.GetComponent<ToolTipItem>();
            x.uiMng = this.uiMng;
            x.gameFacade = this.gameFacade;
        }

        
        //exitBtn = transform.Find("ExitButton").GetComponent<Button>();
        //exitBtn.onClick.AddListener(OnExitClick);
        // exitBtn.gameObject.SetActive(false);


    }

    private void SpawnMonster()
    {
        if (gameFacade.GetUserData().isHouseOwner)
        {
            spawnMonsterRequest.SendRequest();
        }
    }

    public override void OnEnter()
    {
        Invoke("SpawnMonster", 1f);
        gameObject.SetActive(true);
    }
    public override void OnExit()
    {
        successBtn.gameObject.SetActive(false);
        failBtn.gameObject.SetActive(false);
        //exitBtn.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (time > -1)
        {
            ShowTime(time);
            time = -1;
        }

        if (isChooseCareer==true)
        {
            ParseRoleData();
            careerPanel.SetActive(false);
            isChooseCareer = false;

        }

        if (careerTime >-1)
        {
            ShowCareerTime(careerTime);
            if (careerTime == 0)
            {
                careerTimer.gameObject.SetActive(false);
                Invoke("HideCareerPanel", 1);
            }
            careerTime = -1;
        }

        //if (isShowCareerPanel==true)
        //{
        //    careerPanel.SetActive(true);
        //    isShowCareerPanel = false;
        //}
    }



    public void ShowCareerTimeSync(int time)
    {
        this.careerTime = time;
        //isShowCareerPanel = true;
        //transform.GetComponent<SpawnMonsterRequest>().SendRequest();
    }

    public void ShowCareerTime(int time)
    {
        careerTimer.gameObject.SetActive(true);
        careerTimer.text = time.ToString();
    }

    //public void HideCareerPanelSync()
    //{
    //    careerPanel.SetActive(true);
    //}

    public void HideCareerPanel()
    {
        careerPanel.SetActive(false);
    }

    public void ShowCareerPanel()
    {
        careerPanel.SetActive(true);

    }

    public void ShowTimeSync(int time)
    {
        this.time = time;
    }
    public void ShowTime(int time)
    {

        timer.gameObject.SetActive(true);
        timer.text = time.ToString();
        timer.transform.localScale = Vector3.one;
        Color tempColor = timer.color;
        tempColor.a = 1;
        timer.color = tempColor;
        timer.transform.DOScale(2, 0.3f).SetDelay(0.3f);
        timer.DOFade(0, 0.3f).SetDelay(0.3f).OnComplete(() => timer.gameObject.SetActive(false));
        if (time == 1)
        {
            Invoke("ShowCareerPanel", 1);
        }


        //gameFacade.PlayNormalSound(AudioManager.Sound_Alert);
    }

    private void OnResultClick()
    {
        uiMng.PopPanel();
        uiMng.PopPanel();
        gameFacade.GameOver();
    }

    public void OnChooseCareer(RoleType roleType)
    {
        this.roleType = roleType;
        isChooseCareer = true;
    }

    private void ParseRoleData()
    {
        RoleTypeData roleTypeData=uiMng.GetRoleTypeData(roleType);
        if(headPoriait==null) headPoriait = transform.Find("Interface/RoleInfo/HeadPortrait/Image").GetComponent<Image>();
        headPoriait.sprite = Resources.Load<Sprite>(roleTypeData.ImagePath);
        skill_01.GetComponent<Image>().sprite = Resources.Load<Sprite>(roleTypeData.Skill_01ImagePath);
        skill_02.GetComponent<Image>().sprite = Resources.Load<Sprite>(roleTypeData.Skill_02ImagePath);
        skill_03.GetComponent<Image>().sprite = Resources.Load<Sprite>(roleTypeData.Skill_03ImagePath);
        skill_01.GetComponent<ToolTipItem>().SetMessage(roleTypeData.Skill_01Message);
        skill_02.GetComponent<ToolTipItem>().SetMessage(roleTypeData.Skill_02Message);
        skill_03.GetComponent<ToolTipItem>().SetMessage(roleTypeData.Skill_03Message);
    }

    public void OnGameOverResponse(ReturnCode returnCode)
    {
        Button tempBtn = null;
        switch (returnCode)
        {
            case ReturnCode.Success:
                tempBtn = successBtn;
                break;
            case ReturnCode.Fail:
                tempBtn = failBtn;
                break;
        }
        tempBtn.gameObject.SetActive(true);
        tempBtn.transform.localScale = Vector3.zero;
        tempBtn.transform.DOScale(1, 0.5f);
    }

}
