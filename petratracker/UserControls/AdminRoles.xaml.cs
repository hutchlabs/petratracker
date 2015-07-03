using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace petratracker.UserControls
{
    public partial class AdminRoles : UserControl
    {
        #region Constructor

        public AdminRoles()
        {
            InitializeComponent();
            viewRoles.ItemsSource = Models.TrackerUser.GetRoles();
        }

        #endregion

        #region Event Handlers

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (verify_entires())
            {
                try
                {
                    Models.TrackerUser.AddNewRole(txtName.Text, txtDescription.Text);
                    txtName.Text = "";
                    txtDescription.Text = "";
                    viewRoles.ItemsSource = Models.TrackerUser.GetRoles();
                    InnerSubTabControl.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not save role: " + ex.GetBaseException().ToString(), "New Role Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void viewRoles_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string[] hiddenHeaders = { "id", "updated_at","modified_by","created_at", "Notifications", "Users" };

            if (hiddenHeaders.Contains(e.Column.Header.ToString()))
            {
                e.Cancel = true;
            }

            if (e.Column.Header.ToString().Equals("role1"))
            {
                e.Column.Header = "Role";
            }

            if (e.Column.Header.ToString().Equals("description"))
            {
                e.Column.Header = "Description";
            }
        }

        #endregion

        #region Private Helper Methods

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

        #endregion
    }
}
