using Newtonsoft.Json;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using IPAM2.ViewMode;
using EducationalAdministrationManagementSystem;
using GloableVariable;
using System.Data;

namespace IPAM2.UserPlan
{
    /// <summary>
    /// IpManage.xaml 的交互逻辑
    /// </summary>
    public partial class IpManage : UserControl
    {
        public IpManage()
        {
            InitializeComponent();
        }



        public int Status = 0;
        public string strjson;

        public object MaterialDesignRaisedLightButton { get; private set; }







        public void Addip(int x)
        {

            List<ViewMode.ViewMode.IpInfos> iplist = new List<ViewMode.ViewMode.IpInfos>();
            List<string> lklist = new List<string>();
            string[] ls = null;
            ls = LockedIp.Text.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            lklist = new List<string>(ls);

            for (int i = 0; i < x; i++)
            {
                int status = 0;
                string note;
                Brush color;

                List<string> lockip = new List<string>();

                if (i == 0)
                {
                    note = "网段IP";
                    color = Brushes.DarkGray;
                    status = 0;
                }
                else
                {
                    if (i == 1 & LockBoxFirst.IsChecked == true)
                    {
                        note = "保留IP";
                        color = Brushes.LightSeaGreen;
                        status = 1;
                    }
                    else
                    {
                        if (i == x - 2 & LockBoxLast.IsChecked == true)
                        {
                            note = "保留IP";
                            color = Brushes.LightSeaGreen;
                            status = 1;
                        }
                        else
                        {
                            if (i == x - 1)
                            {
                                note = "广播IP";
                                color = Brushes.DarkGray;
                                status = 5;
                            }
                            else
                            {
                                note = "未分配IP";
                                color = Brushes.AliceBlue;
                                status = 2;
                            }

                        }

                    }
                }



                //手动锁定需要保留的IP
                if (lklist.Contains(i.ToString()))
                {

                    note = "保留IP";
                    color = Brushes.LightSeaGreen;
                    status = 1;


                }



                ListPlan.Children.Add(new TextBlock()
                {
                    Width = 40,
                    Height = 20,
                    Background = color,
                    TextAlignment = TextAlignment.Center,
                    Text = i.ToString(),
                    FontSize = 16,
                    ToolTip = note,
                    Margin = new Thickness(1, 10, 1, 10)
                });

               

     
                DateTime date = DateTime.Today;
                string time= date.ToString("yyyy-MM-dd");



                ViewMode.ViewMode.IpInfo ipInfo = new ViewMode.ViewMode.IpInfo(status, "admin", time, note);


                
                ViewMode.ViewMode.IpInfos ipInfos = new ViewMode.ViewMode.IpInfos(i, ipInfo);


                iplist.Add(ipInfos);





            }

            strjson = JsonConvert.SerializeObject(iplist);
          //  MsgBox.Text += strjson;

        }





        /// <summary>
        /// 计算子网掩码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaskSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {


            ClacIp();

           
        }

        /// <summary>
        /// IP地址计算
        /// </summary>
        public void ClacIp()
        {
            if (Status < 2)
            {
                Status += 1;
            }
            else
            {

                int value = (int)MaskSlider.Value;

                int num = 32 - value;
                string str = null;
                int tempStr = 8 - num;


                for (int i = 0; i < tempStr; i++)
                {
                    str += "1";
                }

                for (int i = 0; i < num; i++)
                {
                    str += "0";
                }

                int t = Convert.ToInt32(str, 2);


                maskText.Text = "255.255.255." + t.ToString();



                numBox.Text = (256 - t).ToString();



                string ipstr = IPTextBox.Text;

                int index = ipstr.LastIndexOf(".") + 1;


                var strtemp = ipstr.Substring(0, index);



                FTextBox.Text = strtemp + "1";

                LTextBox.Text = strtemp + (254 - t).ToString();

                STextBox.Text = strtemp + (255 - t).ToString();
            }




        }





        private void IPTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (Status < 2)
            {
                Status += 1;
            }
            else
            {
                ClacIp();
            }



        }









        /// <summary>
        /// 解析JSON到IP列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnsButton_Click(object sender, RoutedEventArgs e)
        {
           // ListPlan2.Children.Clear();

            List<ViewMode.ViewMode.IpInfos> ipList = new List<ViewMode.ViewMode.IpInfos>();

            ipList = JsonConvert.DeserializeObject<List<ViewMode.ViewMode.IpInfos>>(strjson);





            // MessageBox.Show(iplist.Count.ToString());

            for (int i = 0; i < ipList.Count(); i++)
            {

                ReJson(i, ipList[i].IpInfo);



            }



        }



        /// <summary>
        /// JSON解析到IP面板
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="info"></param>
        public void ReJson(int ip, ViewMode.ViewMode.IpInfo info)
        {
            int index = info.Status;
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



        }



        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string num = numBox.Text;
            int num2 = Int32.Parse(num);

            //MsgBox.Text = null;
            ListPlan.Children.Clear();
            Addip(num2);
        }



        /// <summary>
        /// 保存IP段信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {

            string num = numBox.Text;
            int num2 = Int32.Parse(num);

            //MsgBox.Text = null;
            ListPlan.Children.Clear();
            Addip(num2);



            string id =
                EncryptionDecryptionFunction.MD5EncryptionDecryption.MyTextMD5(IPTextBox.Text+DateTime.Now, 8);

            string sql = string.Format("INSERT INTO IPSEGMENT  VALUES ('{0}','{1}','{2}','{3}','{4}','{5}',0)",IPTextBox.Text,maskText.Text,IPsNote.Text, IPsNote.Text,strjson,id);


            if (GlobalVariable.DbConnectInfo != null)
            {
                if (DbConnect.MySqlConnection.State == ConnectionState.Closed)
                {
                    DbConnect.MySqlConnection.Open(); //连接数据库
                }

                DbConnect.ModifySql(sql);

            }

            DbConnect.MySqlConnection.Close();
        }
    }
}
