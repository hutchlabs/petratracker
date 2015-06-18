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



        private bool validate_entries()
        {
            bool valid = false;

            if (txtServer.Text == string.Empty)
            {
                MessageBox.Show("Please specify the Server Name.");
                txtServer.Focus();
            }
            else if (txtUsername.Text == string.Empty)
            {
                MessageBox.Show("Please specify the Username.");
                txtUsername.Focus();
            }
            else
            {
                valid = true;
            }

            return valid;
        }

        private bool validate_entries_1()
        {
            bool valid = false;

            if (txtServer1.Text == string.Empty)
            {
                MessageBox.Show("Please specify the Server Name.");
                txtServer1.Focus();
            }
            else if (txtUsername1.Text == string.Empty)
            {
                MessageBox.Show("Please specify the Username.");
                txtUsername1.Focus();
            }
            else
            {
                valid = true;
            }

            return valid;
        }

        private void btnTestConnection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate_entries())
                {
                    String conStr = "datasource = " + txtServer.Text + ";username = " + txtUsername.Text + " ;password = " + txtPassword.Text + " ;database = " + txtDatabase.Text + " ;port = " + txtPortNumber.Text + "";
                    File.WriteAllText(Environment.CurrentDirectory + "/connection.config", conStr);

                    Data.connection openConn = new Data.connection();

                    if (openConn.chkConnection())
                    {
                        MessageBox.Show("Connection successfull, please restart application.", "Operation Successfull", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    else
                    {
                        MessageBox.Show("Connection could not be established.", "Error in connection", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void frmDatabaseConnection_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (File.Exists(Environment.CurrentDirectory + "/connection.config"))
                {

                    //PetraTracker Data String
                    String conStr = File.ReadAllText(Environment.CurrentDirectory + "/connection.config");
                    char[] charSeparators = new char[] { ';' };
                    string[] results = conStr.Split(charSeparators);
                    txtServer.Text = results[0].Substring(results[0].IndexOf('=') + 2);
                    txtUsername.Text = results[1].Substring(results[1].IndexOf('=') + 2);
                    txtPassword.Text = results[2].Substring(results[2].IndexOf('=') + 2);
                    txtDatabase.Text = results[3].Substring(results[3].IndexOf('=') + 2);
                    txtPortNumber.Text = results[4].Substring(results[4].IndexOf('=') + 2);

                    //Microgen Data String
                    String conStr_1 = File.ReadAllText(Environment.CurrentDirectory + "/connection1.config");
                    
                    string [] results_1 = conStr.Split(charSeparators);
                    txtServer1.Text = results_1[0].Substring(results_1[0].IndexOf('=') + 2);
                    txtUsername1.Text = results_1[1].Substring(results_1[1].IndexOf('=') + 2);
                    txtPassword1.Text = results_1[2].Substring(results_1[2].IndexOf('=') + 2);
                    txtDatabase1.Text = results_1[3].Substring(results_1[3].IndexOf('=') + 2);
                  


                }
                else
                {
                    MessageBox.Show("Connection file not found.");
                }
            }
            catch(Exception)
            {
            
            }
        }

       


    }
}
