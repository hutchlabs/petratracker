using petratracker.Models;
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

namespace petratracker.Views
{
    /// <summary>
    /// Interaction logic for NewRole.xaml
    /// </summary>
    public partial class NewRole : Window
    {
        private User currentUser;
        private TrackerDataContext trackerDB = new TrackerDataContext();

        public NewRole()
        {
            InitializeComponent();
            currentUser = trackerDB.Users.Single(p => p.username == Properties.Settings.Default.username);
        }

        private bool verify_entires()
        { 
            bool cont = false;
            if (txtName.Text != string.Empty)
            {
                cont = true;
            }
            else
            {
                MessageBox.Show("Please specify the name of the role", "New Role Error", MessageBoxButton.OK, MessageBoxImage.Error);
                txtName.Focus();
            }

            return cont;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (verify_entires())
            {
                try
                {
                    Role newRole = new Role();
                    newRole.role1 = txtName.Text;
                    newRole.description = txtDescription.Text;
                    newRole.modified_by = currentUser.id;
                    trackerDB.Roles.InsertOnSubmit(newRole);
                    trackerDB.SubmitChanges();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not save role" + ex.GetBaseException().ToString(), "New Role Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
