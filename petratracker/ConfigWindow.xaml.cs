using MahApps.Metro.Controls;
using petratracker.Models;
using petratracker.Utility;
using System;
using System.Collections.Generic;
using System.Windows;

namespace petratracker
{
    public partial class ConfigWindow : MetroWindow
    {
        #region Private Members

        private App _app;
        private bool _exitToLoginWindow = false;

        #endregion
        
        #region Public Properties

        public IEnumerable<ComboBoxPairs> Servers
        {
            private set { ; }
            get { return Database.GetSQLServers(); }
        }

        #endregion

        #region Constructor

        public ConfigWindow(App app, bool showLogin=false)
        {
            this.DataContext = this;
            InitializeComponent();
            _app = app;
            _exitToLoginWindow = showLogin;
        }

        #endregion

        #region Event Handlers

        private void frmDatabaseConnection_Loaded(object sender, RoutedEventArgs e)
        {
            cbx_TrackerDataSource.SelectedIndex = 0;
        }

        private void btnTrackerTestConnection_Click(object sender, RoutedEventArgs e)
        {
            spinner.IsActive = true;

            try
            {
                if (Database.Setup(((ComboBoxPairs)cbx_TrackerDataSource.SelectedItem)._Key))
                {
                    spinner.IsActive = false;

                    //MessageBox.Show("Connection to databases were successful.", "Connection Successful", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (_exitToLoginWindow)
                    {
                        (new LoginWindow(_app)).Show();
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

        #endregion
    }
}
