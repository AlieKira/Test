using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;


public class MonsterMoveRequest : BaseRequest
{
    //private PlayerMove localPlayerMove;
    private int syncRate = 4;

    //private Transform remotePlayerTransform;
    //private Animator remotePlayerAnim;
    private string posBuffer;  //其他玩家的位置信息

    private bool isSyncPos = false;
    private Vector3 pos;
    private Vector3 rotation;

    public List<Transform> monsterList=new List<Transform>();
    //private float forward;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.MonsterMove;
        base.Awake();
    }
    private void Start() //初始化
    {
        foreach (MonsterData temp in gameFacade.GetMonsterList())
        {
            monsterList.Add(temp.gameObject.transform);
        }
    }

    public void StartSync()  //启动位置同步
    {
        InvokeRepeating("SyncPos", 1f, 1f / syncRate);
    }


    private void FixedUpdate()
    {
        if (isSyncPos)
        {
            gameFacade.SyncMonsterPos(posBuffer);
            // SyncRemotePlayer();
            isSyncPos = false;
        }
        

    }
    //public MonsterMoveRequest SetLocalPlayer(Transform localPlayerTransform)
    //{
    //    this.localPlayerTransform = localPlayerTransform;
    //    // this.localPlayerMove = localPlayerMove;
    //    return this;
    //}
    //public MoveRequest SetRemotePlayer(Transform remotePlayerTransform)
    //{
    //    this.remotePlayerTransform = remotePlayerTransform;
    //    this.remotePlayerAnim = remotePlayerTransform.GetComponent<Animator>();
    //    return this;
    //}
    private void SyncPos()  //向服务器发送本怪物的位置信息
    {
        StringBuilder sb=new StringBuilder();
        foreach (Transform temp in monsterList)
        {
            string data = string.Format("{6}|{0},{1},{2}|{3},{4},{5}", temp.position.x, temp.position.y, temp.position.z,
                temp.eulerAngles.x, temp.eulerAngles.y, temp.eulerAngles.z,
                temp.GetComponent<MonsterData>().number.ToString());
            sb.Append(data + "^");

        }

        sb.Remove(sb.Length - 1, 1); 
        SendRequest(sb.ToString());

    }
    //private void SyncRemotePlayer()  //从服务器接受其他客户端角色的位置信息
    //{

    //    remotePlayerTransform.position = pos;
    //    remotePlayerTransform.eulerAngles = rotation;
    //    //remotePlayerAnim.SetFloat("Forward", forward);
    //}

    private void SendRequest(string data)
    {
        base.SendRequest(data);
    }
    public override void OnResponse(string data)  //接收到怪物的位置信息
    {//27.75,0,1.41-0,0,0-0
        //print(data);
        //string[] strs = data.Split('|');
        //pos = UnityTools.ParseVector3(strs[0]);
        //rotation = UnityTools.ParseVector3(strs[1]);
        ////forward = float.Parse(strs[2]);
        posBuffer = data;
        isSyncPos = true;

    }
}

