using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace GameServer.Data
{
    public class MonsterData
    {
        private int number;
        private MonsterType monsterType = MonsterType.none;
        private string bornPos=null;
        private int hp = 0;
        private int attack = 0;

        public int Number
        {
            get { return number; }
        }

        public int HP
        {
            get { return hp; }
        }

        public int Attack
        {
            get { return attack; }
        }

        public MonsterType MonsterType
        {
            get { return monsterType; }
        }

        public string BornPos
        {
            get { return bornPos; }
        }

        public MonsterData(int number,int hp,int attack,MonsterType type,string bornPos)
        {
            this.number = number;
            this.hp = hp;
            this.attack = attack;
            this.monsterType = type;
            this.bornPos = bornPos;

        }

        public int TakeDamage(int damage)
        {
            this.hp -= damage;
            return this.hp;
        }

        public bool IsDead()
        {
            return hp <= 0;
        }
    }
}
