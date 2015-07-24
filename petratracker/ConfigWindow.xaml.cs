using MahApps.Metro.Controls;
using petratracker.Models;
using System;
using System.Windows;

namespace petratracker
{
    public partial class ConfigWindow : MetroWindow
    {
        #region Private Members

        private bool _exitToLoginWindow = false;

        #endregion

        #region Constructor

        public ConfigWindow(bool showLogin=false)
        {
            InitializeComponent();
            _exitToLoginWindow = showLogin;
        }

        #endregion

        #region Event Handlers

        private void frmDatabaseConnection_Loaded(object sender, RoutedEventArgs e)
        {
            txtTrackerDataSource.Text = TrackerDB.GetConnectionString();
        }

        private void btnTrackerTestConnection_Click(object sender, RoutedEventArgs e)
        {
            if (validate_tracker_entries())
            {
                spinner.IsActive = true;

                try
                {
                    if (TrackerDB.Setup(txtTrackerDataSource.Text))
                    {
                        spinner.IsActive = false;

                        MessageBox.Show("Connection to databases were successful.", "Connection Successful", MessageBoxButton.OK, MessageBoxImage.Information);

                        if (_exitToLoginWindow)
                        {
                            (new LoginWindow()).Show();
                        }
                        this.Close();
                    }
                }
                catch (Exceptions.TrackerDBConnectionException ex)
                {
                    spinner.IsActive = false;
                    MessageBox.Show(ex.Message, "Error in connection", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch(Exception ex)
                {
                    spinner.IsActive = false;
                     MessageBox.Show("Database setup error: " + ex.GetBaseException().ToString(), "Error in connection", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }       
        }

        #endregion

        #region Private Helper Methods

        private bool validate_tracker_entries()
        {
            bool valid = false;

            if (txtTrackerDataSource.Text == string.Empty)
            {
                MessageBox.Show("Please specify the Database server name.");
                txtTrackerDataSource.Focus();
            }
            else
            {
                valid = true;
            }

            return valid;
        }

        #endregion
    }
}
