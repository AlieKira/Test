using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Servers;

namespace GameServer.Controller
{
    class RoomController:BaseController
    {
        public RoomController()
        {
            requestCode = RequestCode.Room;
        }

        public string CreateRoom(string data,Client client,Server server)
        {
            Console.WriteLine("创建房间");
            server.CreateRoom(client);
            return ((int)ReturnCode.Success).ToString();
        }

        public string JoinRoom(string data, Client client, Server server)
        {
            int id = int.Parse(data);
            Room room=server.GetRoomByID(id);
            if (room==null)
            {
                return ((int) ReturnCode.NotFound).ToString() + "^" + "r";
            }
            else if (room.IsWaitingJoin()==false)
            {
                return ((int) ReturnCode.Fail).ToString()+"^"+"r";
            }
            else
            {
                int temp = server.JoinRoom(client,room);
                if (temp==-1)
                {
                    return ((int)ReturnCode.Fail).ToString()+"^"+"r";
                }
                room.BroadcastMessage(client, ActionCode.UpdateRoom, room.GetRoomPlayerData());
                return ((int) ReturnCode.Success).ToString() + "^" + room.GetRoomPlayerData();
            }
            
        }
        public string ListRoom(string data, Client client, Server server)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Room room in server.GetRoomList())
            {
                sb.Append(room.GetHouseOwnerData()+","+room.GetClientsCount() + "|");
            }
            if (sb.Length <= 0)
            {
                return "0";
            }

            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        public string ExitRoom(string data, Client client, Server server)
        {
            bool isHouseOwner = client.IsHouseOwner();
            Room room = client.Room;
            if (isHouseOwner)
            {
                room.BroadcastMessage(client, ActionCode.ExitRoom, ((int)ReturnCode.Success).ToString());
                room.Close();
                return ((int)ReturnCode.Success).ToString();
            }
            else
            {

                client.Room.ExitRoom(client);
                room.BroadcastMessage(client, ActionCode.UpdateRoom, room.GetRoomPlayerData());
                return ((int)ReturnCode.Success).ToString();
            }
        }

        public string PrepareGame(string data, Client client, Server server)
        {
            Room room = client.Room;
            int i=room.GetClientNumber(client);
            room.GetClientDic()[i].SetPrepareState();
            room.BroadcastMessage(client, ActionCode.UpdateRoom, room.GetRoomPlayerData());
            return ((int)ReturnCode.Success).ToString()+","+ i.ToString();
        }
    }
}
