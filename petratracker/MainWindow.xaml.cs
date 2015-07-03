using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using petratracker.Models;
using petratracker.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace petratracker
{
	public partial class MainWindow : MetroWindow
	{
        #region Private Members

        private string _selectedAccent;
        private string _selectedTheme;

        #endregion

        #region Public Properties

        public IEnumerable<string> AccentColorlist { get; private set; }
        public IEnumerable<string> ThemeColorlist  { get; private set; }

        public Visibility ShowAdminTab
        {
            get
            {
                return (TrackerUser.IsCurrentUserAdmin()) ? Visibility.Visible : Visibility.Collapsed;
            }
            private set {}
        }

        public string SelectedAccent
        {
            get { return _selectedAccent; }
            set
            {
                if (value == _selectedAccent)
                    return;
                _selectedAccent = value;

                ChangeAppearance();
            }
        }

        public string SelectedTheme
        {
            get { return _selectedTheme; }
            set
            {
                if (value == _selectedTheme)
                    return;
                _selectedTheme = value;

                ChangeAppearance();
            }
        }

        #endregion

        #region Constructor

        public MainWindow()
		{
			InitializeComponent();

            this.DataContext = this;

            SetAppearance();

            StartNotificationService();

            this.lbl_name.Text = TrackerUser.GetCurrentUserTitle();
        }

        #endregion

        #region Event Handlers

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

        private void Usersettings_Click(object sender, RoutedEventArgs e)
        {
            var flyout = this.settingsFlyout as Flyout;
            if (flyout == null)
                return;
            flyout.IsOpen = !flyout.IsOpen;
        }

        private void Notifications_Click(object sender, RoutedEventArgs e)
        {
            var flyout = this.notificationsFlyout as Flyout;
            if (flyout == null)
                return;
            flyout.IsOpen = !flyout.IsOpen;
        }

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

        #endregion

        #region Private Helper Methods
        
        private void StartNotificationService()
        {
            var dueTime = TimeSpan.FromSeconds(0);
            var interval = TimeSpan.FromSeconds(30);
            DoPeriodicWorkAsync(dueTime, interval, CancellationToken.None);
        }

        private async Task DoPeriodicWorkAsync(TimeSpan dueTime, TimeSpan interval, CancellationToken token)
        {
            // Initial wait time before we begin the periodic loop.
            if (dueTime > TimeSpan.Zero)
                await Task.Delay(dueTime, token);

            // Repeat this loop until cancelled.
            while (!token.IsCancellationRequested)
            {
                // Update Notifications
                lbl_notifications.Text = TrackerNotification.GetNotificationStatus();
                lbl_notifications.ToolTip = TrackerNotification.GetNotificationToolTip();
                lv_notifications.ItemsSource = TrackerNotification.GetNotifications();

                // Wait to repeat again.
                if (interval > TimeSpan.Zero)
                    await Task.Delay(interval, token);
            }
        }

        private void SetAppearance()
        {
            ThemeColorlist = ThemeManager.AppThemes.Select(a => a.Name).ToList();
            AccentColorlist = ThemeManager.Accents.Select(a => a.Name).ToList();

            _selectedTheme = TrackerUser.GetCurrentUser().theme.Trim();
            _selectedAccent = TrackerUser.GetCurrentUser().accent.Trim();
            ChangeAppearance(false);
        }

        private void ChangeAppearance(bool save=true)
        {
            // Change accent 
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            var accent = ThemeManager.GetAccent(_selectedAccent);
            ThemeManager.ChangeAppStyle(Application.Current, accent, theme.Item1);

            // Change theme
            theme = ThemeManager.DetectAppStyle(Application.Current);
            var appTheme = ThemeManager.GetAppTheme(_selectedTheme);
            ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, appTheme);

            // Save new appearance
            if(save)
            {
                TrackerUser.SaveAppearance(SelectedTheme, SelectedAccent);
            }
        }

        #endregion
	}
}
