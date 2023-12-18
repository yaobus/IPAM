using IPAM2.UserPlan;
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
    /// IPMA.xaml 的交互逻辑
    /// </summary>
    public partial class IPMA : Window
    {
        public IPMA()
        {
            InitializeComponent();
            ChlidPlan.Children.Clear();
            ChlidPlan.Children.Add(new IpList());
        }



        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {

                try
                {
                    DragMove();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }

                
            }
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }



        /// <summary>
        /// 选择不同菜单的功能加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;



            switch (index)
            {
                case 0:
                    
                    ChlidPlan.Children.Clear();
                    ChlidPlan.Children.Add(new IpList());

                    break;
                case 1:
                    ChlidPlan.Children.Clear();
                    ChlidPlan.Children.Add(new IpManage());
                    break;

                case 2:
                    ChlidPlan.Children.Clear();
                    ChlidPlan.Children.Add(new Distribution());

                    break;

                default:
                    

                    break;



            }
        }


        /// <summary>
        /// 设置菜单加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewSet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }



        /// <summary>
        /// 程序加载完毕，解析数据库配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IPMA_OnLoaded(object sender, RoutedEventArgs e)
        {

            GlobalFunction.FileClass.AnalysisDatabaseConfig();//解析数据库配置
        }



    }
}
