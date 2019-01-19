using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Vector3 = UnityEngine.Vector3;

public class PlayerManager : BaseManager {
    public PlayerManager(GameFacade gameFacade) : base(gameFacade)
    {
    }
    public List<Transform> remoteRoleList=new List<Transform>();
    public GameObject currentRole;
    public UserData userData;
    public NetworkManager networkManager;
    private bool isSpawnRole=false;
    private bool isSpawnMonster = false;
    private string roleBuffer;
    private string monsterBuffer;
    private string levelUpBuffer;
    private bool isLevelUp = false;

    private GameObject playerSyncRequest;
    public List<MonsterData> monsterDataList=new List<MonsterData>();
    private Dictionary<MonsterType, string> monsterPathDic;

    public override void OnInit()
    {
        GameObject[] terrains = GameObject.FindGameObjectsWithTag("Terrain");
        foreach (GameObject temp in terrains)
        {
           // temp.transform.GetChild(0).tag = "Terrain"; //TODO
        }

        ParseMonsterTypeJson();
    }

    public override void OnUpdate()
    {
        if (isSpawnRole==true)
        {
            SpawnRoles(roleBuffer);
            isSpawnRole = false;
            roleBuffer = null;
        }

        if (isSpawnMonster==true)
        {
            SpawnMonster(monsterBuffer);
            isSpawnMonster = false;
            monsterBuffer = null;
        }

        if (isLevelUp==true)
        {
            LevelUp(levelUpBuffer);
            isLevelUp = false;
        }
    }

    public void SpawnRoleSync(string data)
    {
        isSpawnRole = true;
        roleBuffer = data;
    }

    private void SpawnRoles(string data)
    {
        string[] strs=data.Split('|');
        foreach (string d in strs)
        {
            string[] strs1 = d.Split('-');
            RoleTypeData roleTypeData = gameFacade.GetRoleTypeData((RoleType) int.Parse(strs1[0]));
            string[] positions = strs1[1].Split(',');
            Vector3 position=new Vector3();
            position.x = float.Parse(positions[0]);
            position.y = float.Parse(positions[1]);
            position.z = float.Parse(positions[2]);
            GameObject prefab = Resources.Load<GameObject>(roleTypeData.PrefabPath);
            GameObject mPrefab= GameObject.Instantiate(prefab, position, Quaternion.identity);
            mPrefab.GetComponent<RoleData>().SetRoleData(int.Parse(strs1[3]), (RoleType)int.Parse(strs1[0]));
            mPrefab.GetComponent<RoleData>().SetAttri(int.Parse(strs1[2]));
            if (userData.userID==int.Parse(strs1[3]))
            {
                mPrefab.GetComponent<RoleData>().isLocal=true;
                networkManager.playerPrefab = mPrefab;
                currentRole = mPrefab;
            }
            else
            {
                mPrefab.gameObject.tag = "Enemy";
                remoteRoleList.Add(mPrefab.transform); //将其他客户端添加到列表
            }
        }
        gameFacade.FollowRole();
    }

    public void OnStartPlayResponse(string data)
    {
        SpawnRoleSync(data);
    }

    public void CreateSyncRequest()
    {
        playerSyncRequest = new GameObject("PlayerSyncRequest");
        playerSyncRequest.AddComponent<MoveRequest>().SetLocalPlayer(currentRole.transform);
        playerSyncRequest.AddComponent<MonsterMoveRequest>();
        if (userData.isHouseOwner==true)
        {
            playerSyncRequest.GetComponent<MonsterMoveRequest>().StartSync();
        }
        //shootRequest = playerSyncRequest.AddComponent<ShootRequest>();
        //shootRequest.playerMng = this;
        //attackRequest = playerSyncRequest.AddComponent<AttackRequest>();
    }

    public void SyncRemoteRolePos(string data)   //同步其他玩家的位置信息
    {
        string[] strs= data.Split('|');
        int id = int.Parse(strs[0]);

            Vector3 pos = UnityTools.ParseVector3(strs[1]);
            Vector3 rotation = UnityTools.ParseVector3(strs[2]);
            foreach (Transform temp in remoteRoleList)
            {
                if (temp.GetComponent<RoleData>().UserID==id)
                {
                    temp.position = pos;
                    temp.eulerAngles = rotation;
                }
            } 
    }

