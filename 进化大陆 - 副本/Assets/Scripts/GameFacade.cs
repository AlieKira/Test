using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameFacade : MonoBehaviour
{

    private static GameFacade _instance;

    public static GameFacade Instance
    {
        get
        {
            if (_instance==null)
            {
                _instance=GameObject.Find("GameFacade").GetComponent<GameFacade>();
            }
            return _instance;
        }
    }

    private UIManager uiMng;
    private AudioManager audioMng;
    private CameraManager cameraMng;
    private PlayerManager playerMng;
    private RequestManager requestMng;
    private ClientManager clientMng;
    public NetworkManager networkMng;

    public List<MonsterData> list;

	void Awake ()
	{
	    InitManager();
    }

    private void InitManager()
    {
        uiMng=new UIManager(this);
        audioMng=new AudioManager(this);
        cameraMng=new CameraManager(this);
        playerMng=new PlayerManager(this);
        if(requestMng==null) requestMng=new RequestManager(this);
        clientMng =new ClientManager(this);
        uiMng.OnInit();
        audioMng.OnInit();
        cameraMng.OnInit();
        playerMng.OnInit();
        playerMng.networkManager =networkMng;
        requestMng.OnInit();
        clientMng.OnInit();
    }
	// Update is called once per frame
	void Update () {
		UpdateManager();
	}

    private void UpdateManager()
    {
        uiMng.OnUpdate();
        audioMng.OnUpdate();
        cameraMng.OnUpdate();
        playerMng.OnUpdate();
        requestMng.OnUpdate();
        clientMng.OnUpdate();
    }
    public void HandleResponse(ActionCode actionCode,string data)
    {
        requestMng.HandleResponse(actionCode,data);
    }

    public void SendRequest(RequestCode requestCode,ActionCode actionCode,string data)
    {
        clientMng.SendRequest(requestCode,actionCode,data);
    }

    public void AddRequest(ActionCode actionCode, BaseRequest request)
    {
        if (requestMng == null) requestMng = new RequestManager(this);
        requestMng.AddRequest(actionCode, request);
        
    }

    public void SetUserData(UserData userData)
    {
        playerMng.userData = userData;
    }

    public UserData GetUserData()
    {
        return playerMng.userData;
    }

    public string GetHeadPortraitPath(HeadPortraitType type)
    {
        return uiMng.GetHeadPortraitPath(type);
    }

    public void SetHeadPortraitPath(string path)
    {
        playerMng.userData.headProtraitPath=path;
    }
    public void ShowMessage(string data)
    {
        uiMng.ShowMessage(data);
    }

    public void ShowMessageSync(string data)
    {
        uiMng.ShowMessageSync(data);
    }

    public void SetHouseOwner(RoomPanel panel)
    {
        uiMng.SetHouseOwnerSync(panel);
    }

    public void SetHouseGuest(RoomPanel panel,string data)
    {
        uiMng.SetHouseGuestSync(panel,data);
    }

    public RoleTypeData GetRoleTypeData(RoleType roleType)
    {
        return uiMng.GetRoleTypeData(roleType);
    }

    public void OnStartPlayResponse(string data)
    {
        playerMng.OnStartPlayResponse(data);
    }

    public GameObject GetCurrentRole()
    {
        return playerMng.currentRole;
    }

    public void ShowMainCamera() {
        cameraMng.ShowMainCamera();
    }


    public void FollowRole()
    {
        cameraMng.FollowRole();
    }

    public void StopFollowRole()
    {
        cameraMng.StopFollowRole();
    }

    public void SyncRemoteRolePos(string data)
    {
        playerMng.SyncRemoteRolePos(data);
    }

    public void SyncMonsterPos(string data)
    {
        playerMng.SyncMonsterPos(data);
    }

    public void StartPlaying()
    {
        playerMng.CreateSyncRequest();
        
    }

    public void SpawnMonster(string data)
    {
        playerMng.SpawnMonsterSync(data);
    }

    public void MonsterAttack(int targetID)
    {
        
    }

    public List<MonsterData> GetMonsterList()
    {
        list = playerMng.monsterDataList;
        return playerMng.monsterDataList;
    }

    public void LevelUp(string data)
    {
        playerMng.LevelUpSync(data);
    }

    public List<Transform> GetRoleList()
    {
        
        List<Transform> temp = playerMng.remoteRoleList;
        temp.Add(playerMng.currentRole.transform);
        return temp;
    }

    public void IsHouseOwner()
    {
        playerMng.userData.IsHouseOwner();
    }

    public void GameOver()
    {
        StopFollowRole();
        playerMng.GameOver();
    }
}
