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
        startButton.onClick.AddListener(delegate()
        {
            OnClick();
            EventCenter.Broadcast<UIPanelType>(EventType.PushPanel, UIPanelType.MenuPanel);

        });
    }

    #region Pre_Treatment

    public override void OnEnter()
    {
        this.gameObject.SetActive(true);
        EventCenter.Broadcast(EventType.PlayBGSound,Audios.Sound_Bg_Moderate);
    }

    public override void OnPause()
    {
        this.gameObject.SetActive(false);
    }

    public override void OnResume()
    {
        this.gameObject.SetActive(true);
        EventCenter.Broadcast(EventType.PlayBGSound, Audios.Sound_Bg_Moderate);
    }

    public override void OnExit()
    {
        this.gameObject.SetActive(false);
    }

    #endregion


}
