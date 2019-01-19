using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Model;
using MySql.Data.MySqlClient;

namespace GameServer.DAO
{
    class UserDAO
    {
        public User VerifyUser(MySqlConnection conn,string username,string password)
        {
            MySqlDataReader reader=null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from user where username=@username and password=@password",conn);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int id = reader.GetInt32("userid");
                    return new User(id, username, password);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("VerifyUser has error:"+e);
                return null;
            }
            finally
            {
                if (reader!=null)
                {
                    reader.Close();
                }
            }

            return null;
        }

        public bool GetUserByName(MySqlConnection conn, string username)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from user where username=@username", conn);
        cmd.Parameters.AddWithValue("username", username);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetUserByName has error" + e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return false;
        }

        public void AddUser(MySqlConnection conn, string username, string password)
        {
            MySqlDataReader reader;
            try
            {
                MySqlCommand cmd=new MySqlCommand("insert into user set username=@username , password=@password,userid=username", conn);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                cmd.Parameters.AddWithValue("userid", username);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("AddUser(username:["+username+"]) has error:"+e);
            }
        }
    }
}
