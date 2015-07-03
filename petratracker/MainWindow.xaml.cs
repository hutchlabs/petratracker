using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using petratracker.Models;
using petratracker.Pages;
using petratracker.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace petratracker
{
	public partial class MainWindow : MetroWindow, INotifyPropertyChanged
	{
        #region Private Members

        private string _selectedAccent;
        private string _selectedTheme;
        private bool _animateOnPositionChange = true;

        #endregion

        #region Public Members
        
        public event PropertyChangedEventHandler PropertyChanged;
        
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

        public Visibility ShowJobsTab
        {
            get
            {
                return (TrackerUser.IsCurrentUserParser()) ? Visibility.Visible : Visibility.Collapsed;
            }
            private set { }
        }

        public Visibility ShowSettingsTab
        {
            get
            {
                return (TrackerUser.IsCurrentUserSuperAdmin()) ? Visibility.Visible : Visibility.Collapsed;
            }
            private set { }
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

        public bool AnimateOnPositionChange
        {
            get
            {
                return _animateOnPositionChange;
            }
            set
            {
                if (Equals(_animateOnPositionChange, value)) return;
                _animateOnPositionChange = value;
                RaisePropertyChanged("AnimateOnPositionChange");
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

            StartScheduleWorkflowService();

            this.lbl_name.Text = TrackerUser.GetCurrentUserTitle();
        }

        #endregion

        #region Public Methods

        public bool UpdateNotifications()
        {
            // Update Notifications
            lbl_notifications.Text = TrackerNotification.GetNotificationStatus();
            lbl_notifications.ToolTip = TrackerNotification.GetNotificationToolTip();
            lv_notifications.ItemsSource = TrackerNotification.GetNotifications();
            return true;
        }

        #endregion

        #region Protected Methods

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
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
                SubscriptionsApproveReject frm = new SubscriptionsApproveReject(item.id);
                frm.ShowDialog();         
                //MessageBox.Show("Approve this noticifcation " + item.id);
            }
        }



        #endregion

        #region Private Helper Methods

        private void StartScheduleWorkflowService()
        {
            TrackerSchedule.InitiateScheduleWorkFlow();
        }

        private async void StartNotificationService()
        {
            var dueTime = TimeSpan.FromSeconds(0);
            var interval = TimeSpan.FromSeconds(30);
            await Utils.DoPeriodicWorkAsync(new Func<bool>(UpdateNotifications),dueTime,interval,CancellationToken.None);
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
