using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace GameServer.Tool
{
    public class GetMonsterAttri
    {
        public int GetMaxHP(MonsterType monsterType)
        {
            switch (monsterType)
            {
                case MonsterType.Cactus:
                    return 60;
                case MonsterType.PlantMonster:
                    return 90;
                case MonsterType.RockGolem:
                    return 100;
            }

            return 0;
        }
        public int GetAttack(MonsterType monsterType)
        {
            switch (monsterType)
            {
                case MonsterType.Cactus:
                    return 15;
                case MonsterType.PlantMonster:
                    return 20;
                case MonsterType.RockGolem:
                    return 100;
            }

            return 0;
        }
    }
}
