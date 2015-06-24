using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using petratracker.Models;
using MahApps.Metro;

using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace petratracker
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
        private User currentUser;
        private TrackerDataContext trackerDB = (App.Current as App).TrackerDBo;

        private string[] adminRoles = { "Super User", "Administrator" };


		public MainWindow()
		{
            ThemeManager.ChangeAppTheme(Application.Current, "BaseDark");

			this.InitializeComponent();
                        
            currentUser = trackerDB.Users.Single(p => p.username == Properties.Settings.Default.username);
   
            this.lbl_name.Text = String.Format("{0} {1} ({2})", this.currentUser.first_name, this.currentUser.last_name, this.currentUser.Role.role1);

            if (adminRoles.Contains(this.currentUser.Role.role1))
            {
                this.ncAdmin.Visibility = System.Windows.Visibility.Visible;
            }
        }
     
        private void NavigationControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var tab = ((Label)sender).Name.ToString(); ;
            
            this.ncAdmin.Foreground = (tab.Equals("ncAdmin")) ? (Brush)Application.Current.FindResource("SelectedTitle") : (Brush)Application.Current.FindResource("UnSelectedTitle");
            this.ncHome.Foreground = (tab.Equals("ncHome")) ? (Brush)Application.Current.FindResource("SelectedTitle") : (Brush)Application.Current.FindResource("UnSelectedTitle");
            this.ncPayments.Foreground = (tab.Equals("ncPayments")) ? (Brush)Application.Current.FindResource("SelectedTitle") : (Brush)Application.Current.FindResource("UnSelectedTitle");
            this.ncReports.Foreground = (tab.Equals("ncReports")) ? (Brush)Application.Current.FindResource("SelectedTitle") : (Brush)Application.Current.FindResource("UnSelectedTitle");
            this.ncSchedules.Foreground = (tab.Equals("ncSchedules")) ? (Brush)Application.Current.FindResource("SelectedTitle") : (Brush)Application.Current.FindResource("UnSelectedTitle");

            this.PageHolder.NavigationService.Navigate(new Uri("pages/" + ((Label)sender).Tag.ToString(), UriKind.Relative));
        }

        private void notifications_Click(object sender, RoutedEventArgs e)
        {
            var flyout = this.notificationsFlyout as Flyout;
            if (flyout == null)
                return;
            flyout.IsOpen = !flyout.IsOpen;
        }

        
        private async void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;

            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Quit",
                NegativeButtonText = "Cancel",
                AnimateShow = true,
                AnimateHide = false
            };

            var result = await this.ShowMessageAsync("Quit application?",
                                                     "Sure you want to quit application?",
                                                      MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (result == MessageDialogResult.Affirmative)
                Application.Current.Shutdown();
        }

	}
}
