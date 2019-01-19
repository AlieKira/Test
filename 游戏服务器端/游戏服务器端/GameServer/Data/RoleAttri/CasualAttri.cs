using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace GameServer.Data.RoleAttri
{
    public class CasualAttri:BaseAttri
    {
        public CasualAttri()
        {
            hp = 100;
            mp = 70;
            attack = 12;
            defence = 4;

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
            hp += 30;
            mp += 20;
            attack += 5;
            defence += 3;
            this.exp = 0;
            this.lv += 1;
        }
    }
}
