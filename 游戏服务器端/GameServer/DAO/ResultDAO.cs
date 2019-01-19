using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using GameServer.Model;
using MySql.Data.MySqlClient;

namespace GameServer.DAO
{
    class ResultDAO
    {
        public Result GetResultByUserID(MySqlConnection conn,int userid)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from result where userid=@userid",conn);
                cmd.Parameters.AddWithValue("userid", userid);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    int totalCount = reader.GetInt32("totalcount");
                    int wincount = reader.GetInt32("wincount");
                    string headPortraitPath = reader.GetString("headportraitpath");
                    return new Result(id, userid, totalCount, wincount,headPortraitPath);
                }
                else
                {
                    return new Result(-1, userid, 0, 0,"n");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetResultByUserID has error:"+e);
                return null;
            }
            finally
            {
                if (reader!=null)
                {
                    reader.Close();
                }
            }
        }

        public void UpdateOrAddResult(MySqlConnection conn,Result result)
        {
            try
            {
                MySqlCommand cmd;
                if (result.ID==-1)
                {
                     cmd=new MySqlCommand("insert into result set id=@id, userid=@userid,totalcount=@totalcount,wincount=@wincount",conn);
                }
                else
                {
                    cmd = new MySqlCommand("update result set totalcount=@totalcount,wincount=@wincount where userid=@userid", conn);
                }
                cmd.Parameters.AddWithValue("id", result.userID);
                cmd.Parameters.AddWithValue("userid", result.userID);
                cmd.Parameters.AddWithValue("totalcount", result.totalCount);
                cmd.Parameters.AddWithValue("wincount", result.winCount);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("UpdateOrAddResult(userid:["+result.userID+"]) has error:"+e);
            }
        }

        public void SetHeadPortrait(MySqlConnection conn,string path,int userid)
        {
            try
            {
                MySqlCommand cmd;
                cmd = new MySqlCommand("update result set headportraitpath=@path where userid=@userid", conn);
                cmd.Parameters.AddWithValue("path",path);
                cmd.Parameters.AddWithValue("userid", userid);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("SetHeadPortrait has error" + e);
            }
        }
    }
}
//if (result.headPortraitPath == "n")
//{
//cmd = new MySqlCommand("insert into result set headportraitpath=@path,userid=@userid", conn);
//cmd.Parameters.AddWithValue("userid", result.ID);
//}
//else
//{
//cmd = new MySqlCommand("update into result headportraitpath=@path", conn);
//}