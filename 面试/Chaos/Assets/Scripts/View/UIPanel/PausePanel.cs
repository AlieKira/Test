using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : BasePanel
{
    private Text level;
    private Text time;
    private Button resumeBtn;
    private Button menuBtn;

    private void Start()
    {
        level = transform.Find("Level").GetComponent<Text>();
        time = transform.Find("Time").GetComponent<Text>();
        resumeBtn = transform.Find("ResumeButton").GetComponent<Button>();
        menuBtn = transform.Find("MenuButton").GetComponent<Button>();
        resumeBtn.onClick.AddListener(OnResumeClick);
        menuBtn.onClick.AddListener(OnMenuClick);
    }

    private void OnResumeClick()
    {
        OnClick();
        uiMng.PopPanel();
    }

    private void OnMenuClick()
    {
        uiMng.PopPanel();
        uiMng.PopPanel();
        uiMng.PopPanel();
        EventCenter.Broadcast(EventType.ReturnMenuPanel);

    }

    public override void OnEnter()
    {
        this.gameObject.SetActive(true);
        int[] ints=Model.Instance.GetLevelAndTime();
        int minute = ints[1] / 60;
        int second = ints[1] % 60;
        string gameTimeString = minute.ToString("00") + ":" + second.ToString("00");
        if (level==null)
        {
            level = transform.Find("Level").GetComponent<Text>();
        }
        if (time == null)
        {
            time = transform.Find("Time").GetComponent<Text>();
        }
        level.text = string.Format("第{0}关", ints[0]);
        time.text = string.Format("用时：{0}", gameTimeString);
        EventCenter.Broadcast(EventType.PauseGame);
    }

    public override void OnResume()
    {
        base.OnResume();
        EventCenter.Broadcast(EventType.PauseGame);
    }

    public override void OnExit()
    {
        this.gameObject.SetActive(false);
    }

    //private void AppearAnim()
    //{
    //    GetComponent<CanvasGroup>().DOFade(1, 0.3f);
    //    this.transform.DOLocalMove(Vector3.zero, 0.3f);
    //}

    //private void HideAnim()
    //{
    //            GetComponent<CanvasGroup>().DOFade(1, 0.3f);
    //    this.transform.DOLocalMove(Vector3.zero, 0.3f);
    //}
}
