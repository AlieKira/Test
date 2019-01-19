using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;

public class MoveRequest : BaseRequest
{
    private Transform localPlayerTransform;
    //private PlayerMove localPlayerMove;
    private int syncRate = 10;

    private Transform remotePlayerTransform;
    private Animator remotePlayerAnim;
    private string posBuffer;  //其他玩家的位置信息

    private bool isSyncRemotePlayer = false;
    private Vector3 pos;
    private Vector3 rotation;
    private float forward;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Move;
        base.Awake();
    }
    private void Start()
    {
        InvokeRepeating("SyncLocalPlayer", 1f, 1f / syncRate);
    }
    private void FixedUpdate()
    {
        if (isSyncRemotePlayer)
        {
            gameFacade.SyncRemoteRolePos(posBuffer);
           // SyncRemotePlayer();
            isSyncRemotePlayer = false;
        }


    }
    public MoveRequest SetLocalPlayer(Transform localPlayerTransform)
    {
        this.localPlayerTransform = localPlayerTransform;
       // this.localPlayerMove = localPlayerMove;
        return this;
    }
    public MoveRequest SetRemotePlayer(Transform remotePlayerTransform)
    {
        this.remotePlayerTransform = remotePlayerTransform;
        this.remotePlayerAnim = remotePlayerTransform.GetComponent<Animator>();
        return this;
    }
    private void SyncLocalPlayer()  //向服务器发送本地角色的位置信息
    {
        SendRequest(localPlayerTransform.position.x, localPlayerTransform.position.y, localPlayerTransform.position.z,
            localPlayerTransform.eulerAngles.x, localPlayerTransform.eulerAngles.y, localPlayerTransform.eulerAngles.z);
    }
    private void SyncRemotePlayer()  //从服务器接受其他客户端角色的位置信息
    {
        
        remotePlayerTransform.position = pos;
        remotePlayerTransform.eulerAngles = rotation;
        //remotePlayerAnim.SetFloat("Forward", forward);
    }

    private void SendRequest(float x, float y, float z, float rotationX, float rotationY, float rotationZ)
    {
        string data = string.Format("{6}|{0},{1},{2}|{3},{4},{5}", x, y, z, rotationX, rotationY, rotationZ,
            localPlayerTransform.GetComponent<RoleData>().UserID);
        base.SendRequest(data);
    }
    public override void OnResponse(string data)  //接收到其他客户端角色的位置信息
    {//27.75,0,1.41-0,0,0-0
        //print(data);
        //string[] strs = data.Split('|');
        //pos = UnityTools.ParseVector3(strs[0]);
        //rotation = UnityTools.ParseVector3(strs[1]);
        ////forward = float.Parse(strs[2]);
        posBuffer = data;
        isSyncRemotePlayer = true;
    }
}
