using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace GameServer.Data.RoleAttri
{
    public class BarbarianAttri:BaseAttri
    {
        public BarbarianAttri()
        {
            hp = 200;
            mp = 90;
            attack = 14;
            defence = 10;

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
            hp += 50;
            mp += 20;
            attack += 5;
            defence += 3;
            this.exp = 0;
            this.lv += 1;
        }
    }
}
