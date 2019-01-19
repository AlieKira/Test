using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace GameServer.Tool
{
    class GetRoleAttri
    {
        public int GetAttri(RoleType roleType)
        {
            switch (roleType)
            {
                case RoleType.Archer:
                    return 20;
                case RoleType.Barbarian:
                    return 30;
                case RoleType.Casual:
                    return 50;
            }

            return 0;
        }
        public int GetAttack(MonsterType monsterType)
        {
            switch (monsterType)
            {
                case MonsterType.Cactus:
                    return 5;
                case MonsterType.PlantMonster:
                    return 10;
                case MonsterType.RockGolem:
                    return 20;
            }

            return 0;
        }
    }
}
