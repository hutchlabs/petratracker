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
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using petratracker.Pages;

namespace petratracker
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
        private User currentUser;
        private TrackerDataContext trackerDB = (App.Current as App).TrackerDBo;
        private TrackerNotification trackerNF = new TrackerNotification();

        private string[] adminRoles = { "Super User", "Administrator" };

		public MainWindow()
		{
            ThemeManager.ChangeAppTheme(Application.Current, "BaseDark");

			InitializeComponent();

            currentUser = trackerDB.Users.Single(p => p.username == Properties.Settings.Default.username);
   
            this.lbl_name.Text = String.Format("{0} {1} ({2})", this.currentUser.first_name, 
                                                                this.currentUser.last_name, 
                                                                this.currentUser.Role.role1);

            var dueTime = TimeSpan.FromSeconds(0);
            var interval = TimeSpan.FromSeconds(30);
            DoPeriodicWorkAsync(dueTime, interval, CancellationToken.None);          
            
            if (adminRoles.Contains(this.currentUser.Role.role1))
            {
                this.ncAdmin.Visibility = System.Windows.Visibility.Visible;
            }
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
            {
                Application.Current.Shutdown();
            }
        }


        private async Task DoPeriodicWorkAsync(TimeSpan dueTime,   TimeSpan interval, CancellationToken token)
        {
            // Initial wait time before we begin the periodic loop.
            if (dueTime > TimeSpan.Zero)
                await Task.Delay(dueTime, token);

            // Repeat this loop until cancelled.
            while (!token.IsCancellationRequested)
            {
                // Update Notifications
                lbl_notifications.Text = trackerNF.GetNotificationStatus();
                lbl_notifications.ToolTip = trackerNF.GetNotificationToolTip();
                lv_notifications.ItemsSource = trackerNF.GetNotifications();

                // Wait to repeat again.
                if (interval > TimeSpan.Zero)
                    await Task.Delay(interval, token);
            }
        }

     
        private void NavigationControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // TODO: Change this to Tab Control
            var tab = ((Label)sender).Name.ToString(); ;
            
            this.ncAdmin.Foreground = (tab.Equals("ncAdmin")) ? (Brush)Application.Current.FindResource("SelectedTitle") : (Brush)Application.Current.FindResource("UnSelectedTitle");
            this.ncHome.Foreground = (tab.Equals("ncHome")) ? (Brush)Application.Current.FindResource("SelectedTitle") : (Brush)Application.Current.FindResource("UnSelectedTitle");
            this.ncPayments.Foreground = (tab.Equals("ncPayments")) ? (Brush)Application.Current.FindResource("SelectedTitle") : (Brush)Application.Current.FindResource("UnSelectedTitle");
            this.ncReports.Foreground = (tab.Equals("ncReports")) ? (Brush)Application.Current.FindResource("SelectedTitle") : (Brush)Application.Current.FindResource("UnSelectedTitle");
            this.ncSchedules.Foreground = (tab.Equals("ncSchedules")) ? (Brush)Application.Current.FindResource("SelectedTitle") : (Brush)Application.Current.FindResource("UnSelectedTitle");

            this.PageHolder.NavigationService.Navigate(new Uri("pages/" + ((Label)sender).Tag.ToString(), UriKind.Relative));
        }

        // Show notificaitons fly out
        private void Notifications_Click(object sender, RoutedEventArgs e)
        {
            var flyout = this.notificationsFlyout as Flyout;
            if (flyout == null)
                return;
            flyout.IsOpen = !flyout.IsOpen;
        }

        // Handle Nofication click
        private void lv_notifications_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = lv_notifications.SelectedItem as Notification;

            if(item !=null)
            {
                ApproveRejectSubscription frm = new ApproveRejectSubscription(item.id);
                frm.ShowDialog();         
                //MessageBox.Show("Approve this noticifcation " + item.id);
            }
        }

	}
}
