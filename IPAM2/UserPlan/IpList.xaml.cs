using EducationalAdministrationManagementSystem;
using GloableVariable;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IPAM2.EncryptionDecryptionFunction;
using Newtonsoft.Json;
using static IPAM2.MainWindow;
using MaterialDesignThemes.Wpf;
using static System.Net.Mime.MediaTypeNames;
using System.Collections;
using System.Data;
using ControlzEx.Standard;

namespace IPAM2.UserPlan
{
    /// <summary>
    /// IpList.xaml 的交互逻辑
    /// </summary>
    public partial class IpList : UserControl
    {
        public IpList()
        {
            InitializeComponent();
            LoadIpsData();
            DataBinding();
        }

        public ObservableCollection<ViewMode.ViewMode.IpsInfo> ips =
            new ObservableCollection<ViewMode.ViewMode.IpsInfo>(){};

        public List<ViewMode.ViewMode.IpInfos> TempIplist = new List<ViewMode.ViewMode.IpInfos>();

        public string SelectId;
        public int ReloadStatus = 0;
        public int SelectIndex = 0;

        /// <summary>
        /// 页面加载完毕载入数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IpList_OnLoaded(object sender, RoutedEventArgs e)
        {



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


            ReloadStatus = 0;
        }






        /// <summary>
        /// 绑定IP段数据信息到列表
        /// </summary>
        public void DataBinding()
        {
            //SelectIndex = IpListView.SelectedIndex;
            IpListView.ItemsSource = null;
            IpListView.ItemsSource = ips;
            IpListView.SelectedItems.Clear();
            IpListView.SelectedItems.Add(IpListView.Items[SelectIndex]);

        }




        /// <summary>
        /// 加载选中IP段的详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IpListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            if (ReloadStatus == 1)
            {

            }
            else
            {
                var ipsInfo = IpListView.SelectedItem as ViewMode.ViewMode.IpsInfo;

                string configJson = ipsInfo.Allocation;

                SelectId = ipsInfo.Id;

                // DebugTextBox.Text = configJson;
                TEMPTEXTBOX.Text = configJson;

                IpMapPlan.Children.Clear();




                foreach (TextBlock item in IpSelectPlan.Children)
                {
                    string str = item.Text;

                    int index = Convert.ToInt32(str.Replace("★", ""));

                    IpSelectPlan.UnregisterName("tb" + index.ToString());

                    SelectIpNum.Text = "0";

                }
                IpSelectPlan.Children.Clear();


                List<ViewMode.ViewMode.IpInfos> iplist = new List<ViewMode.ViewMode.IpInfos>();

                iplist = JsonConvert.DeserializeObject<List<ViewMode.ViewMode.IpInfos>>(configJson);

                TempIplist = iplist;

                for (int i = 0; i < iplist.Count; i++)
                {

                    ReJson(i, iplist[i].IpInfo);

                }

            }





        }


        /// <summary>
        /// 加载选中IP段地址
        /// </summary>
        /// <param name="index">索引</param>
        public void LoadSelectIp(int index2)
        {

            IpListView.SelectedIndex = index2;

            

            


        }



        /// <summary>
        /// JSON解析到IP MAP面板
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="ipInfo"></param>
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
                

