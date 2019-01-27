using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class EditorPanel : BasePanel
{
    private Difficulty difficulty;

    private Slider m_BG_Volume;

    private Slider m_Effectsound_Volume;

    private Button confirmButton;

    private Button closeButton;

    private Toggle[] btns = new Toggle[3];

    private void Start()
    {
        m_BG_Volume = transform.Find("BG_Volume").GetComponentInChildren<Slider>();
        m_Effectsound_Volume = transform.Find("Effectsound_Volume").GetComponentInChildren<Slider>();
        confirmButton = transform.Find("ConfirmButton").GetComponent<Button>();
        closeButton = transform.Find("CloseButton").GetComponent<Button>();
        confirmButton.onClick.AddListener(OnConfirmClick);
        closeButton.onClick.AddListener(OnCloseClick);
        btns = transform.GetComponentsInChildren<Toggle>();
        uiMng.editorPanel = this;
        Init();
    }

    private void OnConfirmClick()
    {
        foreach (Toggle temp in btns)
        {
            if (temp.isOn)
            {
                Difficulty d = (Difficulty)Enum.Parse(typeof(Difficulty), temp.gameObject.name);
                GameManager.Instance.MenuEdit(m_BG_Volume.value, m_Effectsound_Volume.value, d);
            }
        }

        uiMng.PopPanel();
    }

    #region Pre_Treatment

    private void OnCloseClick()
    {
        uiMng.PopPanel();
    }

    private void Init()
    {
        string s = GameManager.Instance.GetSetting();
        Debug.Log(s);
        if (m_BG_Volume == null)
        {
            Debug.Log("null");
        }
        m_BG_Volume.value = float.Parse(s.Split(',')[0]);
        m_Effectsound_Volume.value = float.Parse(s.Split(',')[1]);
        difficulty = (Difficulty)Enum.Parse(typeof(Difficulty), s.Split(',')[2]);
        foreach (Toggle temp in btns)
        {
            if (temp.gameObject.name == difficulty.ToString())
            {
                temp.isOn = true;
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
        this.gameObject.transform.localScale = Vector3.one;
        this.gameObject.SetActive(true);
        this.gameObject.transform.DOScale(1, 0.5f);
    }

    public void HideAnim()
    {
        this.gameObject.SetActive(false);

    }

    #endregion

    //public void SetEditor(float m_BG_Volume,float m_Effectsound_Volume,Difficulty difficulty)
    //{
    //    this.m_BG_Volume.value = m_BG_Volume;
    //    this.m_Effectsound_Volume.value = m_Effectsound_Volume;
    //    foreach (Toggle temp in btns)
    //    {
    //        if (temp.gameObject.name==difficulty.ToString())
    //        {
    //            temp.isOn = true;
    //        }
    //    }
    //}

}

