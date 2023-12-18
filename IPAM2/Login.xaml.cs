using EducationalAdministrationManagementSystem;
using GloableVariable;
using IPAM2.EncryptionDecryptionFunction;
using IPAM2.UserPlan;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IPAM2
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 程序启动后加载数据库配置信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_OnLoaded(object sender, RoutedEventArgs e)
        {

            GlobalFunction.FileClass.AnalysisDatabaseConfig();//解析数据库配置


        }



        /// <summary>
        /// 允许程序界面任意拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }


        /// <summary>
        /// 打开设置页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (GlobalVariable.ServerSetPlanShow == false)
            {
                FunctionPlan.Children.Clear();
                FunctionPlan.Children.Add(new ServerSettingPlan());
                GlobalVariable.ServerSetPlanShow = true;
            }
            else
            {
                FunctionPlan.Children.Clear();
                GlobalVariable.ServerSetPlanShow = false;
            }
        }


        /// <summary>
        /// 结束程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }




        /// <summary>
        /// 登陆验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            ProgressBar.IsIndeterminate = true;

            if (GlobalVariable.ServerSetPlanShow == true)
            {
                GlobalVariable.ServerSetPlanShow = false;
                FunctionPlan.Children.Clear();
            }


            GlobalFunction.FileClass.AnalysisDatabaseConfig();//解析数据库配置


            //第一步,连接数据库

            if (GlobalVariable.DbConnectInfo != null)
            {
                try
                {
                    DbConnect.MySqlConnection = DbConnect.ConnectionMysql(GlobalVariable.DbConnectInfo);
                    DbConnect.MySqlConnection.Open(); //连接数据库

                }
                catch (Exception)
                {
                    MessageBox.Show("数据库连接失败!请检查数据库配置");
                }

                //第二步,查询比对管理员登录信息
                string sql = string.Format("SELECT * FROM user WHERE user='{0}'", UserNameBox.Text);

                MySqlDataReader reader = DbConnect.CarrySqlCmd(sql);
                if (reader.Read())
                {

                    string password = reader.GetString("PASSWORD");

                    if (password == MD5EncryptionDecryption.PasswordMD5(UserPasswordBox.Password))//密码正确
                    {
                        //释放连接资源
                        reader.Dispose();

                        if (UserNameBox.Text == "Administrator")
                        {
                            Window mainWindow = new Manage();
                            var window = Window.GetWindow(this);//关闭父窗体
                            window?.Close();

                            //打开新窗口
                            mainWindow.Show();
                        }
                        else
                        {
                            Window mainWindow = new IPAMWindow();
                            var window = Window.GetWindow(this);//关闭父窗体
                            window?.Close();

                            //打开新窗口
                            mainWindow.Show();

                        }



                       







                    }
                    else
                    {
                        MessageBox.Show("用户名或密码错误!");
                    }






                }
                else
                {
                    MessageBox.Show("用户名或密码错误!");
                }

            }


        }

        private void RregisterButton_Click(object sender, RoutedEventArgs e)
        {




        }
    }
}
