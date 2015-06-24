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
        private User currentUser;
        private TrackerDataContext trackerDB = (App.Current as App).TrackerDBo;

        public admin()
        {
            InitializeComponent();
            currentUser = trackerDB.Users.Single(p => p.username == Properties.Settings.Default.username);

            if (!currentUser.Role.role1.Equals("Super User"))
            {
                this.TabSetts.Visibility = Visibility.Collapsed;
            }
            viewUsers.ItemsSource = this.GetUsers();
        }

        public IEnumerable<User> GetUsers()
        {
            return (from u in trackerDB.Users select u);
        }

        private void Pillbar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            string tab = ((Label)sender).Name.ToString();

            this.TabUsers.Foreground = (tab.Equals("TabUsers")) ? (Brush)Application.Current.FindResource("SelectedTitle") : (Brush)Application.Current.FindResource("UnSelectedTitle");
            this.TabRoles.Foreground = (tab.Equals("TabRoles")) ? (Brush)Application.Current.FindResource("SelectedTitle") : (Brush)Application.Current.FindResource("UnSelectedTitle");
            this.TabSetts.Foreground = (tab.Equals("TabSetts")) ? (Brush)Application.Current.FindResource("SelectedTitle") : (Brush)Application.Current.FindResource("UnSelectedTitle");

            this.UsersContentbar.Visibility = (tab.Equals("TabUsers")) ? Visibility.Visible : Visibility.Collapsed;
            this.RolesContentbar.Visibility = (tab.Equals("TabRoles")) ? Visibility.Visible : Visibility.Collapsed;
            this.SettsContentbar.Visibility = (tab.Equals("TabSetts")) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UsersContentbarMenu_MouseLeftButtonUp(object sender,  MouseButtonEventArgs e)
        {
            string menuitem = ((Label)sender).Name.ToString();
            this.btnAddUser.Foreground = (menuitem.Equals("btnAddUser")) ? (Brush)Application.Current.FindResource("SelectedTitle") : (Brush)Application.Current.FindResource("UnSelectedMenu");
            this.btnViewUsers.Foreground = (menuitem.Equals("btnViewUsers")) ? (Brush)Application.Current.FindResource("SelectedTitle") : (Brush)Application.Current.FindResource("UnSelectedMenu");

            if (menuitem.Equals("btnAddUser"))
            {
                NewUser openUser = new NewUser();
                openUser.ShowDialog();
                this.btnAddUser.Foreground = (Brush)Application.Current.FindResource("UnSelectedMenu");
                this.btnViewUsers.Foreground = (Brush)Application.Current.FindResource("SelectedTitle");
            }
        }


        private void btnConfigureConnection_Click(object sender, RoutedEventArgs e)
        {
            Views.DatabaseConnSetup openConfig = new Views.DatabaseConnSetup();
            openConfig.ShowDialog();
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

               /* System.Data.DataRowView row = (System.Data.DataRowView)viewUsers.SelectedItems[0];

                NewUser editUser = new NewUser();

                editUser.btnSave.Content = "Update";

                editUser.ShowDialog();
                */
            }
            catch (Exception viewUserError)
            {

                MessageBox.Show("Action failed to complete.\n" + viewUserError.Message);
                //Log error
            }

        }

      /*  private void btnUploadPayment_Click(object sender, RoutedEventArgs e)
        {
            //Pages.uploadDeal openUpload = new Pages.uploadDeal();
            //openUpload.ShowDialog();
        }*/

        private void viewUsers_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string[] hiddenHeaders = { "password", "id", "role_id", "first_login", "Payments", "Settings", 
                                       "Role", "email2", "email3", "signature", "logged_in", "middle_name" ,
                                       "updated_at","modified_by","created_at", "email1"};

            if (hiddenHeaders.Contains(e.Column.Header.ToString()))
            {
                e.Cancel = true;
            }

            if (e.Column.Header.ToString().Equals("first_name"))
            {
                e.Column.Header = "First Name";
            }

            if (e.Column.Header.ToString().Equals("last_name"))
            {
                e.Column.Header = "Last Name";
            }

            if (e.Column.Header.ToString().Equals("last_login"))
            {
                e.Column.Header = "Last Login";
            }
        }
    }
}
