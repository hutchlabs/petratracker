using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Diagnostics;

namespace petratracker.Data
{
    class connection
    {
        private string connString = null;
        public DataTable mytable = new DataTable();

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
                    System.Windows.MessageBox.Show("Config file not found.");
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
                    System.Windows.MessageBox.Show("Connection file was not found.", "Config. Required", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }

            }
            catch (Exception)
            {
              
            }

            return connState;
        }


        

        public void executeCmdToCombo(string myCmd, System.Windows.Controls.ComboBox myCombo, String displayField)
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
                        System.Windows.MessageBox.Show("Operation Completed Successfully.", "Operation Compete", MessageBoxButton.OK, MessageBoxImage.Information);
                        res = true;
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Operation Failed to Complete.", "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    connection.Close();
                }
            }
            catch (Exception rt) { System.Windows.MessageBox.Show(rt.Message); }
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
                System.Windows.MessageBox.Show(rt.Message); 
            }

            return count;
        }

        public DataTable ExecuteCmdToDataGrid(String myCmd)
        {
            try
            {
                myCmd = myCmd.Replace("'", "\'");
                mytable = new DataTable();


                using (MySqlConnection connection = new MySqlConnection(connString))
                {

                    MySqlDataAdapter myadpt = new MySqlDataAdapter(myCmd, connection);
                    myadpt.FillLoadOption = LoadOption.OverwriteChanges;
                    MySqlCommandBuilder mycombuild = new MySqlCommandBuilder(myadpt);

                    // Populate a new data table and bind it to the BindingSource.

                    mytable.Clear();


                    mytable.Locale = System.Globalization.CultureInfo.InvariantCulture;


                    myadpt.Fill(mytable);

                    

                    connection.Close();

                }
            }
            catch (Exception e) { }
            return mytable;
        }

public bool chkMicrogenConnection()
        {
            bool connState = false;

            try
            {

                SqlConnection mycon = null;


                if (File.Exists(Environment.CurrentDirectory + "/microgen_connection.config"))
                {
                    mycon = new SqlConnection(File.ReadAllText(Environment.CurrentDirectory + "/microgen_connection.config"));
                    mycon.Open();
                    if (mycon.State == ConnectionState.Open)
                    {
                        mycon.Close();
                        connState = true;
                    }

                }
                else
                {
                    System.Windows.MessageBox.Show("Connection file was not found.", "Config. Required", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }

            }
            catch (Exception)
            {

            }

            return connState;
        }
    }
}
