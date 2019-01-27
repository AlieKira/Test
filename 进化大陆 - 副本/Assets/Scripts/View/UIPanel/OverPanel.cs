using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class OverPanel:BasePanel
{
    private Image mask;

    private Text levelRecord;

    private Text timeRecord;

    private Button closeBtn;

    private GameObject NewRecord;

    private void Awake()
    {
        mask = transform.Find("Mask").GetComponent<Image>();
        levelRecord = transform.Find("Level").GetComponent<Text>();
        timeRecord = transform.Find("Time").GetComponent<Text>();
        NewRecord = transform.Find("NewRecord").gameObject;
        closeBtn = transform.Find("CloseButton").GetComponent<Button>();
        closeBtn.onClick.AddListener(OnCloseClick);
    }

    private void OnCloseClick()
    {
        uiMng.PopPanel();
        uiMng.PopPanel();
    }

    public void ShowRecod(string level,string time,int number)
    {
        levelRecord.text = "第"+level+"关";
        timeRecord.text = "用时："+time;
        if (number!=0)
        {
            NewRecord.SetActive(true);
            NewRecord.GetComponent<Text>().text =string.Format("新纪录！     \n     第{0}名！", number);
        }
    }

    public override void OnEnter()
    {
        this.gameObject.SetActive(true);
        mask.DOFade(0.5f, 1);
    }

    public override void OnExit()
    {
        Color a = mask.color;
        a.a = 0;
        NewRecord.SetActive(false);
        this.gameObject.SetActive(false);
        GameManager.Instance.ReturnMenuPanel();
    }

}
