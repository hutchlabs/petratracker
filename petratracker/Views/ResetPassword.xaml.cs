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
    public partial class ResetPassword : Window
    {
        private User currentUser;
        private TrackerDataContext trackerDB = (App.Current as App).TrackerDBo;
        
        public ResetPassword()
        {
            InitializeComponent();
            currentUser = trackerDB.Users.Single(p => p.username == Properties.Settings.Default.username);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            if(tbx_password.Password == string.Empty || tbx_confirmpassword.Password == string.Empty)
            {
                MessageBox.Show("Please fill out both fields.","Reset Password Error",MessageBoxButton.OK,MessageBoxImage.Error);
                tbx_password.Focus();
            }
            else
            {
                if (tbx_password.Password != tbx_confirmpassword.Password)
                {
                    MessageBox.Show("The two passwords do not match. Please re-enter the password confirmation.", "Reset Password Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    tbx_confirmpassword.Clear();
                    tbx_confirmpassword.Focus();
                }
                else
                {
                    try
                    {
                        var newpassword = BCrypt.HashPassword(tbx_password.Password, BCrypt.GenerateSalt());

                        if (newpassword != currentUser.password)
                        {
                            currentUser.password = newpassword;
                            currentUser.first_login = false;
                            currentUser.modified_by = currentUser.id;
                            currentUser.updated_at = new DateTime();
                            trackerDB.SubmitChanges();

                            MainWindow mw = new MainWindow();
                            mw.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("You cannot use the same password as the old one. Please enter a new password.", "Reset Password Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            tbx_confirmpassword.Clear();
                            tbx_password.Clear();
                            tbx_password.Focus();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Could not save new password " + ex.GetBaseException().ToString(), "Reset Password Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
