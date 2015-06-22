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
using System.Windows.Shapes;
using System.IO;

using petratracker.Models;

namespace petratracker.Views
{
    /// <summary>
    /// Interaction logic for DatabaseConnSetup.xaml
    /// </summary>
    public partial class DatabaseConnSetup : Window
    {
        private bool exitToLoginWindow = false;

        public DatabaseConnSetup()
        {
            InitializeComponent();
        }

        public DatabaseConnSetup(bool showLogin) : this()
        {
            this.exitToLoginWindow = showLogin;
        }

        private bool validate_tracker_entries()
        {
            bool valid = false;

            if (txtTrackerDataSource.Text == string.Empty)
            {
                MessageBox.Show("Please specify the Data Source.");
                txtTrackerDataSource.Focus();
            }
            /*else if (txtTrackerUsername.Text == string.Empty)
            {
                MessageBox.Show("Please specify the Username.");
                txtTrackerUsername.Focus();
            }*/
            else
            {
                valid = true;
            }

            return valid;
        }       
     
        private void frmDatabaseConnection_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                
                char[] charSeparators = new char[] { ';' };

                if (Properties.Settings.Default.database != string.Empty)
                {
                    //PetraTracker Data String
                    String conStr = Properties.Settings.Default.database;           
                    string[] results = conStr.Split(charSeparators);
                    txtTrackerDataSource.Text= results[0].Substring(results[0].IndexOf('=') + 1);
                    txtTrackerDatabase.Text = results[1].Substring(results[1].IndexOf('=') + 1);                
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
                    String conStr = "Data Source=" + txtTrackerDataSource.Text + ";Initial Catalog=" + txtTrackerDatabase.Text + ";Integrated Security=True";
           
                    TrackerDataContext tdc = new TrackerDataContext(conStr);

                    if (tdc.DatabaseExists())
                    {
                        MessageBox.Show("Connection to Tracker was successful.", "Connection Successfull", MessageBoxButton.OK, MessageBoxImage.Information);
                        Properties.Settings.Default.database = conStr;
                        Properties.Settings.Default.Save();
                        (App.Current as App).TrackerDBo = tdc;

                        if (this.exitToLoginWindow)
                        {
                            (new LoginWindow()).Show();
                        }
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Connection could not be established with Tracker. Please enter correct details.", "Error in connection", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection could not be established with Tracker. "+ex.GetBaseException().ToString(), "Error in connection", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}
