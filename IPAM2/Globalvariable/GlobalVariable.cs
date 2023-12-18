using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;

namespace GloableVariable
{
    public class GlobalVariable
    {


        //登陆界面的设置面板是否已经显示,默认为假
        public static bool ServerSetPlanShow = false;

        //数据库路径
        public static string DbPath = Directory.GetCurrentDirectory() + @"\config\db_config.json";//指定配置文件路径


        //数据库配置信息
        public static JObject JObject;


        //数据库配置信息显示状态,判断是否正在向配置框加载配置信息
        public static bool DatabaseInfoShowStatus = false;



        

        //----------------------Database-Info-------------------------
        //Remote-Server-Core服务器ip地址
        public static IPAddress IpAddress;

        //Remote-Server-Core服务器端口
        public static Int32 ServerPort;

        //与Remote-Server-Core服务器建立的socket
        public static Socket ServerSocket;

        //定义数据接收缓存库
        public static  byte[] ResultBytes = new byte[1024 * 1024 * 64];

        //数据库连接信息
        public static string DbConnectInfo;







    }


}
