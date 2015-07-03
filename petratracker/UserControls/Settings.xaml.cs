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
    public partial class Settings : UserControl
    {
        #region Constructor

        public Settings()
        {
            InitializeComponent();
            viewSettings.ItemsSource = Models.TrackerSettings.GetSettings();
        }

        #endregion

        #region Event Handlers

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (verify_entires())
            {
                try
                {
                    Models.TrackerSettings.Add(txtName.Text, txtValue.Text);
                    txtName.Text = "";
                    txtValue.Text = "";
                    viewSettings.ItemsSource = Models.TrackerSettings.GetSettings();
                    InnerSubTabControl.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not save setting: " + ex.GetBaseException().ToString(), "New Setting Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void viewSettings_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string[] hiddenHeaders = { "id", "updated_at","modified_by","created_at", "Users" };

            if (hiddenHeaders.Contains(e.Column.Header.ToString()))
            {
                e.Cancel = true;
            }

            if (e.Column.Header.ToString().Equals("setting1"))
            {
                e.Column.Header = "Setting";
            }

            if (e.Column.Header.ToString().Equals("value"))
            {
                e.Column.Header = "Value";
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
