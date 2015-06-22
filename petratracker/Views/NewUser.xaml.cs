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

using petratracker.Code;
using petratracker.Models;

namespace petratracker
{
    /// <summary>
    /// Interaction logic for NewUser.xaml
    /// </summary>
    public partial class NewUser : Window
    {
        private User currentUser;

        Code.SendEmail sendMail = new Code.SendEmail();
        TrackerDataContext trackerDB = new TrackerDataContext();
        
        public NewUser()
        {
            InitializeComponent();
            currentUser = trackerDB.Users.Single(p => p.username == Properties.Settings.Default.username);
        }

        public IEnumerable<Role> GetRoles()
        {
            return (from r in trackerDB.Roles select r);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
          if(createUser())
          {
              this.Close();
          }
        }

        private bool validate_entries()
        {
            bool cont = false;
            if (txtFirstName.Text == string.Empty)
            {
                MessageBox.Show("Please provide the first name of the user.");
                txtFirstName.Focus();
            }
            else if(txtLastName.Text == string.Empty)
            {
                MessageBox.Show("Please provide the last name of the user.");
                txtLastName.Focus();
            }
            else if(txtEmail.Text == string.Empty)
            {
                MessageBox.Show("Please provide the email of the user.");
                txtEmail.Focus();
            }
            else if(txtPassword.Password == string.Empty)
            {
                MessageBox.Show("Please provide the password of the user.");
                txtFirstName.Focus();
            }
            else if (cmbUserRole.Text == string.Empty)
            {
                MessageBox.Show("Please select the role of the user.");
            }
            else
            {
                cont = true;
            }

            return cont;
        }

        private bool createUser()
        {
            bool cont = false;

            try
            {        
                if (validate_entries())
                {
                    User newUser = new User();
                    newUser.username = txtEmail.Text;
                    newUser.password = BCrypt.HashPassword(txtPassword.Password + "^Y8~JJ", BCrypt.GenerateSalt());
                    newUser.first_name = txtFirstName.Text;
                    newUser.last_name = txtLastName.Text;
                    newUser.email1 = txtEmail.Text;
                    newUser.role_id = (int) cmbUserRole.SelectedValue;
                    newUser.modified_by = currentUser.id;

                    trackerDB.Users.InsertOnSubmit(newUser);
                    trackerDB.SubmitChanges();

                    cont = true;
                    if(sendMail.sendNewUserMail(txtFirstName.Text+ " "+txtLastName.Text, txtEmail.Text, txtPassword.Password))
                    {
                        cont = true;
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Could not create user: "+e.GetBaseException().ToString(), "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return cont;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbUserRole.ItemsSource = this.GetRoles();
            }
            catch (Exception)
            {
                MessageBox.Show("Could not load roles", "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
