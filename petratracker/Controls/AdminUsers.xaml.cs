using MahApps.Metro.Controls;
using petratracker.Models;
using petratracker.Pages;
using petratracker.Utility;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace petratracker.Controls
{
    public partial class AdminUsers : UserControl
    {
        #region Private Members

        private readonly string[] _userFilterOptions = { Constants.STATUS_ALL, Constants.USER_STATUS_ACTIVE,
                                                         Constants.USER_STATUS_NONACTIVE, Constants.USER_STATUS_ONLINE,
                                                         Constants.USER_STATUS_OFFLINE};
        #endregion

        #region Public Properties

        public string[] UserFilterOptions
        {
            get { return _userFilterOptions;  }
            private set { ;  }
        }

        #endregion

        #region Constructor

        public AdminUsers()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        #endregion

        #region Event Handlers

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateGrid();
        }

        private void UserListFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void UserListFilter_Click(object sender, RoutedEventArgs e)
        {
            UpdateGrid();
        }

        private void viewUsers_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (viewUsers.SelectedItem != null)
            {
                User u = viewUsers.SelectedItem as User;
                ShowActionBarButtons((string)((SplitButton)UserListFilter).SelectedItem);
            }
        }

        private void btn_activateUser_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult rs = MessageBox.Show("Are you want to activate the selected user?", "User Update", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (rs == MessageBoxResult.Yes)
            {
                User u = viewUsers.SelectedItem as User;
                TrackerUser.ActivateUser(u);
                UpdateGrid();
            }
        }

        private void btn_deactivateUser_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult rs = MessageBox.Show("Are you want to deactivate the selected user?", "User Update", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (rs == MessageBoxResult.Yes)
            {
                User u = viewUsers.SelectedItem as User;
                TrackerUser.DeactivateUser(u);
                UpdateGrid();
            }
        }

        private void btn_deleteUser_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult rs = MessageBox.Show("Are you want to delete the selected user?", "User Update", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (rs == MessageBoxResult.Yes)
            {
                User u = viewUsers.SelectedItem as User;
                try
                {
                    TrackerUser.DeleteUser(u);
                    UpdateGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("User linked to existing records: Cannot delete user. Error: " + ex.Message,"Delete user error",MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }
        }

        private void btn_showAddUser_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            object obj = parentWindow.FindName("surrogateFlyout");
            Flyout flyout = (Flyout)obj;
            flyout.ClosingFinished += flyout_ClosingFinished;
            flyout.Content = new AddUser(true);
            flyout.IsOpen = !flyout.IsOpen;
        }

        private void viewUsers_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            User u = viewUsers.SelectedItem as User;

            Window parentWindow = Window.GetWindow(this);
            object obj = parentWindow.FindName("surrogateFlyout");
            Flyout flyout = (Flyout)obj;
            flyout.ClosingFinished += flyout_ClosingFinished;
            flyout.Content = new EditUser(u,true);
            flyout.IsOpen = !flyout.IsOpen;
        }

        private void flyout_ClosingFinished(object sender, RoutedEventArgs e)
        {
            UpdateGrid();
        }

        private void viewUsers_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string[] headers = { "first_name", "last_name", "last_login"  };

            if (!headers.Contains(e.Column.Header.ToString()))
            {
                e.Cancel = true;
            }

            if (e.Column.Header.ToString().Equals("first_name"))
            {
                e.Column.Header = "First Name";
            }

            if (e.Column.Header.ToString().Equals("last_name"))
            {
                e.Column.Header = "Last Name";
            }

            if (e.Column.Header.ToString().Equals("last_login"))
            {
                e.Column.Header = "Last Login";
            }
        }

        #endregion

        #region Private Helper Methods

        private void UpdateGrid()
        {
            string filter = (string)((SplitButton)UserListFilter).SelectedItem;

            // Get items
            if (filter == Constants.STATUS_ALL) { viewUsers.ItemsSource = Models.TrackerUser.GetUsers(); }
            if (filter == Constants.USER_STATUS_ACTIVE) { viewUsers.ItemsSource = Models.TrackerUser.GetActiveUsers(); }
            if (filter == Constants.USER_STATUS_NONACTIVE) { viewUsers.ItemsSource = Models.TrackerUser.GetNonActiveUsers(); }
            if (filter == Constants.USER_STATUS_ONLINE) { viewUsers.ItemsSource = Models.TrackerUser.GetOnlineUsers(); }
            if (filter == Constants.USER_STATUS_OFFLINE) { viewUsers.ItemsSource = Models.TrackerUser.GetOfflineUsers(); }

            lbl_userCount.Content = string.Format("{0} Users", viewUsers.Items.Count);

            // Highlight items if checked.
            if (this.chx_userfilter.IsChecked == true)
            {
                ShowActionBarButtons(filter);
                viewUsers.SelectAll();
            }
            else
            {
                ShowActionBarButtons();
                viewUsers.UnselectAll();
            }
        }

        private bool ShowActionBarButtons(string filter = "")
        {
            bool activebuttons = false;

            btn_deleteUser.Visibility = Visibility.Collapsed;
            btn_activateUser.Visibility = Visibility.Collapsed;
            btn_deactivateUser.Visibility = Visibility.Collapsed;
            
                                                         
            btn_deleteUser.Visibility = Visibility.Visible;

            if (filter == Constants.STATUS_ALL || filter == Constants.USER_STATUS_ONLINE || filter == Constants.USER_STATUS_OFFLINE)
            {
                btn_activateUser.Visibility = Visibility.Visible;
                btn_deactivateUser.Visibility = Visibility.Visible;
                activebuttons = true;
            }
            else if (filter == Constants.USER_STATUS_ACTIVE)
            {
                btn_deactivateUser.Visibility = Visibility.Visible;
                activebuttons = true;
            }
            else if (filter == Constants.USER_STATUS_NONACTIVE)
            {
                btn_activateUser.Visibility = Visibility.Visible;
                activebuttons = true;
            }
     

            if (activebuttons)
            {
                actionBar.Visibility = Visibility.Visible;
            }
            else
            {
                actionBar.Visibility = Visibility.Collapsed;
            }

            return activebuttons;
        }

        #endregion
    }
}
