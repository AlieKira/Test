using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace GameServer.Data.RoleAttri
{
    public class BaseAttri
    {
        protected int hp;
        protected int mp;
        protected int attack = 0;
        protected int defence = 0;
        protected int exp = 0;
        protected int lv=0;

        public Dictionary<RoleType, BaseAttri> attriDic =new Dictionary<RoleType, BaseAttri>();

        public void OnInit()
        {
            attriDic.Add(RoleType.Archer, new ArcherAttri());
            attriDic.Add(RoleType.Barbarian,new BarbarianAttri());
            attriDic.Add(RoleType.Casual,new CasualAttri());
            attriDic.Add(RoleType.Knight,new KnightAttri());
            attriDic.Add(RoleType.Mage,new MageAttri());
        }

        public virtual int GetAttack()
        {
            return attack;
        }

        public virtual bool AddEXP(int exp)
        {
            return false;
        }

        public int HP
        {
            get { return hp; }
        }
        public int MP
        {
            get { return mp; }
        }
        public int Attack
        {
            get { return attack; }
        }
        public int Defence
        {
            get { return defence; }
        }

        public int Lv
        {
            get { return lv; }
        }

        public int TakeDamage(int damage)
        {
            this.hp -= (damage-defence);
            return hp;
        }

        public bool IsDead()
        {
            return hp <= 0;
        }
    }
}
