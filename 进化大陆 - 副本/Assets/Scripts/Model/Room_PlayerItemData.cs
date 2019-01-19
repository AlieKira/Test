using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class Room_PlayerItemData : MonoBehaviour
{
    public Image headPortrait;
    private Text username;
    private Text winCount;
    private Text totalCount;
    private Text state;
    private int userid;
    public int itemNumber;
    private int isPrepareState=1;
    private bool isSetPrepareState = false;

    private void Awake()
    {
        headPortrait = transform.Find("HeadPortrait").GetComponent<Image>();
        username = transform.Find("PlayerInfo/Username").GetComponent<Text>();
        winCount = transform.Find("PlayerInfo/WinCount").GetComponent<Text>();
        totalCount = transform.Find("PlayerInfo/TotalCount").GetComponent<Text>();
        state = transform.Find("State").GetComponent<Text>();
     isPrepareState = 1;
}

    private void Update()
    {
        if (isSetPrepareState==true)
        {
            if (isPrepareState==1)
            {
                SetPrepareState();
                isPrepareState = 0;
            }
            else
            {
                SetNoneState();
                isPrepareState = 1;
            }
            isSetPrepareState = false;
        }
    }

    public Room_PlayerItemData(int itemNumber, int userid, string path, string username, int winCount, int totalCount)
    {
        this.itemNumber = itemNumber;
        this.userid = userid;
        this.username.text = username;
        this.headPortrait.sprite = Resources.Load<Sprite>(path);
        this.winCount.text = "胜" + winCount.ToString();
        this.totalCount.text = "总" + totalCount.ToString();
    }

    public void SetInfo(int userid,string username,int winCount,int totalCount, string path)
    {
        this.userid = userid;
        this.username.text = username;
        this.headPortrait.sprite = Resources.Load<Sprite>(path);
        this.winCount.text = "胜"+winCount.ToString();
        this.totalCount.text = "总"+totalCount.ToString();
    }

    public void SetPrepareState()
    {
        this.state.text = "准备";
    }

    public void SetPrepareStateSync()
    {
        isSetPrepareState = true;
    }

    public void SetNoneState()
    {
        this.state.text = "";
    }

    public void SetHouseOwnerState()
    {
        this.state.text = "房主";
    }

    public void Clear()
    {
        this.itemNumber = 0;
        this.userid = 0;
        this.username.text = "";
        this.headPortrait.sprite = Resources.Load<Sprite>("半透明背景");
        this.winCount.text = ""; ;
        this.totalCount.text = ""; 
    }
    
}

