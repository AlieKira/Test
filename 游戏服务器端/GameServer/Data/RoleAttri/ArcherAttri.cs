using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Data.RoleAttri;

namespace GameServer.Data
{
    public class ArcherAttri:BaseAttri
    {
        public ArcherAttri()
        {
            hp = 100;
            mp = 100;
            attack = 20;
            defence = 4;
        }

        public override int GetAttack()
        {
            return attack;
        }

        public override bool AddEXP(int exp)
        {
            this.exp += exp;
            if (this.exp >= (lv * 100))
            {
                LevelUP();
                return true;
            }

            return false;
        }

        public void LevelUP()
        {
            hp += 20;
            mp += 20;
            attack += 7;
            defence += 2;
            this.exp = 0;
            this.lv += 1;
        }
    }
}