    public void SyncMonsterPos(string data)   
    {
        string[] strs = data.Split('^');            
        foreach (string temp in strs)
        {
            string[] strs2 = temp.Split('|');
            foreach (MonsterData monsterData in monsterDataList)
            {
                if (monsterData.number==int.Parse(strs2[0]))   //判断是否该编号怪物位置信息，若是则同步
                {
                    monsterData.gameObject.transform.position = UnityTools.ParseVector3(strs2[1]);
                    monsterData.gameObject.transform.eulerAngles = UnityTools.ParseVector3(strs2[2]);
                }
            }
        }


        //string[] strs = data.Split('|');
        //int id = int.Parse(strs[0]);

        //Vector3 pos = UnityTools.ParseVector3(strs[1]);
        //Vector3 rotation = UnityTools.ParseVector3(strs[2]);
        //foreach (MonsterData temp in monsterDataList)
        //{
        //    if (temp.number == id)
        //    {
        //        temp.gameObject.transform.position = pos;
        //        temp.gameObject.transform.eulerAngles = rotation;
        //    }
        //}
    }

    public void SpawnMonsterSync(string data)
    {
        isSpawnMonster = true;
        monsterBuffer = data;
    }

    public void SpawnMonster(string data)
    {
        string[] strs = data.Split('|');
        foreach (string temp in strs)
        {
            string[] strs1 = temp.Split('`');
            Vector3 pos = UnityTools.ParseVector3(strs1[2]);
            MonsterType type = (MonsterType) int.Parse(strs1[1]);
            GameObject GO = Resources.Load<GameObject>(GetMonsterPath(type));
            GameObject monster = GameObject.Instantiate(GO, pos, Quaternion.identity);
            monster.GetComponent<MonsterData>().SetAttri(int.Parse(strs1[0]), int.Parse(strs1[3]), int.Parse(strs1[4]));
            monsterDataList.Add(monster.transform.GetComponent<MonsterData>());
            if (userData.isHouseOwner == true)
            {
                monster.transform.Find("Rig").gameObject.SetActive(true);
            }
        }
    }

    public string GetMonsterPath(MonsterType type)
    {
        string temp;
        monsterPathDic.TryGetValue(type, out temp);
        return temp;
    }

    public void LevelUpSync(string data)
    {
        levelUpBuffer = data;
        isLevelUp = true;
    }

    public void LevelUp(string data)
    {
        string[] strs = data.Split(',');
        if (currentRole.transform.GetComponent<RoleData>().UserID==int.Parse(strs[0]))
        {
            currentRole.transform.GetComponent<RoleData>().SetLevel(int.Parse(strs[1]));
            currentRole.transform.Find("Canvas/LevelUpText").GetComponent<AfterLevelUp>().LevelUp();
        }
        else
        {
            foreach (Transform temp in remoteRoleList)
            {
                if (temp.GetComponent<RoleData>().UserID== int.Parse(strs[0]) )
                {
                    temp.GetComponent<RoleData>().SetLevel(int.Parse(strs[1]));
                    temp.Find("Canvas/LevelUpText").GetComponent<AfterLevelUp>().LevelUp();
                }
            }
        }
    }

    [Serializable]
    class MonsterTypeJson
    {
        public List<MonsterInfo> infoList;
    }

    private void ParseMonsterTypeJson()
    {
        monsterPathDic = new Dictionary<MonsterType, string>();
        string s = File.ReadAllText(@"E:\UnityDocuments/进化大陆/Assets/Resources/MonsterType.json", Encoding.GetEncoding("utf-8"));
        MonsterTypeJson t = JsonConvert.DeserializeObject<MonsterTypeJson>(s);
        foreach (MonsterInfo info in t.infoList)
        {
            MonsterType temp = (MonsterType)Enum.Parse(typeof(MonsterType), info.monsterTypeString);
            monsterPathDic.Add(temp, info.prefabPath);
        }
    }

    public void GameOver()
    {
        //private GameObject currentRoleGameObject;
        //private GameObject playerSyncRequest;
        //private GameObject remoteRoleGameObject;

        //private ShootRequest shootRequest;
        //private AttackRequest attackRequest;

        for (int i = 0; i < remoteRoleList.Count; i++)
        {
            
            GameObject.Destroy(remoteRoleList[i].gameObject);
        }
        remoteRoleList=new List<Transform>();
        for (int i = 0; i < monsterDataList.Count; i++)
        {

            GameObject.Destroy(monsterDataList[i].gameObject);
        }
        monsterDataList=new List<MonsterData>();
        //foreach (Transform temp in remoteRoleList)
        //{
        //    //remoteRoleList.Remove(temp);
        //    GameObject.Destroy(temp.gameObject);
        //}
        //foreach (MonsterData temp in monsterDataList)
        //{
        //   // monsterDataList.Remove(temp);
        //    GameObject.Destroy(temp.gameObject);

        //}
        GameObject.Destroy(currentRole);
        GameObject.Destroy(playerSyncRequest);
    }
}

