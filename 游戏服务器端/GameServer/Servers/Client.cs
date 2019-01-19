using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Data.RoleAttri;
using GameServer.Model;
using GameServer.Tool;
using MySql.Data.MySqlClient;

namespace GameServer.Servers
{
    class Client
    {
        private Socket clientSocket;
        private Server server;
        public MySqlConnection conn;
        private Message msg = new Message();
        private User user;
        private Result result;
        public int isPrepare=1;  //0=true,1=false
        private Room room;

        public RoleType roleType=RoleType.none;
        public BaseAttri attri;

        private bool isgetid = false;//TODO



        public Room Room
        {
            get { return room; }
            set { room = value; }
        }


        public Client()
        {

        }

        public Client(Socket clientSocket, Server server)
        {
            this.clientSocket = clientSocket;
            this.server = server;
            this.conn = ConnectHelper.Connect();
        }

        public void Start()
        {
            if (clientSocket == null || clientSocket.Connected == false) return;
            clientSocket.BeginReceive(msg.DataBytes, msg.StartIndex, msg.RemainCount, SocketFlags.None, ReceiveCallBack,
                null);

        }

        public void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                if (clientSocket == null || clientSocket.Connected == false) return;
                int count = clientSocket.EndReceive(ar);
                if (isgetid&&GetUserId()==1)
                
                if (count <= 0)
                {
                    Close();
                }
                msg.ReadMessage(count,OnProcessCallBack);
                Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Close();
            }

        }

        private void OnProcessCallBack(RequestCode requestCode, ActionCode actionCode, string data)
        {
            server.HandleRequest(requestCode,actionCode,data,this);
        }

        public void SendRespond(ActionCode actionCode,string data)
        {
            try
            {
                clientSocket.Send(msg.PackBytes(actionCode, data));
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't send the respond.Error message is:"+e);
                return;
            }
        }

        private void Close()
        {
            Console.WriteLine("client close");
            ConnectHelper.Close(conn);
            if (clientSocket != null)
            {
                clientSocket.Close();
                if (room != null)
                {
                    room.ExitRoom(this);
                }
                server.RemoveClient(this);

            }

        }

        public void SetUserData(User user, Result result)
        {
            isgetid = true;
            this.user = user;
            this.result = result;
        }

        public string GetHouseOwnerData()
        {
            return user.userID + "," + user.username;
        }

        public string GetUserData()
        {
            return user.userID +","+ user.username +","+ result.totalCount + "," + result.winCount + "," +
                   result.headPortraitPath + "," + isPrepare;
        }
        public int GetUserId()
        {
            return user.userID;
        }

        public void SetPrepareState()
        {
            if (isPrepare==1)
            {
                isPrepare = 0;
            }
            else
            {
                isPrepare = 1;
            }
        }

        public int GetPrepareState()
        {
            return isPrepare;
        }

        public bool IsHouseOwner()
        {
            return room.IsHouseOwner(this);
        }

        public int TakeDamage(int damage)
        {
            return attri.TakeDamage(damage);
        }

        public void Init()
        {
            roleType = RoleType.none;
            attri = new BaseAttri();
        }
    }
}
