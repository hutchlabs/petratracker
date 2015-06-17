using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Windows.Controls;

namespace petratracker.Data
{
    class connection
    {
        private string connString = null;

        public connection()
        {
            try
            {
                if (File.Exists(Environment.CurrentDirectory + "/connection.config"))
                {
                    connString = File.ReadAllText(Environment.CurrentDirectory + "/connection.config");
                }
                else
                { 
                    //Config file not found
                    MessageBox.Show("Config file not found.");
                }
            
            }
            catch(Exception)
            {
                //Throw exeception message and record in error log
            }
        }

        public bool chkConnection()
        {
            bool connState = false;

            try
            {

                MySqlConnection mycon = null;


                if (File.Exists(Environment.CurrentDirectory + "/connection.config"))
                {
                    mycon = new MySqlConnection(File.ReadAllText(Environment.CurrentDirectory + "/connection.config"));
                    mycon.Open();
                    if (mycon.State == ConnectionState.Open)
                    {
                        mycon.Close();
                        connState = true;
                    }

                }
                else 
                {
                    MessageBox.Show("Connection file was not found.","Config. Required",MessageBoxButton.OK,MessageBoxImage.Exclamation);
                }

            }
            catch (Exception)
            {
              
            }

            return connState;
        }

        public void executeCmdToCombo(string myCmd, ComboBox myCombo, String displayField)
        {
            try
            {
                myCmd = myCmd.Replace("'", "\'");
                myCombo.Items.Clear();
                using (MySqlConnection connection = new MySqlConnection(connString))
                {
                    MySqlCommand command = new MySqlCommand(myCmd, connection);
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        myCombo.Items.Add(reader[displayField]);
                    }
                    // always call Close when done reading.
                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception) { }
        }

        public bool executeCmd(string myCmd)
        {
            bool res = false;
            myCmd = myCmd.Replace("'", "\'");
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connString))
                {
                    MySqlCommand command = new MySqlCommand(myCmd, connection);
                    connection.Open();
                    int myRes = command.ExecuteNonQuery();

                    if (myRes > 0)
                    {
                        MessageBox.Show("Operation Completed Successfully.", "Operation Compete", MessageBoxButton.OK, MessageBoxImage.Information);
                        res = true;
                    }
                    else
                    {
                        MessageBox.Show("Operation Failed to Complete.", "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    connection.Close();
                }
            }
            catch (Exception rt) { MessageBox.Show(rt.Message); }
            return res;
        }

        public int executeCmdCount(String query)
        {
            int count = -1;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connString))
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    connection.Open();
                    count = int.Parse(cmd.ExecuteScalar() + "");
                    connection.Close();
                    return count;
                }              
            }
            catch (Exception rt) 
            {
                MessageBox.Show(rt.Message); 
            }

            return count;
        }

    }
}
