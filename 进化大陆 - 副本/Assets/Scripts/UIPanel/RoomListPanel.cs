using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RoomListPanel : BasePanel
{
    private RectTransform battleRecord;
    private RectTransform roomList;
    private Button creatRoomButton;
    private Button refreshButton;
    private Button closeButton;
    private CreateRoomRequest creatRoomRequest;
    private JoinRoomRequest joinRoomRequest;
    private VerticalLayoutGroup roomLayout;
    private GameObject roomItemPrefab;
    private List<RoomItemData> countList = null;
    private ListRoomRequest listRoomRequest;
    private RoomPanel roomPanel;
    private bool isPushOwnerPanel=false;
    private bool isPushGuestPanel = false;
    private string data;

    private void Start()
    {
        joinRoomRequest = GetComponent<JoinRoomRequest>();
        roomLayout = transform.Find("RoomList/ScrollRect/Layout").GetComponent<VerticalLayoutGroup>();
        battleRecord =GameObject.Find("BattleRecord").GetComponent<RectTransform>();
        roomList=GameObject.Find("RoomList").GetComponent<RectTransform>();
        closeButton = GameObject.Find("RoomList/CloseButton").GetComponent<Button>();
        creatRoomButton=GameObject.Find("RoomList/CreateRoomButton").GetComponent<Button>();
        refreshButton = GameObject.Find("RoomList/RefreshButton").GetComponent<Button>();
        creatRoomRequest = GetComponent<CreateRoomRequest>();
        closeButton.onClick.AddListener(OnCloseClick);
        creatRoomButton.onClick.AddListener(OnCreatRoomClick);
        refreshButton.onClick.AddListener(OnRefreshClick);
        roomItemPrefab = Resources.Load("UIPanel/RoomItem") as GameObject;
        listRoomRequest = GetComponent<ListRoomRequest>();
    }

    private void Update()
    {
        if (countList != null)
        {
            LoadRoomItem(countList);
            countList = null;
        }

        if (isPushOwnerPanel==true)//TODO 设置不需update执行
        {
            roomPanel = uiMng.PushPanel(UIPanelType.RoomPanel) as RoomPanel;
            uiMng.SetHouseOwnerSync(roomPanel);
            isPushOwnerPanel = false;
        }

        if (isPushGuestPanel==true)
        {
            roomPanel = uiMng.PushPanel(UIPanelType.RoomPanel) as RoomPanel;
            uiMng.SetHouseGuestSync(roomPanel,data);
            isPushGuestPanel = false;
        }

    }

    public override void OnEnter()
    {
        SetBattleRes();
        if (battleRecord==null)
        {
            battleRecord = GameObject.Find("BattleRecord").GetComponent<RectTransform>();
        }
        if (roomList == null)
        {
            roomList = GameObject.Find("RoomList").GetComponent<RectTransform>();
        }

        if (listRoomRequest==null)
        {
            listRoomRequest = GetComponent<ListRoomRequest>();
        }
        listRoomRequest.SendRequest();
        AppearAnim();
    }

    public override void OnPause()
    {
        HideAnim();
    }

    public override void OnResume()
    {
        AppearAnim();
        gameFacade.ShowMainCamera();
    }

    public override void OnExit()
    {
        HideAnim();
    }

    private void SetBattleRes()
    {
        UserData userData = gameFacade.GetUserData();
        transform.Find("BattleRecord/HeadPortrait").GetComponent<Image>().sprite =
            Resources.Load<Sprite>(userData.headProtraitPath);
        transform.Find("BattleRecord/Username").GetComponent<Text>().text = userData.username;
        transform.Find("BattleRecord/TotalCount").GetComponent<Text>().text = "总场数: " + userData.totalCount;
        transform.Find("BattleRecord/WinCount").GetComponent<Text>().text = "胜利: " + userData.winCount;
    }


    public void OnJoinClick(int userid)
    {
        joinRoomRequest.SendRequest(userid.ToString());
    }

    private void OnCloseClick()
    {
        uiMng.PopPanel();
    }

    private void OnCreatRoomClick()
    {
        creatRoomRequest.SendRequest("r");
    }



    private void OnRefreshClick()
    {
        listRoomRequest.SendRequest();
    }

    public void OnCreatRoomResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Fail)
        {
            gameFacade.ShowMessageSync("无法创建房间");
        }
        else
        {
            isPushOwnerPanel = true;
            gameFacade.IsHouseOwner();
        }
    }

    public void OnJoinRoomResponse(ReturnCode returnCode,string data)
    {
        if (returnCode == ReturnCode.NotFound)
        {
            gameFacade.ShowMessageSync("房间不存在");
        }
        else if (returnCode == ReturnCode.Fail)
        {
            gameFacade.ShowMessageSync("加入失败");
        }
        else
        {
 
            this.data = data;
            isPushGuestPanel = true;
        }
    }

    public void LoadRoomItemSync(List<RoomItemData> countList)
    {
        this.countList = countList;
    }

    private void LoadRoomItem(List<RoomItemData> countList)
    {
        RoomItem[] roomItemArray = roomLayout.GetComponentsInChildren<RoomItem>();
        foreach (RoomItem ri in roomItemArray)
        {
            ri.DestroySelf();
        }
        int count = countList.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject roomItem = GameObject.Instantiate(roomItemPrefab);
            roomItem.transform.SetParent(roomLayout.transform,false);
            RoomItemData rd = countList[i];
            roomItem.GetComponent<RoomItem>().SetRoomInfo(rd.Userid, rd.Username, rd.ClientCount,this);
        }
        int roomCount = GetComponentsInChildren<RoomItem>().Length;
        Vector2 size = roomLayout.GetComponent<RectTransform>().sizeDelta;
        roomLayout.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x,
            roomCount * (roomItemPrefab.GetComponent<RectTransform>().sizeDelta.y + roomLayout.spacing));
    }
    private void AppearAnim()
    {
        this.gameObject.SetActive(true);
        battleRecord.localPosition=new Vector3(-1000,0,0);
        battleRecord.DOLocalMoveX(-340, 0.5f);
        roomList.localPosition=new Vector3(1200,0,0);
        roomList.DOLocalMoveX(128, 0.5f);
    }

    private void HideAnim()
    {
        battleRecord.DOLocalMoveX(-1000, 0.5f);
        roomList.DOLocalMoveX(1100, 0.5f).OnComplete(() => this.gameObject.SetActive(false));
    }
}
