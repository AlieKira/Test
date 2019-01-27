using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordPanel : BasePanel
{

    private Button closeBtn;

    private Button easyBtn;

    private Button commonBtn;

    private Button difficultBtn;

    private Transform easyRecord;

    private Transform commonRecord;

    private Transform difficultRecord;

    private RecordItem[] recordItems = new RecordItem[3];

    private void Awake()
    {
        easyRecord = transform.Find("EasyRecord");
        commonRecord = transform.Find("CommonRecord");
        difficultRecord = transform.Find("DifficultRecord");
    }

    private void Start()
    {
        closeBtn = transform.Find("CloseButton").GetComponent<Button>();
        closeBtn.onClick.AddListener(OnCloseClick);
        
        recordItems = transform.GetComponentsInChildren<RecordItem>();
        commonRecord.gameObject.SetActive(false);
        difficultRecord.gameObject.SetActive(false);
    }

    /// <summary>
    /// 更新各榜单记录
    /// </summary>
    private void UpdateRecordItem()
    {
        string[] easyRecords = uiMng.view.GetEasyRecords();
        string[] commonRecords = uiMng.view.GetCommonRecords();
        string[] difficultRecords = uiMng.view.GetDifficultRecords();

        for (int i = 0; i < easyRecords.Length; i++)
        {
            Debug.Log(easyRecords[i]);
            RecordItem[] ri = easyRecord.GetComponentsInChildren<RecordItem>();
            ri[i].level.text = easyRecords[i].Split(',')[0];
            ri[i].time.text = easyRecords[i].Split(',')[1];
        }
        for (int i = 0; i < commonRecords.Length; i++)
        {
            RecordItem[] ri = commonRecord.GetComponentsInChildren<RecordItem>();
            ri[i].level.text = commonRecords[i].Split(',')[0];
            ri[i].time.text = commonRecords[i].Split(',')[1];
        }
        for (int i = 0; i < difficultRecords.Length; i++)
        {
            RecordItem[] ri = difficultRecord.GetComponentsInChildren<RecordItem>();
            ri[i].level.text = difficultRecords[i].Split(',')[0];
            ri[i].time.text = difficultRecords[i].Split(',')[1];
        }
        uiMng.view.GetEasyRecords();
    }


    private void OnCloseClick()
    {
        uiMng.PopPanel();
    }

    #region Pre_Treatment
    public override void OnEnter()
    {
        this.gameObject.SetActive(true);
        UpdateRecordItem();
    }

    public override void OnPause()
    {
        this.gameObject.SetActive(false);
    }

    public override void OnResume()
    {
        this.gameObject.SetActive(true);
        UpdateRecordItem();
    }

    public override void OnExit()
    {
        this.gameObject.SetActive(false);
    }

    #endregion
}
