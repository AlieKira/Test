using System.Collections;
using System.Collections.Generic;
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
        uiMng.PopPanel();
        GameManager.Instance.ResumeGame();
    }

    private void OnMenuClick()
    {
        uiMng.PopPanel();
        uiMng.PopPanel();
        GameManager.Instance.ReturnMenuPanel();
        
    }

    public override void OnEnter()
    {
        this.gameObject.SetActive(true);
        GameManager.Instance.PauseGame();
        int[] ints=GameManager.Instance.GetLevelAndTime();
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
    }

    public override void OnExit()
    {
        this.gameObject.SetActive(false);
    }
}
