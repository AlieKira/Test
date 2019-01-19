using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace GameServer.Data.RoleAttri
{
    public class MageAttri:BaseAttri
    {
        public MageAttri()
        {
            hp = 100;
            mp = 200;
            attack = 24;
            defence = 2;

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
            mp += 30;
            attack += 8;
            defence += 2;
            this.exp = 0;
            this.lv += 1;
        }
    }
}
