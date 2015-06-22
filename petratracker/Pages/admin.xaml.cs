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
using System.Windows.Navigation;
using System.Windows.Shapes;
using petratracker.Models;

namespace petratracker.Pages
{
    /// <summary>
    /// Interaction logic for admin.xaml
    /// </summary>
    public partial class admin : Page
    {

        Data.connection accessDB = new Data.connection();
        TrackerDataContext trackerDB = new TrackerDataContext();

        public admin()
        {
            InitializeComponent();
            var currentUser = trackerDB.Users.Single(p => p.username == Properties.Settings.Default.username);

            if (!currentUser.Role.role1.Equals("Super Admin"))
            {
                this.TabSetts.Visibility = Visibility.Collapsed;
            }
            //viewUsers.ItemsSource = User.usersInGrid();
        }

        private void Pillbar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            string tab = ((Label)sender).Name.ToString();

            this.TabUsers.Background = (tab.Equals("TabUsers")) ? (Brush)Application.Current.FindResource("GreyBackground") : Brushes.Black;
            this.TabRoles.Background = (tab.Equals("TabRoles")) ? (Brush)Application.Current.FindResource("GreyBackground") : Brushes.Black;
            this.TabSetts.Background = (tab.Equals("TabSetts")) ? (Brush)Application.Current.FindResource("GreyBackground") : Brushes.Black;

            this.UsersContentbar.Visibility = (tab.Equals("TabUsers")) ? Visibility.Visible : Visibility.Collapsed;
            this.RolesContentbar.Visibility = (tab.Equals("TabRoles")) ? Visibility.Visible : Visibility.Collapsed;
            this.SettsContentbar.Visibility = (tab.Equals("TabSetts")) ? Visibility.Visible : Visibility.Collapsed;
        }

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

        private void viewUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            try
            {

                System.Data.DataRowView row = (System.Data.DataRowView)viewUsers.SelectedItems[0];

                NewUser editUser = new NewUser();

                editUser.txtUserID.Text = row["ID"].ToString();

                editUser.btnSave.Content = "Update";

                editUser.ShowDialog();

            }
            catch (Exception viewUserError)
            {

                MessageBox.Show("Action failed to complete.\n" + viewUserError.Message);
                //Log error
            }

        }

        private void btnUploadPayment_Click(object sender, RoutedEventArgs e)
        {
            Pages.uploadDeal openUpload = new Pages.uploadDeal();
            openUpload.ShowDialog();
        }


    }
}
