using System;
using System.Collections.Generic;
using System.Linq;
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
using petratracker.Controls;


namespace petratracker
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        private User currentUser;

        private string[] adminRoles = { "Super User", "Administrator" };

        Data.connection accessDB = new Data.connection();

        TrackerDataContext trackerDB = new TrackerDataContext();

		public MainWindow()
		{
			this.InitializeComponent();
            
            Properties.Settings.Default.username = "dhutchful@gmail.com";
            
            currentUser = trackerDB.Users.Single(p => p.username == Properties.Settings.Default.username);
   
            this.lbl_name.Content = this.currentUser.first_name + " " + this.currentUser.last_name;
            this.lbl_role.Content = this.currentUser.Role.role1;

            if (adminRoles.Contains(this.currentUser.Role.role1))
            {
                this.ncAdmin.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
        }
           
        private void NavigationControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.PageHolder.NavigationService.Navigate(new Uri("pages/" + ((NavigationControl)sender).ItemUri, UriKind.Relative));
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

        private void notifications_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = (sender as Button);
            (sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;
        } 
        
	}
}
