using petratracker.Models;
using System;
using System.Windows;

namespace petratracker
{
    public partial class ConfigWindow : Window
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
            try
            {               
                char[] charSeparators = new char[] { ';' };

                if (Properties.Settings.Default.database_tracker != string.Empty)
                {
                    String conStr = Properties.Settings.Default.database_tracker;           
                    string[] results = conStr.Split(charSeparators);
                    txtTrackerDataSource.Text= results[0].Substring(results[0].IndexOf('=') + 1);
                }              
            }
            catch(Exception configFileError)
            {
                MessageBox.Show(configFileError.Message);
            }
        }

        private void btnTrackerTestConnection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate_tracker_entries())
                {
                    String tconStr = "Data Source=" + txtTrackerDataSource.Text + ";Initial Catalog=Petra_tracker;Integrated Security=True";
                    String pconStr = "Data Source=" + txtTrackerDataSource.Text + ";Initial Catalog=Petra5;Integrated Security=True";
                    String ptasStr = "Data Source=" + txtTrackerDataSource.Text + ";Initial Catalog=PTASDB;Integrated Security=True";

                    TrackerDataContext tdc = new TrackerDataContext(tconStr);
                    MicrogenDataContext mdc = new MicrogenDataContext(pconStr);
                    PTASDataContext pdc = new PTASDataContext(ptasStr);

                    bool t=false;
                    bool m=false;
                    bool p=false;

                    t = tdc.DatabaseExists();
                    m = mdc.DatabaseExists();
                    p = pdc.DatabaseExists();

                    if (m && t && p)
                    {
                        MessageBox.Show("Connection to databases were successful.", "Connection Successfull", MessageBoxButton.OK, MessageBoxImage.Information);
                        Properties.Settings.Default.database_tracker = tconStr;
                        Properties.Settings.Default.database_microgen = pconStr;
                        Properties.Settings.Default.database_ptas = ptasStr;

                        Properties.Settings.Default.Save();

                        (App.Current as App).TrackerDBo = tdc;
                        (App.Current as App).MicrogenDBo = mdc;
                        (App.Current as App).PTASDBo = pdc;


                        if (_exitToLoginWindow)
                        {
                            (new LoginWindow()).Show();
                        }
                        this.Close();
                    }
                    else if (m)
                    {
                        MessageBox.Show("Connection to Tracker DB failed.", "Connection Status", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (t)
                    {
                        MessageBox.Show("Connection to Microgen failed.", "Connection Status", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (p)
                    {
                        MessageBox.Show("Connection to PTAS DB failed.", "Connection Status", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Connection could not be established with Tracker. Please enter correct details.", "Error in connection", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection could not be established with databases. "+ex.GetBaseException().ToString(), "Error in connection", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        #endregion

        #region Private Helper Methods

        private bool validate_tracker_entries()
        {
            bool valid = false;

            if (txtTrackerDataSource.Text == string.Empty)
            {
                MessageBox.Show("Please specify the Data Source.");
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
