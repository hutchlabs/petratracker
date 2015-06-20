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

namespace petratracker.Pages
{
    /// <summary>
    /// Interaction logic for payments.xaml
    /// </summary>
    public partial class payments : Page
    {
        public payments()
        {
            InitializeComponent();
        }

        private void Pillbar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            string tab = ((Label)sender).Name.ToString();

            this.TabSubs.Background = (tab.Equals("TabSubs")) ? (Brush)Application.Current.MainWindow.FindResource("GreyBackground") : Brushes.Black;
            this.TabTran.Background = (tab.Equals("TabTran")) ? (Brush)Application.Current.MainWindow.FindResource("GreyBackground") : Brushes.Black;
            this.TabRedm.Background = (tab.Equals("TabRedm")) ? (Brush)Application.Current.MainWindow.FindResource("GreyBackground") : Brushes.Black;

            this.SubsContentbar.Visibility = (tab.Equals("TabSubs")) ? Visibility.Visible : Visibility.Collapsed;
            this.TranContentbar.Visibility = (tab.Equals("TabTran")) ? Visibility.Visible : Visibility.Collapsed;
            this.RedmContentbar.Visibility = (tab.Equals("TabRedm")) ? Visibility.Visible : Visibility.Collapsed;
        }


        private void btnUploadPayment_Click(object sender, RoutedEventArgs e)
        {
            Pages.uploadDeal openUpload = new Pages.uploadDeal();
            openUpload.ShowDialog();
        }
    }
}
