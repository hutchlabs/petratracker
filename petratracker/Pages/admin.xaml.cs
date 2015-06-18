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

namespace petratracker.Pages
{
    /// <summary>
    /// Interaction logic for admin.xaml
    /// </summary>
    public partial class admin : Page
    {

        Data.connection accessDB = new Data.connection();

        public admin()
        {
            InitializeComponent();   
            string get_users = "select * from view_users";
            viewUsers.ItemsSource = accessDB.ExecuteCmdToDataGrid(get_users).DefaultView;
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
            System.Data.DataRowView row = (System.Data.DataRowView)viewUsers.SelectedItems[0];
            NewUser editUser = new NewUser();
            editUser.txtUserID.Text = row["ID"].ToString();
            editUser.btnSave.Content = "Update";
            editUser.ShowDialog();

        }
    }
}
