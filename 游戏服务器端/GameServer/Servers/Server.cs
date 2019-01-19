using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Controller;
using Microsoft.SqlServer.Server;

namespace GameServer.Servers
{
    class Server
    {
        private IPEndPoint ipEndPoint;
        private Socket serverSocket;
        private List<Client> clientList = new List<Client>();
        private ControllerManager controllerManager;
        private List<Room> roomList=new List<Room>();

        public Server()
        {

        }

        public Server(string ip, int port)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            controllerManager=new ControllerManager(this);
        }

        public void Start()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipEndPoint);
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallBack, null);
        }

        private void AcceptCallBack(IAsyncResult ar)
        {
            Socket clientSocket = serverSocket.EndAccept(ar);
            Client client = new Client(clientSocket, this);
            client.Start();
            clientList.Add(client);
            serverSocket.BeginAccept(AcceptCallBack, null);
        }

        public void RemoveClient(Client client)
        {
            Console.WriteLine("移除");
            if (clientList.Contains(client))
            {
                lock (clientList)
                {
                    clientList.Remove(client);
                }
            }
            else
            {
                Console.WriteLine("Client不存在，无法移除");
            }
        }

        public void RemoveRoom(Room room)
        {
            if (room!=null&&roomList!=null)
            {
                roomList.Remove(room);
            }
        }

        public void HandleRequest(RequestCode requestCode,ActionCode actionCode,string data,Client client)
        {
            controllerManager.HandleRespond(requestCode,actionCode,data,client);
        }

        public void SendRespond(Client client,ActionCode actionCode,string data)
        {
            client.SendRespond(actionCode,data);
        }

        public void CreateRoom(Client client)
        {
            Room room=new Room(client,this);
            room.AddClient(client);
            roomList.Add(room);
        }

        public int JoinRoom(Client client,Room room)
        {
            int temp=room.AddClient(client);
            return temp;
        }
        public List<Room> GetRoomList()
        {
            return roomList;
        }
        public Room GetRoomByID(int id)
        {
            foreach (Room room in roomList)
            {
                if (room.GetID() == id)
                {
                    return room;
                }
            }

            return null;
        }


    }
}
