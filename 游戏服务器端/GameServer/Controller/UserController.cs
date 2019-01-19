using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.DAO;
using GameServer.Model;
using GameServer.Servers;

namespace GameServer.Controller
{
    class UserController:BaseController
    {
        private UserDAO userDao=new UserDAO();
        private ResultDAO resultDao=new ResultDAO();
        public UserController()
        {
            requestCode = RequestCode.User;
        }

        public string Login(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            string username = strs[0];
            string password = strs[1];
            User user=userDao.VerifyUser(client.conn, username, password);
            if (user==null)
            {
                return ((int) ReturnCode.Fail).ToString();
            }
            Result result = resultDao.GetResultByUserID(client.conn, user.userID);
            if (result.ID==-1)
            {
                resultDao.UpdateOrAddResult(client.conn, result);
            }
            client.SetUserData(user, result);
            return string.Format("{0},{1},{2},{3},{4},{5}", (int) ReturnCode.Success, user.userID,user.username, result.totalCount,
                result.winCount,result.headPortraitPath);
        }

        public string Register(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            string username = strs[0];
            string password = strs[1];
            bool isGot = userDao.GetUserByName(client.conn, username);
            if (isGot != false)
            {
                return ((int)ReturnCode.Fail).ToString();
            }
            userDao.AddUser(client.conn, username, password);
            return ((int)ReturnCode.Success).ToString();
        }

        public string SetHeadPortrait(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            resultDao.SetHeadPortrait(client.conn,strs[1], int.Parse(strs[0]));
            Result result = resultDao.GetResultByUserID(client.conn, int.Parse(strs[0]));
            if (result.headPortraitPath=="n")
            {
                return ((int) ReturnCode.Fail).ToString();
            }

            return ((int) ReturnCode.Success).ToString() + "," + result.headPortraitPath;
        }
           
    }
}
