using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Common;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class RoomPanel:BasePanel
{
    public Room_PlayerItemData[] playerItemArray;
    private bool isHouseOwner = false;
    private bool isSetHouseOwner = false;
    private bool isSetHouseGuest = false;
    private string buffer;
    private List<Room_PlayerItemData> tempList;
    private Button startButton;
    private Button exitButton;
    private bool isPopPanel = false;
    private bool isInit = false;


    private ExitRoomRequest exitRoomRequest;
    private StartGameRequest startGameRequest;
    private PrepareGameRequest prepareGameRequest;
    //private int currentPlayerNumber;

    private void Start()
    {
        
        startButton = GameObject.Find("StartButton").GetComponent<Button>();
        exitButton = GameObject.Find("ExitButton").GetComponent<Button>();
        startButton.onClick.AddListener(OnStartClick);
        exitButton.onClick.AddListener(OnExitClick);
        exitRoomRequest = GetComponent<ExitRoomRequest>();
        prepareGameRequest = GetComponent<PrepareGameRequest>();
        startGameRequest = GetComponent<StartGameRequest>();
    }

    private void Update()
    {
        if (isSetHouseOwner==true)
        {
            SetHouseOwner();
            isSetHouseOwner = false;
        }

        if (isSetHouseGuest==true)
        {
            SetHouseGuest(buffer);
            isSetHouseGuest = false;
            buffer = null;
        }

        if (isPopPanel==true)
        {
            uiMng.PopPanel();
            isPopPanel = false;
        }

        if (isInit==true)
        {
            startButton.transform.Find("Text").GetComponent<Text>().text = "准备";
            isInit = false;
        }
        
    }

    public void SetHouseOwner()
    {
        isHouseOwner = true;
        foreach (Room_PlayerItemData temp in playerItemArray)
        {
            temp.Clear();
        }
        startButton.transform.Find("Text").GetComponent<Text>().text = "开始";
        UserData userData = gameFacade.GetUserData();
        playerItemArray[0].SetInfo(userData.userID, userData.username,
            userData.winCount, userData.totalCount, userData.headProtraitPath);
        playerItemArray[0].SetHouseOwnerState();
        //currentPlayerNumber = 1;
    }
    public void SetHouseGuest(string buffer)
    {
        foreach (Room_PlayerItemData temp in playerItemArray)
        {
            temp.Clear();
        }
        string[] strs = buffer.Split('|');
        for (int i = 0; i < strs.Length; i++)
        {
            string[] strs2 = strs[i].Split(',');
            playerItemArray[int.Parse(strs2[0])].SetInfo(int.Parse(strs2[1]), strs2[2], int.Parse(strs2[3]), int.Parse(strs2[4]), strs2[5]);
            //currentPlayerNumber = int.Parse(strs2[0]);
            if (int.Parse(strs2[0])==0)
            {
                playerItemArray[0].SetHouseOwnerState();
            }
            else if (int.Parse(strs2[6])==0)
            {
                playerItemArray[int.Parse(strs2[0])].SetPrepareState();
            }
            else
            {
                playerItemArray[int.Parse(strs2[0])].SetNoneState();
            }
        }

    }

    public void SetHouseOwnerSync()
    {
        isSetHouseOwner = true;
    }

    public void SetHouseGuestSync(string data)
    {
        buffer = data;
        isSetHouseGuest = true;
    }

    private void OnStartClick()
    {
        if (isHouseOwner==false)
        {
            prepareGameRequest.SendRequest();
        }
        else
        {
            
            startGameRequest.SendRequest();
        }
       
    }

    

    private void OnExitClick()
    {
        exitRoomRequest.SendRequest();
    }

    public void OnExitResponse(ReturnCode returnCode)
    {
        if (returnCode != ReturnCode.Success)
        {
            uiMng.ShowMessageSync("退出房间失败");
        }
        isHouseOwner = false;
        isInit = true;
        
        isPopPanel = true;
    }

    public void OnStartResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Fail)
        {
            uiMng.ShowMessageSync("您不是房主，无法开始游戏！！");
        }
        else if (returnCode==ReturnCode.NotFound)
        {
            uiMng.ShowMessageSync("有成员未准备，无法开始游戏");
        }
        else
        {
            uiMng.PushPanelSync(UIPanelType.GamePanel);
            //gameFacade.EnterPlayingSync();
        }
    }

    public override void OnEnter()
    {
        this.gameObject.SetActive(true);
        this.gameObject.transform.localScale=Vector3.zero;
        this.gameObject.transform.DOScale(1f, 0.3f);
    }

    public override void OnPause()
    {
        foreach (Room_PlayerItemData temp in playerItemArray)
        {
            temp.SetNoneState();
        }
        this.gameObject.transform.DOScale(0, 0.3f).OnComplete(() => this.gameObject.SetActive(false));
    }

    public override void OnExit()
    {
        foreach (Room_PlayerItemData temp in playerItemArray)
        {
            temp.SetNoneState();
        }
        this.gameObject.transform.DOScale(0, 0.3f).OnComplete(()=>this.gameObject.SetActive(false));
    }

    public void OnPrepareGameResponse(ReturnCode returnCode,int number)
    {
        if (returnCode!=ReturnCode.Success)
        {
            uiMng.ShowMessageSync("准备游戏发生错误");
        }
        else
        {
            playerItemArray[number].SetPrepareStateSync();
        }
    }

    public void OnStartPlayResponse(List<RoleData> roleDataList)
    {
        foreach (RoleData temp in roleDataList)
        {
            
        }
    }
         
}
