using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace GameServer.Data.RoleAttri
{
    public class KnightAttri:BaseAttri
    {
        public KnightAttri()
        {
            hp = 150;
            mp = 120;
            attack = 16;
            defence = 6;


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
            defence += 4;
            this.exp = 0;
            this.lv += 1;
        }
    }
}
