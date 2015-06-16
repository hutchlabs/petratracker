﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace petratracker
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			this.InitializeComponent();

			// Insert code required on object creation below this point.
		}

        Data.connection accessDB = new Data.connection();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewUser openUser = new NewUser();
            openUser.ShowDialog();
        }

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
           

        }
      
        private void btnConfigureConnection_Click(object sender, RoutedEventArgs e)
        {
            Views.DatabaseConnSetup openConfig = new Views.DatabaseConnSetup();
            openConfig.ShowDialog();
            
        }

        private void btnNewDepartment_Click(object sender, RoutedEventArgs e)
        {
            Views.NewDepartment openDepartment = new Views.NewDepartment();
            openDepartment.ShowDialog();
         }

        private void btnNewRole_Click(object sender, RoutedEventArgs e)
        {
            Views.NewRole openRole = new Views.NewRole();
            openRole.ShowDialog();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            string get_users = "select * from view_users";
            viewUsers.ItemsSource = accessDB.ExecuteCmdToDataGrid(get_users).DefaultView;
        }

        private void viewUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Data.DataRowView  row = (System.Data.DataRowView)viewUsers.SelectedItems[0];
            NewUser editUser = new NewUser();
            editUser.txtUserID.Text = row["ID"].ToString();
            editUser.btnSave.Content = "Update";
            editUser.ShowDialog();
           
        }
	}
}