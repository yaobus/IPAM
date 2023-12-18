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
    /// IPAMWindow.xaml 的交互逻辑
    /// </summary>
    public partial class IPAMWindow : Window
    {
        public IPAMWindow()
        {
            InitializeComponent();
            wdgl_Click(null,null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton==MouseButton.Left)
            {
              //  DragMove();
            }
        }

        private void wdgl_Click(object sender, RoutedEventArgs e)
        {

         MainPlane.Children.Clear();

            MainPlane.Children.Add(new IpList());


        }

        private void dzfp_Click(object sender, RoutedEventArgs e)
        {
            MainPlane.Children.Clear();

            MainPlane.Children.Add(new IpManage());


        }
    }
}
