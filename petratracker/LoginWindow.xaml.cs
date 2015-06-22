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

using petratracker.Models;
using petratracker.Code;

namespace petratracker
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        TrackerDataContext trackerDB = new TrackerDataContext();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void doLogin()
        {
            if (tbx_username.Text.Length > 0 && tbx_password.Password.Length > 0)
            {
                try
                {
                    var user = trackerDB.Users.Single(u => u.username == tbx_username.Text);

                    if (BCrypt.CheckPassword(tbx_password.Password + "^Y8~JJ", user.password))
                    {
                        Properties.Settings.Default.username = tbx_username.Text;
                        MainWindow mw = new MainWindow();
                        mw.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Password are incorrect. Please re-enter.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Username is incorrect. Please re-enter.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Enter your username and/or password", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }       
        }

        private void closeApplication()
        {
            this.Hide();
            var confirmResult = MessageBox.Show("Are you sure to exit?",
                                     "Exit Tracker Application",
                                     System.Windows.MessageBoxButton.YesNo,MessageBoxImage.Question);
            if (confirmResult == MessageBoxResult.Yes)
            {
                this.Close();
            }
            else
            {
                this.Show();
            }
        }

        private void onKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                closeApplication();
            }
        }

        private void password_onKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                doLogin();
            }
        }

        private void lbl_close_MousePress(object sender, MouseButtonEventArgs e)
        {
                this.closeApplication();
        }

        private void lbl_login_Click(object sender, RoutedEventArgs e)
        {
            doLogin();
        }

        private void autoLogin(object sender, RoutedEventArgs e)
        {
            tbx_password.Password = "dogdog";
            tbx_username.Text = "dhutchful@gmail.com";
            doLogin();
        }

        private void resetPassword(object sender, MouseButtonEventArgs e)
        {
            if (tbx_username.Text.Length > 0)
            {
                TrackerDataContext trackerDB = new TrackerDataContext();
                try
                {
                    var user = trackerDB.Users.Single(u => u.username == tbx_username.Text);

                    SendEmail sendMail = new SendEmail();
                    //sendMail.sendMail("arkaah@cdhgroup.co",txtEmail.Text,"Your us.coer credentails from PetraTracker")

                    MessageBox.Show("Reset message sent to Administrator. Please follow up with them.", "Reset Password", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Username is incorrect. Please re-enter.", "Reset Password Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Enter your username and re-click the \"Reset\" link", "Reset Password Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
        }
    }
}
