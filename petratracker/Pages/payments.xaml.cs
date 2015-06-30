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
            try
            {
                string tab = ((Label)sender).Name.ToString();

                this.TabSubs.Foreground = (tab.Equals("TabSubs")) ? (Brush)Application.Current.FindResource("SelectedTitle") : (Brush)Application.Current.FindResource("UnSelectedTitle");
                this.TabTran.Foreground = (tab.Equals("TabTran")) ? (Brush)Application.Current.FindResource("SelectedTitle") : (Brush)Application.Current.FindResource("UnSelectedTitle");
                this.TabRedm.Foreground = (tab.Equals("TabRedm")) ? (Brush)Application.Current.FindResource("SelectedTitle") : (Brush)Application.Current.FindResource("UnSelectedTitle");

                this.SubsContentbar.Visibility = (tab.Equals("TabSubs")) ? Visibility.Visible : Visibility.Collapsed;
                this.TranContentbar.Visibility = (tab.Equals("TabTran")) ? Visibility.Visible : Visibility.Collapsed;
                this.RedmContentbar.Visibility = (tab.Equals("TabRedm")) ? Visibility.Visible : Visibility.Collapsed;
            }
            catch(Exception tabErr)
            {
                MessageBox.Show(tabErr.Message);
            }
        }

        private void SubsContentbarMenu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            string menuitem = ((Label)sender).Name.ToString();
            this.btnViewJobs.Foreground = (menuitem.Equals("btnViewJobs")) ? (Brush)Application.Current.FindResource("SelectedTitle") : (Brush)Application.Current.FindResource("UnSelectedMenu");
            this.btnAddSubs.Foreground = (menuitem.Equals("btnAddSubs")) ? (Brush)Application.Current.FindResource("SelectedTitle") : (Brush)Application.Current.FindResource("UnSelectedMenu");
            
            if (menuitem.Equals("btnAddSubs"))
            {
                this.SubsPageHolder.NavigationService.Navigate(new Uri("pages/uploadDeal.xaml", UriKind.Relative));
            }
            else if (menuitem.Equals("btnViewJobs"))
            {
                this.SubsPageHolder.NavigationService.Navigate(new Uri("pages/jobs.xaml", UriKind.Relative));
            }
        }
    }
}
