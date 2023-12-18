using System;
using System.Windows;
using GloableVariable;
using MySqlConnector;


namespace EducationalAdministrationManagementSystem
{
    public class DbConnect //数据库连接操作库
    {


        public static MySqlConnector.MySqlConnection MySqlConnection;



        //连接MYSQL数据库
        public static MySqlConnector.MySqlConnection ConnectionMysql(string DbConnectInfo)
        {
            MySqlConnection = new MySqlConnection(DbConnectInfo);

            return MySqlConnection;
        }



        /// <summary>
        /// 在指定的mysql连接上执行sql语句,返回MySqlDataReader 
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public static MySqlDataReader CarrySqlCmd(string sql)
        {
            MySqlCommand cmdCommand = new MySqlCommand(sql, MySqlConnection);
            MySqlDataReader reader = cmdCommand.ExecuteReader();

            return reader;

        }


        /// <summary>
        /// 执行mysql修改,插入数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回受影响的行数,为int值</returns>
        public static int ModifySql(string sql)
        {
            try
            {
                MySqlConnection = DbConnect.ConnectionMysql(GlobalVariable.DbConnectInfo);
                MySqlConnection.Open(); //连接数据库
                MySqlCommand SqlCommand = new MySqlCommand(sql, MySqlConnection);
                return SqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 0;
            }

           
        }






    }


}
