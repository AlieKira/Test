using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Data
{
    public class MonsterPosList
    {
        private List<string> positions = new List<string>() { "5,5,0", "8,5,3", "-5.1,5,6", "-10,5,3", "8,5,10", "8,5,-3" };
        //private List<string> positions = new List<string>() { "-5,5,0" };


        public List<string> Positions
        {
            get { return positions; }
        }
    }
}
