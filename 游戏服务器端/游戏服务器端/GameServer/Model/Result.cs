using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
{
    class Result
    {
        public Result(int id,int userId,int totalCount,int winCount,string headPortraitPath)
        {
            this.ID = id;
            this.userID = userId;
            this.totalCount = totalCount;
            this.winCount = winCount;
            this.headPortraitPath = headPortraitPath;
        }
        public int ID;
        public int userID;
        public int totalCount;
        public int winCount;
        public string headPortraitPath;
    }
}
