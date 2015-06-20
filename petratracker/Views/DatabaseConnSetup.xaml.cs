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
using MySql.Data;

namespace petratracker.Views
{
    /// <summary>
    /// Interaction logic for DatabaseConnSetup.xaml
    /// </summary>
    public partial class DatabaseConnSetup : Window
    {
        public DatabaseConnSetup()
        {
            InitializeComponent();
        }



        private bool validate_tracker_entries()
        {
            bool valid = false;

            if (txtTrackerDataSource.Text == string.Empty)
            {
                MessageBox.Show("Please specify the Data Source.");
                txtTrackerDataSource.Focus();
            }
            else if (txtTrackerUsername.Text == string.Empty)
            {
                MessageBox.Show("Please specify the Username.");
                txtTrackerUsername.Focus();
            }
            else
            {
                valid = true;
            }

            return valid;
        }

        private bool validate_microgen_entries()
        {
            bool valid = false;

            if (txtMicrogenDataSource.Text == string.Empty)
            {
                MessageBox.Show("Please specify the Data Source.");
                txtMicrogenDataSource.Focus();
            }
            else if (txtMicrogenUsername.Text == string.Empty)
            {
                MessageBox.Show("Please specify the Username.");
                txtMicrogenUsername.Focus();
            }
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

                if (File.Exists(Environment.CurrentDirectory + "/tracker_connection.config"))
                {

                    //PetraTracker Data String
                    String conStr = File.ReadAllText(Environment.CurrentDirectory + "/tracker_connection.config");                  
                    string[] results = conStr.Split(charSeparators);
                    txtTrackerDataSource.Text= results[0].Substring(results[0].IndexOf('=') + 2);
                    txtTrackerUsername.Text = results[1].Substring(results[1].IndexOf('=') + 2);
                    txtTrackerPassword.Text = results[2].Substring(results[2].IndexOf('=') + 2);
                    txtTrackerDatabase.Text = results[3].Substring(results[3].IndexOf('=') + 2);
                    
                }

                if(File.Exists(Environment.CurrentDirectory + "/microgen_connection.config"))
                {
                    //Microgen Data String
                    String conStr = File.ReadAllText(Environment.CurrentDirectory + "/microgen_connection.config");
                    
                    string [] results = conStr.Split(charSeparators);
                    txtMicrogenDataSource.Text = results[0].Substring(results[0].IndexOf('=') + 2);
                    txtMicrogenUsername.Text = results[1].Substring(results[1].IndexOf('=') + 2);
                    txtMicrogenPassword.Text = results[2].Substring(results[2].IndexOf('=') + 2);
                    txtMicrogenDatabase.Text = results[3].Substring(results[3].IndexOf('=') + 2);
                } 
                else
                {
                    MessageBox.Show("Microgen config file not found.");
                }
            }
            catch(Exception configFileError)
            {
                MessageBox.Show(configFileError.Message);
                //Log error
            }
        }

        private void btnMicrogenTestConnection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate_microgen_entries())
                {
                    String conStr = "Data Source =" + txtMicrogenDataSource.Text + ";User ID =" + txtMicrogenUsername.Text + " ;Password =" + txtMicrogenPassword.Text + ";Initial Catalog =" + txtMicrogenDatabase.Text + "";
                    File.WriteAllText(Environment.CurrentDirectory + "/microgen_connection.config", conStr);

                    Data.connection openConn = new Data.connection();

                    if (openConn.chkMicrogenConnection())
                    {
                        MessageBox.Show("Connection to Microgen was successfull, please restart application.", "Operation Successfull", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    else
                    {
                        MessageBox.Show("Connection could not be established with Microgen.", "Error in connection", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception)
            {
                //record error
            }
        }

        private void btnTrackerTestConnection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate_tracker_entries())
                {
                    String conStr = "Data Source=" + txtTrackerDataSource.Text + ";User ID=" + txtTrackerUsername.Text + ";Password=" + txtTrackerPassword.Text + ";Initial Catalog=" + txtTrackerDatabase.Text + "";
                    File.WriteAllText(Environment.CurrentDirectory + "/tracker_connection.config", conStr);

                    Data.connection openConn = new Data.connection();

                    if (openConn.chkTrackerConnection())
                    {
                        MessageBox.Show("Connection to Tracker was successfull, please restart application.", "Operation Successfull", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    else
                    {
                        MessageBox.Show("Connection could not be established with Tracker.", "Error in connection", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception)
            {
                //log error
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Pages.uploadDeal openUpload = new Pages.uploadDeal();
            openUpload.ShowDialog();
        }

       


    }
}
