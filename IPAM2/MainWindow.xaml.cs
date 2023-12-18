using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static IPAM2.MainWindow;

namespace IPAM2
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
            //MaskSlider.Value = 24;


        }

        public int Status = 0;
        public string strjson;

        public object MaterialDesignRaisedLightButton { get; private set; }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string num = numBox.Text;
            int num2 = Int32.Parse(num);

            MsgBox.Text = null;
            ListPlan.Children.Clear();
            Addip(num2);
            
        }



        public void Addip(int x)
        {

            List<IpInfo2> iplist = new List<IpInfo2>();
            List<string>lklist=new List<string>();
            string[] ls = null;
            ls = LockedIp.Text.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            lklist = new List<string>(ls);

            for (int i = 0; i < x; i++)
            {
                int status = 0;
                string note;
                Brush color;

                List<string>lockip= new List<string>();

                if (i==0)
                {
                    note = "网段IP";
                    color = Brushes.DarkGray;
                    status = 0;
                }
                else
                {
                    if (i==1 & LockBoxFirst.IsChecked == true)
                    {
                        note = "保留IP";
                        color = Brushes.LightSeaGreen;
                        status = 1;
                    }
                    else
                    {
                        if (i == x-2 & LockBoxLast.IsChecked==true)
                        {
                            note = "保留IP";
                            color = Brushes.LightSeaGreen;
                            status = 1;
                        }
                        else
                        {
                            if (i==x-1)
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
                    ToolTip = i.ToString(),
                    Margin = new Thickness(1, 10, 1, 10)
                });


                IPinfo iPinfo = new IPinfo();
                iPinfo.Status = status;
                DateTime date=DateTime.Today;
                iPinfo.Time = date.ToString("yyyy-MM-dd");
                iPinfo.User = "admin";
                iPinfo.Note = note;

                IpInfo2 ipInfo2 = new IpInfo2();
                ipInfo2.Ip = i;
                ipInfo2.Info = iPinfo;

                iplist.Add(ipInfo2);


               


                
            }

            strjson = JsonConvert.SerializeObject(iplist);
            MsgBox.Text += strjson;

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

        private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }




        public class IPinfo
        {


            public int Status { get; set; }
            
            public string User { get; set; }

            public string Time { get; set; }

            public string Note { get; set; }
        }

        public class IpInfo2
        {
            public int Ip { get; set; }

            public IPinfo Info { get; set; }
        }



        /// <summary>
        /// 解析JSON到IP列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnsButton_Click(object sender, RoutedEventArgs e)
        {
            ListPlan2.Children.Clear();

            List<IpInfo2> iplist = new List<IpInfo2>();

            iplist = JsonConvert.DeserializeObject<List<IpInfo2>>(strjson);

            

            

           // MessageBox.Show(iplist.Count.ToString());

            for (int i = 0; i < iplist.Count; i++)
            {

                ReJson(i, iplist[i].Info);
               

                
            }

           

        }



        /// <summary>
        /// JSON解析到IP面板
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="info"></param>
        public void ReJson(int ip, IPinfo info)
        {
            int index = info.Status;
            Brush color=Brushes.DarkBlue ;

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
                    color= Brushes.OrangeRed;

                    break;

                case 5:
                    ///广播IP
                    color= Brushes.Gray;
                    break;
                    

            }





            ListPlan2.Children.Add(new TextBlock()
            {
                Width = 40,
                Height = 20,
                Background = color,
                TextAlignment = TextAlignment.Center,
                Text = ip.ToString(),
                FontSize = 16,
                ToolTip = info.Note,
                Margin = new Thickness(1, 10, 1, 10)
            });



        }


    }
}
