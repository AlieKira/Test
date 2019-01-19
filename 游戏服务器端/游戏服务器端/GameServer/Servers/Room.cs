using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using GameServer.Controller;
using GameServer.Data;
using GameServer.Data.RoleAttri;
using GameServer.Tool;

namespace GameServer.Servers
{
    enum RoomState
    {
        WaitingJoin,
        WaitingBattle,
        Battle,
        End
    }
    class Room
    {
        private Dictionary<int, Client> clientDic = new Dictionary<int, Client>();
        private List<Client> prepareClientList = new List<Client>();
        public List<MonsterData> monsterList = new List<MonsterData>();
        private List<Client> deadClientList = new List<Client>();
        private Server server;
        private Client client;
        private RoomState state = RoomState.WaitingJoin;


        private GetMonsterPosList positionList = new GetMonsterPosList();

        public Room(Client client, Server server)
        {
            this.client = client;
            this.server = server;
        }

        public int AddClient(Client client)
        {
            for (int i = 0; i < 6; i++)
            {
                if (clientDic.ContainsKey(i) == false)
                {
                    clientDic.Add(i, client);
                    client.Room = this;
                    return i;
                }
            }
            return -1;
        }

        public int GetClientsCount()
        {
            return clientDic.Count;
        }

        public string GetHouseOwnerData()
        {
            Client temp;
            clientDic.TryGetValue(0, out temp);
            return temp.GetHouseOwnerData();
        }

        public int GetID()
        {
            if (clientDic.Count > 0)
            {
                Client temp;
                clientDic.TryGetValue(0, out temp);
                return temp.GetUserId();
            }

            return -1;
        }

        public bool AddPrepareClient(Client client)
        {
            prepareClientList.Add(client);
            if (prepareClientList.Count == clientDic.Count)
            {
                return true;
            }

            return false;
        }

        public bool IsWaitingJoin()
        {
            return state == RoomState.WaitingJoin;
        }

        public string GetRoomPlayerData()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var temp in clientDic)
            {
                sb.Append(temp.Key + "," + temp.Value.GetUserData() + "|");
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);

            }

            return sb.ToString();
        }

        public void BroadcastMessage(Client excludeClient, ActionCode actionCode, string data)
        {
            lock (clientDic)
            {
                foreach (Client client in clientDic.Values)
                {
                    if (client != excludeClient)
                    {
                        server.SendRespond(client, actionCode, data);
                    }
                }
            }

        }

        public void ExitRoom(Client client)           //客户端退出房间
        {
            Client c;
            clientDic.TryGetValue(0, out c);
            if (client == c)
            {
                if (client.Room != null)
                {
                    Console.WriteLine("close");
                    Close();
                }

            }
            else
            {
                clientDic.Remove(GetClientNumber(client));
            }
            if (clientDic.Count >= 6)
            {
                state = RoomState.WaitingBattle;
            }
            else
            {
                state = RoomState.WaitingJoin;
            }

        }

        public void Close()             //关闭房间
        {
            foreach (Client client in clientDic.Values)
            {
                client.Room = null;
            }
            server.RemoveRoom(this);
        }

        public bool IsHouseOwner(Client client)
        {
            Client temp;
            clientDic.TryGetValue(0, out temp);
            return client == temp;
        }

        public int GetClientNumber(Client client)
        {
            int i = -1;
            foreach (var temp in clientDic)
            {
                if (client == temp.Value)
                {
                    i = temp.Key;
                }
            }

            return i;
        }

        public Dictionary<int, Client> GetClientDic()
        {
            return clientDic;
        }

        public void StartTimer()
        {
            new Thread(RunTimer).Start();
        }

        private void RunTimer()
        {
            Thread.Sleep(1000);
            
            for (int i = 3; i > 0; i--)
            {
                BroadcastMessage(null, ActionCode.ShowTimer, i.ToString());
                Thread.Sleep(1000);
            }
            new Thread(RunCareerTimer).Start();
        }


        private void RunCareerTimer()           //职业选择界面计时
        {
            Thread.Sleep(1000);
            for (int i = 2; i > -1; i--)
            {
                BroadcastMessage(null, ActionCode.ShowCareerTimer, i.ToString());
                Thread.Sleep(1000);
            }

            int x = -1;
            if (prepareClientList.Count != clientDic.Count)        //计时结束，但仍有玩家未选择职业，则随机为其选择
            {
                foreach (Client temp in clientDic.Values)
                {
                    if (temp.roleType == RoleType.none)
                    {
                        Random r = new Random();
                        x = r.Next(1, 6);
                        temp.roleType = (RoleType) x;
                        Console.WriteLine(temp.GetUserId()+":"+temp.roleType.ToString());
                        BaseAttri mattri = new BaseAttri();
                        mattri.OnInit();
                        BaseAttri targetAttri;
                        mattri.attriDic.TryGetValue(temp.roleType, out targetAttri);
                        temp.attri = targetAttri;
                        
                        temp.Room.AddPrepareClient(temp);
                        server.SendRespond(temp, ActionCode.ChooseCareer, x.ToString());
                        Console.WriteLine("choose career");
                    }
                }
            }
            StringBuilder sb = new StringBuilder();
            foreach (var temp in clientDic)
            {
                sb.Append((int)temp.Value.roleType + "-");
                sb.Append(positionList.Positions[temp.Key] + "-");
                Console.WriteLine("clientID:"+temp.Value.GetUserId());
                sb.Append(temp.Value.attri.HP + "-");
                sb.Append(temp.Value.GetUserId() + "|");
            }
            sb.Remove(sb.Length - 1, 1);

            BroadcastMessage(null, ActionCode.StartPlay, sb.ToString());       //广播职业信息，客户端开始游戏
        }

        public void AddDeadClient(Client client)
        {
            prepareClientList.Remove(client);
            if (prepareClientList.Count == 1)     //若房间剩下一人，该玩家胜利，其他失败
            {
                Console.WriteLine("add deadaclient");
                BroadcastMessage(prepareClientList[0], ActionCode.GameOver, ((int)ReturnCode.Fail).ToString());
                server.SendRespond(prepareClientList[0], ActionCode.GameOver, ((int)ReturnCode.Success).ToString());
                List<Client> dic = new List<Client>();
                foreach (Client temp in clientDic.Values)
                {
                    dic.Add(temp);
                }
                foreach (Client temp in dic)
                {
                    temp.Init();
                    ExitRoom(temp);            //所有玩家退出游戏房间
                }

            }
            else if (prepareClientList.Count == 0)       //若房间无人剩余，所有玩家失败
            {
                BroadcastMessage(null, ActionCode.GameOver, ((int)ReturnCode.Fail).ToString());
                
                Dictionary<int, Client> dic = clientDic;
                foreach (Client temp in dic.Values)
                {
                    temp.Init();
                    ExitRoom(temp);              //所有玩家退出游戏房间
                }
            }
        }
    }
}
