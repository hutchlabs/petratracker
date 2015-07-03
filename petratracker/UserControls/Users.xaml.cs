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

namespace petratracker.UserControls
{
    public partial class Users : UserControl
    {
        #region Constructor

        public Users()
        {
            InitializeComponent();

            try
            {
                viewUsers.ItemsSource = Models.TrackerUser.GetUsers();
                cmbUserRole.ItemsSource = Models.TrackerUser.GetRoles();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Event Handlers

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate_entries())
                {
                    Models.TrackerUser.AddUser(txtEmail.Text, txtPassword.Password, txtFirstName.Text, txtLastName.Text, txtMiddleName.Text, (int)cmbUserRole.SelectedValue);
                    clear_entries();
                    viewUsers.ItemsSource = Models.TrackerUser.GetUsers();
                    InnerSubTabControl.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not create user: " + ex.GetBaseException().ToString(), "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void viewUsers_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string[] hiddenHeaders = { "password", "id", "role_id", "first_login", "Payments", "Settings", 
                                       "Notifications", "Jobs", "theme", "accent", "Schedules","SchedulesItems",
                                       "Role", "logged_in", "middle_name" ,"updated_at","modified_by","created_at", };

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

        #endregion

        #region Private Helper Methods

        private void clear_entries()
        {
            txtFirstName.Text = "";
            txtMiddleName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            txtPassword.Password = "";
            cmbUserRole.SelectedIndex = -1;
        }

        private bool validate_entries()
        {
            bool cont = false;
            if (txtFirstName.Text == string.Empty)
            {
                MessageBox.Show("Please provide the first name of the user.");
                txtFirstName.Focus();
            }
            else if (txtLastName.Text == string.Empty)
            {
                MessageBox.Show("Please provide the last name of the user.");
                txtLastName.Focus();
            }
            else if (txtEmail.Text == string.Empty)
            {
                MessageBox.Show("Please provide the email of the user.");
                txtEmail.Focus();
            }
            else if (txtPassword.Password == string.Empty)
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

        #endregion
    }
}
