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
    public partial class Roles : UserControl
    {
        #region Constructor

        public Roles()
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
