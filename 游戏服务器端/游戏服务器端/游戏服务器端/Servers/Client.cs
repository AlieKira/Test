using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Servers
{
    class Client
    {
        private Socket clientSocket;
        private Server server;
        private  Message msg=new Message();

        public Client()
        {

        }

        public Client(Socket clientSocket, Server server)
        {
            this.clientSocket = clientSocket;
            this.server = server;
        }

        public void Start()
        {
            if (clientSocket==null||clientSocket.Connected==false)return;
            clientSocket.BeginReceive(msg.DataBytes, msg.StartIndex, msg.RemainCount, SocketFlags.None, ReceiveCallBack,
                null);

        }

        public void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                if (clientSocket == null || clientSocket.Connected == false) return;
                int count = clientSocket.EndReceive(ar);
                if (count<=0)
                {
                    return;
                }
                msg.ReadMessage(count);
                Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        public void SendRespond()
        {

        }
    }
}
