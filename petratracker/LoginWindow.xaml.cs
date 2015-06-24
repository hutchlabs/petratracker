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

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
        
        }

        private void autoLogin(object sender, RoutedEventArgs e)
        {
            tbx_password.Password = "john";
            tbx_username.Text = "niicoark27@gmail.com";
            doLogin();
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
                            changePassword(user);
                        }
                        else
                        {
                            MainWindow mw = new MainWindow();
                            mw.Show();
                            this.Close();
                        }
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

        private async void changePassword(User currentUser)
        {
            LoginDialogData result = await this.ShowLoginAsync("Change Password"
                                                             , "This is your first login. Please change your password"
                                                             , new LoginDialogSettings { 
                                                                 ColorScheme = this.MetroDialogOptions.ColorScheme, 
                                                                 InitialUsername = currentUser.username,
                                                                 AffirmativeButtonText ="Change Password"
                                                                 //, EnablePasswordPreview = true 
                                                             });
            if (result == null)
            {
                MessageDialogResult messageResult = await this.ShowMessageAsync("Change Password", "You need to change  your password");
                changePassword(currentUser);
            }
            else
            {
                if (result.Password.Length > 3)
                {
                    bool success = false;
                    string err = "";
                    try
                    {
                        var newpassword = BCrypt.HashPassword(result.Password, BCrypt.GenerateSalt());

                        currentUser.password = newpassword;
                        currentUser.first_login = false;
                        currentUser.modified_by = currentUser.id;
                        currentUser.updated_at = new DateTime();
                        trackerDB.SubmitChanges();
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        err = ex.GetBaseException().ToString();
                    }

                    if (success)
                    {
                        MessageDialogResult messageResult = await this.ShowMessageAsync("Change Password", "Password Updated!");
                        MainWindow mw = new MainWindow();
                        mw.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageDialogResult messageResult = await this.ShowMessageAsync("Change Password", err);
                        this.changePassword(currentUser);
                    }
                }
                else
                {
                    MessageDialogResult messageResult = await this.ShowMessageAsync("Change Password", "Password is too short!");
                    this.changePassword(currentUser);
                }
            }
        }

        private async void resetPassword(object sender, RoutedEventArgs e)
        {
            var result = await this.ShowInputAsync("Reset Password", "What is your username?");

            if (result == null) //user pressed cancel
                return;

            if (result.Length > 0)
            {
                TrackerDataContext trackerDB = new TrackerDataContext();
                bool success = false;
                try
                {
                    var user = trackerDB.Users.Single(u => u.username == result);
                    SendEmail sendMail = new SendEmail();
                    //sendMail.sendResetPasswordMail(result);
                    success = true;
                }
                catch (Exception) { }

                if (success)
                {
                    await this.ShowMessageAsync("Reset Password", "Reset message sent to Administrator. Please follow up with them.");
                    this.Close();
                }
                else
                {
                    await this.ShowMessageAsync("Reset Password Error", "Username is incorrect. Please re-enter.");
                    resetPassword(sender, e);
                }
            }
        }
    }
}
