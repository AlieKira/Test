using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class MenuPanel :BasePanel{

    private Button StartButton;

    private Button EditorButton;

    private Button RecordButton;

    private Button ExitButton;

    private void Start()
    {
        StartButton = transform.Find("StartButton").GetComponent<Button>();
        EditorButton = transform.Find("EditorButton").GetComponent<Button>();
        RecordButton = transform.Find("RecordButton").GetComponent<Button>();
        ExitButton = transform.Find("ExitButton").GetComponent<Button>();

        StartButton.onClick.AddListener(OnStartClick);
        EditorButton.onClick.AddListener(OnEditClick);
        RecordButton.onClick.AddListener(OnRecordClick);
        ExitButton.onClick.AddListener(OnExitClick);
    }

    private void OnStartClick()
    {
        GameManager.Instance.StartGame();
    }
    
    private void OnEditClick()
    {
        uiMng.PushPanel(UIPanelType.EditorPanel);
    }

    private void OnRecordClick()
    {
        uiMng.PushPanel(UIPanelType.RecordPanel);
    }

    private void OnExitClick()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Debug.Log(1111);
    }

    #region Pre_Treatment

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

    #endregion

    

}

