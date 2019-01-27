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

    public Image headPoriait;

    private RoleType roleType;

    private void Start()
    {


        archerButton = transform.Find("Layout/Archer").GetComponent<Button>();
        barbarianButton = transform.Find("Layout/Barbarian").GetComponent<Button>();
        casualButton = transform.Find("Layout/Casual").GetComponent<Button>();
        knightButton = transform.Find("Layout/Knight").GetComponent<Button>();
        mageButton = transform.Find("Layout/Mage").GetComponent<Button>();

        archerButton.onClick.AddListener(OnArcherBtnClick);
        barbarianButton.onClick.AddListener(OnBarbarianBtnClick);
        casualButton.onClick.AddListener(OnCasualBtnClick);
        knightButton.onClick.AddListener(OnKnightBtnClick);
        mageButton.onClick.AddListener(OnMageBtnClick);
    }




    private void Hide()
    {
        uiMng.ShowTimer();
        uiMng.PopPanel();
        GameManager.Instance.Spawn(roleType);
        Invoke("StartPlay",3);
    }

    private void StartPlay()
    {
        GameManager.Instance.StartPlay();
    }

    #region OnBtnClick

    private void OnArcherBtnClick()
    {
        roleType = RoleType.Archer;
        Hide();
    }

    private void OnBarbarianBtnClick()
    {
        roleType = RoleType.Barbarian;
        Hide();
    }

    private void OnCasualBtnClick()
    {
        roleType = RoleType.Casual;
        Hide();
    }

    private void OnKnightBtnClick()
    {
        roleType = RoleType.Knight;
        Hide();
    }

    private void OnMageBtnClick()
    {
        roleType = RoleType.Mage;
        Hide();
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

