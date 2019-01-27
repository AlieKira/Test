using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OriginalPanel : BasePanel
{
    private Button startButton;

    private void Start()
    {
        startButton = GameObject.Find("EnterButton").GetComponent<Button>();
        startButton.onClick.AddListener(OnEnterClick);
    }

    private void OnEnterClick()
    {
        uiMng.PushPanel(UIPanelType.MenuPanel);
    }

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
