using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
{
    class User
    {
        public User(int userID, string username, string password)
        {
            this.userID = userID;
            this.username = username;
            this.password = password;
        }
        public int userID;
        public string username;
        public string password;
    }
}
