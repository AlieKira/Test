using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Data;
using GameServer.Data.RoleAttri;
using GameServer.Servers;
using GameServer.Tool;

namespace GameServer.Controller
{
    class GameController:BaseController
    {
        private int monsterNum=-1;
        private List<string> monsterPosList;
        private GetMonsterAttri monsterAttri=new GetMonsterAttri();
        private MonsterType monsterType;
        private Client deadClient=null;

        public GameController()
        {
            requestCode = RequestCode.Game;
        }

        public string StartGame(string data, Client client, Server server)
        {
            if (client.IsHouseOwner())
            {
                Room room = client.Room;
                foreach (Client temp in room.GetClientDic().Values)
                {
                    if (temp==client)
                    {
                        continue;
                    }
                    if (temp.isPrepare == 1)
                    {
                        return ((int)ReturnCode.NotFound).ToString();
                    }
                }
                room.BroadcastMessage(client, ActionCode.StartGame, ((int)ReturnCode.Success).ToString());
                room.StartTimer();
                return ((int)ReturnCode.Success).ToString();
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }

        public string ChooseCareer(string data, Client client, Server server)
        {
            Room room = client.Room;
            RoleType roleType = (RoleType)int.Parse(data);
            client.roleType = roleType;
            BaseAttri mattri=new BaseAttri();
            mattri.OnInit();
            BaseAttri targetAttri;
            mattri.attriDic.TryGetValue(roleType, out targetAttri);
            client.attri = targetAttri;
            room.AddPrepareClient(client);
            return ((int)roleType).ToString();
        }

        public void SpawnMonster(string data, Client client, Server server)
        {
            MonsterPosList m = new MonsterPosList();
            monsterPosList = m.Positions;
            StringBuilder sb = new StringBuilder();
            foreach (string pos in monsterPosList)
            {
                monsterNum += 1;
                Random r = new Random();
                monsterType = (MonsterType)r.Next(1, 4);
                MonsterData monsterData = new MonsterData(monsterNum,monsterAttri.GetMaxHP(monsterType), monsterAttri.GetAttack(monsterType), monsterType, pos);
                client.Room.monsterList.Add(monsterData);
                sb.Append(monsterNum.ToString()+"`"+((int)monsterType).ToString() + "`"+ pos + "`"+monsterAttri.GetMaxHP(monsterType) 
                          + "`"+monsterAttri.GetAttack(monsterType) + "|");
            }

            monsterNum = -1;
            sb.Remove(sb.Length - 1, 1);
            client.Room.BroadcastMessage(null,ActionCode.SpawnMonster,sb.ToString());
            //return sb.ToString();
        }

        public string Move(string data, Client client, Server server)
        {

            Room room = client.Room;
            if (room != null)
            {
                room.BroadcastMessage(client, ActionCode.Move, data);
            }

            return null;
        }

        public void MonsterMove(string data, Client client, Server server)
        {
            Room room = client.Room;
            if (room != null)
            {
                room.BroadcastMessage(client, ActionCode.MonsterMove, data);
            }
        }

        public void Attack(string data, Client client, Server server)
        {
            if (client.Room==null)
            {
                return;
            }
            bool isDead;
            string[] strs = data.Split(',');
            if (int.Parse(strs[0])!=0)   //攻击目标为怪物
            {
                MonsterData monster=client.Room.monsterList[int.Parse(strs[1])];
                int hp= monster.TakeDamage(client.attri.Attack);
                isDead = monster.IsDead();
                if (isDead)
                {
                    bool lv=client.attri.AddEXP(100);
                    if (lv)
                    {
                        client.Room.BroadcastMessage(null, ActionCode.LevelUp, (client.GetUserId() + "," + client.attri.Lv.ToString()));
                    }

                    client.Room.BroadcastMessage(null,ActionCode.MonsterDead,(strs[1] + "," + hp));
                }
                else
                {
                    client.Room.BroadcastMessage(null, ActionCode.MonsterTakeDamage, (strs[1] + "," + hp));
                }
            }
            else
            {
                foreach (Client temp in client.Room.GetClientDic().Values)
                {
                    if (temp.GetUserId()== int.Parse(strs[1]))
                    {
                        int hp=temp.TakeDamage(client.attri.Attack);
                        isDead = temp.attri.IsDead();
                        if (isDead)
                        {
                            bool lv= client.attri.AddEXP(100);
                            if (lv)
                            {
                                client.Room.BroadcastMessage(null, ActionCode.LevelUp, (client.GetUserId() + "," + client.attri.Lv.ToString()));
                            }
                            client.Room.BroadcastMessage(null, ActionCode.RoleDead, (strs[1] + "," + hp));
                        }
                        else
                        {
                            client.Room.BroadcastMessage(null, ActionCode.RoleTakeDamage, (strs[1] + "," + hp));
                        }
                    }
                }
            }
            client.Room.BroadcastMessage(client, ActionCode.Attack, client.GetUserId().ToString());
        }

        public void MonsterAttack(string data, Client client, Server server)
        {
            if (client.Room == null)
            {
               return;
            }
            int id = int.Parse(data);                //得到被攻击玩家的userid
            Console.WriteLine("玩家:"+id+"，被攻击");
            
            foreach (Client temp in client.Room.GetClientDic().Values)
            {
                if (temp.GetUserId()==id)      
                {
                   int hp= temp.attri.TakeDamage(monsterAttri.GetAttack(monsterType));
                    if (temp.attri.IsDead())                
                    {
                        Console.WriteLine("isdead:"+temp.GetUserId()+","+temp.Room.GetClientDic().Count);
                        deadClient = temp;
                        //广播该玩家死亡
                        client.Room.BroadcastMessage(null, ActionCode.RoleDead, id.ToString());
                        
                        
                    }
                    else
                    {

                        client.Room.BroadcastMessage(null, ActionCode.RoleTakeDamage, id + "," + hp);
                    }
                    
                }
            }

            if (deadClient!=null)
            {
                //从玩家列表中移除死亡玩家
                client.Room.AddDeadClient(deadClient);
                deadClient = null;
            }
            
        }

        public void MonsterReborn(Client client, string monsterNumber)
        {
            Random r=new Random();
            client.Room.BroadcastMessage(null,ActionCode.MonsterReborn,(monsterNumber+","+ r.Next(1, 4)));
        }
    }
}
