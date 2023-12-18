using EducationalAdministrationManagementSystem;
using GloableVariable;
using MySqlConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IPAM2.UserPlan
{
    /// <summary>
    /// Distribution.xaml 的交互逻辑
    /// </summary>
    public partial class Distribution : UserControl
    {
        public Distribution()
        {
            InitializeComponent();
            LoadIpsData();
            DataBinding();
        }

        /// <summary>
        /// 从数据库获取IP段信息
        /// </summary>
        public void LoadIpsData()
        {
            GlobalFunction.FileClass.AnalysisDatabaseConfig(); //解析数据库配置

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


                //第二步,查询IP段信息
                string sql = "SELECT * FROM IPSEGMENT WHERE Del!=1;";

                MySqlDataReader reader = DbConnect.CarrySqlCmd(sql);






                this.ips.Clear();

                while (reader.Read())
                {

                    string IpSegment = reader.GetString("IPSegment");
                    string Mask = reader.GetString("Mask");
                    string Description = reader.GetString("Notes");
                    string Notes = reader.GetString("Notes");
                    string Allocation = reader.GetString("Allocation");


                    string Id = reader.GetString("Id");




                    this.ips.Add(new ViewMode.ViewMode.IpsInfo(IpSegment, Mask, Description, Notes, Allocation, Id));


                }



                DbConnect.MySqlConnection.Close();





            }
            else
            {
                MessageBox.Show("数据库连接信息有误");
            }



        }



        public ObservableCollection<ViewMode.ViewMode.IpsInfo> ips =
            new ObservableCollection<ViewMode.ViewMode.IpsInfo>() { };

        /// <summary>
        /// 绑定IP段数据信息到列表
        /// </summary>
        public void DataBinding()
        {

            IpListView.ItemsSource = ips;


        }


        /// <summary>
        /// JSON解析到IP MAP面板
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="info"></param>
        public void ReJson(int ip, ViewMode.ViewMode.IpInfo ipInfo)
        {

            if (ipInfo != null)
            {

                int index = ipInfo.Status;

                Brush color = Brushes.DarkBlue;

                switch (index)
                {

                    case 0:
                        ///网段IP
                        color = Brushes.DarkGray;

                        break;

                    case 1:
                        ///保留IP
                        color = Brushes.LightSeaGreen;

                        break;

                    case 2:
                        ///未分配IP
                        color = Brushes.AliceBlue;
                        break;

                    case 3:
                        ///已锁定IP
                        color = Brushes.Yellow;
                        break;

                    case 4:
                        ///已分配IP
                        color = Brushes.OrangeRed;

                        break;

                    case 5:
                        ///广播IP
                        color = Brushes.Gray;
                        break;


                }


                //IpMapPlan.Children.Add(new TextBlock()
                //{
                //    Width = 40,
                //    Height = 20,
                //    Background = color,
                //    TextAlignment = TextAlignment.Center,
                //    Text = ip.ToString(),
                //    FontSize = 16,
                //    ToolTip = ipInfo.Note,
                //    Margin = new Thickness(1, 10, 1, 10)
                //});



                //IpMapPlan.Children.Add(new CheckBox()
                //{
                //    Width = 45,
                //    Height = 30,
                //    //Foreground = color,
                //   Background = color,
                //    Style = (Style)FindResource("MaterialDesignFilterChipCheckBox"),
                //    Content = ip.ToString(),
                //    FontSize = 16,
                //    ToolTip = ipInfo.Note,
                //    Margin = new Thickness(1, 10, 1, 10)

                //});


                AddControl(ip, color, index);

            }
            else
            {
                //DebugTextBox.Text = "未正确获取IPINFO";
            }





        }




        public void AddControl(int ip, Brush color, int status)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = ip.ToString();
            textBlock.Width = 50;
            textBlock.Height = 25;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Background = color;
            textBlock.Margin = new Thickness(5);
            textBlock.TextAlignment = TextAlignment.Center;

            if (status != 2)
            {
                textBlock.IsEnabled = false;


            }
            else
            {
                textBlock.IsEnabled = true;
            }



            textBlock.MouseLeftButtonDown += TextBlock_MouseLeftButtonDown;



            IpMapPlan.Children.Add(textBlock);


        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock)
            {


                string text = textBlock.Text;

                if (text.IndexOf("★") != -1)
                {


                    textBlock.Text = text.Replace("★", "");
                    textBlock.Background = Brushes.AliceBlue;


                }
                else
                {

                    textBlock.Text += "★";
                    textBlock.Background = Brushes.LightSalmon;

                }






            }


        }

        private void IpListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var ipsInfo = IpListView.SelectedItem as ViewMode.ViewMode.IpsInfo;

            string configJson = ipsInfo.Allocation;

            // DebugTextBox.Text = configJson;

            IpMapPlan.Children.Clear();


            List<ViewMode.ViewMode.IpInfos> iplist = new List<ViewMode.ViewMode.IpInfos>();

            iplist = JsonConvert.DeserializeObject<List<ViewMode.ViewMode.IpInfos>>(configJson);

            for (int i = 0; i < iplist.Count; i++)
            {

                ReJson(i, iplist[i].IpInfo);

            }
        }
    }
}
