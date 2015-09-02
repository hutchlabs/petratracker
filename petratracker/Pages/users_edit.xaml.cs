using MahApps.Metro.Controls;
using petratracker.Models;
using petratracker.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public partial class EditUser : UserControl, INotifyPropertyChanged
    {
        #region Private Members
        private User _staff;
        private bool _loadedInFlyout = false;
        private string _textNotEmptyProperty;

        #endregion

        #region Public Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public string TextNotEmptyProperty
        {
            get { return this._textNotEmptyProperty; }
            set
            {
                if (Equals(value, _textNotEmptyProperty))
                {
                    return;
                }

                _textNotEmptyProperty = value;
                RaisePropertyChanged("TextNotEmptyProperty");
            }
        }

        #endregion

        #region Constructor

        public EditUser(User u, bool inFlyout = false)
        {
            this.DataContext = this;

            InitializeComponent();

            _staff = u;
            cmbUserRole.ItemsSource = Models.TrackerUser.GetRoles();
            _loadedInFlyout = inFlyout;
        }

        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtEmail.Text = _staff.username;
            txtFirstName.Text = _staff.first_name;
            txtMiddleName.Text = _staff.middle_name;
            txtLastName.Text = _staff.last_name;
            txtPassword.Password = _staff.password;

            for(int i=0; i < cmbUserRole.Items.Count; i++)
            {
                if ( ((Role)cmbUserRole.Items[i]).id == _staff.role_id) {
                    cmbUserRole.SelectedIndex = i;
                }
            }
        }

        #endregion

        #region Event Handlers

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (_loadedInFlyout)
                close_flyout();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate_entries())
                {

                    _staff.username = txtEmail.Text;
                    if (_staff.password != txtPassword.Password)
                    {
                        _staff.password = BCrypt.HashPassword(txtPassword.Password, BCrypt.GenerateSalt());
                    }
                    _staff.first_name = txtFirstName.Text;
                    _staff.last_name = txtLastName.Text;
                    _staff.middle_name = txtMiddleName.Text;
                    _staff.role_id = (int)cmbUserRole.SelectedValue;
                    Models.TrackerUser.Save(_staff);

                    if (_loadedInFlyout)
                        close_flyout();
                }
            }
            catch (Exception ex)
            {
                Utility.LogUtil.LogError("UpdateUsers", "btnSave_Click", ex);
                MessageBox.Show("Could not update user: " + ex.GetBaseException().ToString(), "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
   
            }
        }

        #endregion

        #region Private Helper Methods

        private void close_flyout()
        {
            Window parentWindow = Window.GetWindow(this);
            object obj = parentWindow.FindName("surrogateFlyout");
            Flyout flyout = (Flyout)obj;
            flyout.Content = null;
            flyout.IsOpen = !flyout.IsOpen;
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "TextNotEmptyProperty" && this.TextNotEmptyProperty.Equals(string.Empty))
                {
                    return "Field cannot be empty. Please enter a value";
                }
                return null;
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
