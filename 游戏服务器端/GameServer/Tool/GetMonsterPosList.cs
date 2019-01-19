using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Tool
{
    class GetMonsterPosList
    {


        private List<string> positions=new List<string>(){ "12.5,3,0" , "16,3,0" , "20,3,0" , "24,3,0" , "26,3,0" , "28,3,0" };

        public List<string> Positions
        {
            get { return positions; }
        }
    }
}
