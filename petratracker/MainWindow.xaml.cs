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

using petratracker.Models;

namespace petratracker
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        private User currentUser;

		public MainWindow()
		{
			this.InitializeComponent();
            this.currentUser = new User("dhutchful@gmail.com", "dogdog");
            this.lbl_username.Content = "David Hutchful";
		}

        public MainWindow(User u) : this()
        {
            this.currentUser = u;
        }

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
      
        }
           

        Data.connection accessDB = new Data.connection();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewUser openUser = new NewUser();
            openUser.ShowDialog();
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


        // Top bar buttons
        private void plus_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ((Image)sender).Visibility = System.Windows.Visibility.Collapsed;

            if (this.WindowState.Equals(System.Windows.WindowState.Maximized))
            {
                win_max.Visibility = System.Windows.Visibility.Visible;             
              this.WindowState = System.Windows.WindowState.Normal;
            }
            else
            {
                win_restore.Visibility = System.Windows.Visibility.Visible;             
                this.WindowState = System.Windows.WindowState.Maximized;
            }
        }

        private void minus_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void cross_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to exit?",
                                     "Exit Tracker Application",
                                     System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirmResult == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        // Allow window to drag
        private Point startPoint;
        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(this);
        }


        private void Window_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point newPoint = e.GetPosition(this);
            if (e.LeftButton == MouseButtonState.Pressed && (Math.Abs(newPoint.X - startPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(newPoint.Y - startPoint.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                this.DragMove();
            }
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
