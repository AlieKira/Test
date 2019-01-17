using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Common;
using UnityEngine;

public class ClientManager : BaseManager
{
    private string ip = "192.168.0.105";
    private int port = 8888;
    private Socket clientSocket;
    private Message msg=new Message();

    public ClientManager(GameFacade gameFacade) : base(gameFacade) { }

    public override void OnInit()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            clientSocket.Connect(IPAddress.Parse(ip),port);
            Start();
        }
        catch (Exception e)
        {
            Debug.Log("无法连接服务器"+e);
        }
    }

    private void Start()
    {
        clientSocket.BeginReceive(msg.Buffer,msg.StartIndex,msg.RemainCount,SocketFlags.None,ReceiveCallBack,null);
    }

    private void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            if (clientSocket==null||clientSocket.Connected==false)return;
            int count = clientSocket.EndReceive(ar);
            msg.ReadMessage(count, OnProcessCallBack);
            Start();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        
    }

    private void OnProcessCallBack(ActionCode actionCode,string data)
    {
        gameFacade.HandleResponse(actionCode,data);
    }

    public void SendRequest(RequestCode requestCode,ActionCode actionCode,string data)
    {
        clientSocket.Send(msg.PackData(requestCode, actionCode, data));
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        try
        {
            clientSocket.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("ClientSocket can't be closed:"+e);
        }
    }
}
