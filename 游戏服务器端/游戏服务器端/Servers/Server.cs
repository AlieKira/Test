using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Servers
{
    class Server
    {
        private IPEndPoint ipEndPoint;
        private Socket serverSocket;
        private  List<Client> clientList=new List<Client>();

        public Server()
        {

        }

        public Server(string ip, int port)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
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
            Client client=new Client(clientSocket,this);
            clientList.Add(client);
            serverSocket.BeginAccept(AcceptCallBack, null);
        }

        public void RemoveClient(Client client)
        {
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

        public void HandleRequest()
        {

        }

        public void SendRespond(Client client)
        {
            client.SendRespond();
        }
    }
}