                AddControl(ip,color,index, ipInfo.Note+"\n"+ipInfo.User ,index);

            }
            else
            {
                //DebugTextBox.Text = "未正确获取IPINFO";
            }





        }



        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="color"></param>
        /// <param name="status"></param>
        public void AddControl(int ip,Brush color,int status,string note,int tag)
        {

                TextBlock textBlock = new TextBlock();
                textBlock.Text = ip.ToString();
                textBlock.Width = 50;
                textBlock.Height = 25;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                textBlock.Background = color;
                textBlock.Margin = new Thickness(5);
                textBlock.TextAlignment = TextAlignment.Center;
                textBlock.ToolTip = "该地址用途为: " + note+ " 为责任人";
                textBlock.Tag = tag;

                if (status != 2)
                {
                   // textBlock.IsEnabled = false;


                }
                else
                {
                   // textBlock.IsEnabled = true;
                }



                textBlock.MouseLeftButtonDown += TextBlock_MouseLeftButtonDown;



                IpMapPlan.Children.Add(textBlock);


            





        }


        /// <summary>
        /// 选择IP地址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock)
            {
                int tag = (Int32)textBlock.Tag;

                if (tag == 2)
                {

                    string text = textBlock.Text;

                    if (text.IndexOf("★") != -1)
                    {

                        textBlock.Text = text.Replace("★", "");
                        textBlock.Background = Brushes.AliceBlue;

                        TextBlock tb = IpSelectPlan.FindName("tb" + textBlock.Text) as TextBlock;

                        if (tb != null)
                        {
                            IpSelectPlan.Children.Remove(tb);
                            IpSelectPlan.UnregisterName("tb" + textBlock.Text);
                        }
                        else
                        {
                            // MessageBox.Show("未发现组件"+"tb"+textBlock.Text);
                        }






                    }
                    else
                    {
                        string str = "tb" + textBlock.Text;
                        textBlock.Text += "★";
                        textBlock.Background = Brushes.LightSalmon;

                        TextBlock tb = new TextBlock();

                        tb.Text = textBlock.Text;
                        tb.Background = textBlock.Background;
                        tb.Height = textBlock.Height;
                        tb.Width = textBlock.Width;
                        tb.Margin = textBlock.Margin;
                        tb.TextAlignment = TextAlignment.Center;

                        IpSelectPlan.Children.Add(tb);
                        IpSelectPlan.RegisterName(str, tb);

                    }


                    //统计IP数量
                    CountIP();




                }
                else
                {

                    MessageBox.Show("该IP地址被占用！请选择其他未分配IP地址");

                }





            }

            
        }



        /// <summary>
        /// 查找控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parentObj">开始查找的父控件</param>
        /// <param name="typename">需要查找控件名称</param>
        /// <returns></returns>
        public List<T> GetChildObjects<T>(DependencyObject parentObj, Type typename) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(parentObj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(parentObj, i);

                if (child is T && (((T)child).GetType() == typename))
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child, typename));
            }
            return childList;
        }




        /// <summary>
        /// 统计已选择IP数量
        /// </summary>
        public  void CountIP()
        {

            int num = IpSelectPlan.Children.Count;

            SelectIpNum.Text = num.ToString();

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {


            if (IpUse.Text != "" && IpUser.Text != "")
            {
                ReloadStatus = 1;

                string sqlStr;

                foreach (TextBlock item in IpSelectPlan.Children)
                {
                    string str = item.Text;

                    int index = Convert.ToInt32(str.Replace("★", ""));
                                       
                    ViewMode.ViewMode.IpInfo ipInfo = new ViewMode.ViewMode.IpInfo(4, IpUser.Text, DateTime.Now.ToString("yyyy-MM-dd"), IpUse.Text);

                    TempIplist[index].IpInfo = ipInfo;
                }

                sqlStr= JsonConvert.SerializeObject(TempIplist);

                string sql = string.Format("UPDATE IPSEGMENT SET  Allocation ='{0}' WHERE id ='{1}'",sqlStr,SelectId);


                if (GlobalVariable.DbConnectInfo != null)
                {
                    if (DbConnect.MySqlConnection.State == ConnectionState.Closed)
                    {
                        DbConnect.MySqlConnection.Open(); //连接数据库
                    }

                    DbConnect.ModifySql(sql);

                }

                DbConnect.MySqlConnection.Close();

                TEMPTEXTBOX.Text =SelectId+"    "+ sqlStr;

                //重新加载数据
                IpMapPlan.Children.Clear();
                DataBinding();
                LoadIpsData();



                //重新加载选中IP段
                LoadBeforeSelectData(SelectIndex);

                IpSelectPlan.Children.Clear();
                SelectIpNum.Text = "0";


            }
            else
            {
                MessageBox.Show("IP地址使用人信息不完整，无法保存！");
            }








        }



    /// <summary>
    /// 加载之前选定项更新后的内容
    /// </summary>
        public void LoadBeforeSelectData(int index)
        {
            var ipsInfo = ips[index];

            var confjson = ipsInfo.Allocation;

            List<ViewMode.ViewMode.IpInfos> iplist = new List<ViewMode.ViewMode.IpInfos>();

            iplist = JsonConvert.DeserializeObject<List<ViewMode.ViewMode.IpInfos>>(confjson);



            for (int i = 0; i < iplist.Count; i++)
            {

                ReJson(i, iplist[i].IpInfo);

            }


        }



        /// <summary>
        /// 鼠标点击ListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IpListView_MouseDown(object sender, MouseButtonEventArgs e)
        {





        }
    }
}
