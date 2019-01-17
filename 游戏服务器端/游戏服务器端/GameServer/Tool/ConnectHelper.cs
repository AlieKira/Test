using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace GameServer.Tool
{
    class ConnectHelper
    {
        private const string CONNECTIONSTRING = "database=chaosfight;datasource=127.0.0.1;port=3306;user=root;pwd=root";

        public static MySqlConnection Connect()
        {
            MySqlConnection conn=new MySqlConnection(CONNECTIONSTRING);
            try
            {
                conn.Open();
                return conn;
            }
            catch (Exception e)
            {
                Console.WriteLine("无法连接MySQL:"+e);
                return null;
            }
        }

        public static void Close(MySqlConnection conn)
        {
            if (conn!=null)
            {
               conn.Close();
            }
            else
            {
                Console.WriteLine("MySqlconnection不能为空");
            }
        }
    }
}
