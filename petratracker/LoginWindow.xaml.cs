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
using petratracker.Views;

using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace petratracker
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class LoginWindow : MetroWindow
    {

        TrackerDataContext trackerDB = (App.Current as App).TrackerDBo;

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

                    if (BCrypt.CheckPassword(tbx_password.Password, user.password))
                    {
                        Properties.Settings.Default.username = tbx_username.Text;

                        if (user.first_login)
                        {
                            ResetPassword rpw = new ResetPassword();
                            rpw.Show();
                        }
                        else
                        {
                            MainWindow mw = new MainWindow();
                            mw.Show();
                        }
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Password is incorrect. Please re-enter.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void password_onKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                doLogin();
            }
        }

        private void lbl_login_Click(object sender, RoutedEventArgs e)
        {
            doLogin();
        }

        private void autoLogin(object sender, RoutedEventArgs e)
        {    
            tbx_password.Password = "john";
            tbx_username.Text = "niicoark27@gmail.com";
            doLogin();
        }

 

       private async void resetPassword(object sender, MouseButtonEventArgs e)
        {
            bool _shutdown = false;

            try
            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Quit",
                    NegativeButtonText = "Cancel",
                    AnimateShow = true,
                    AnimateHide = false
                };

                var result = await this.ShowMessageAsync("Quit application?",
                    "Sure you want to quit application?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);

                _shutdown = result == MessageDialogResult.Affirmative;

                if (_shutdown)
                    Application.Current.Shutdown();
            } catch (Exception ex){
                MessageBox.Show(ex.GetBaseException().ToString(), "Reset Password Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            
          /*  if (tbx_username.Text.Length > 0)
            {
                TrackerDataContext trackerDB = new TrackerDataContext();
                try
                {
                    var user = trackerDB.Users.Single(u => u.username == tbx_username.Text);

                    SendEmail sendMail = new SendEmail();
                    sendMail.sendResetPasswordMail(tbx_username.Text);

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
            } */
        }
    }
}
