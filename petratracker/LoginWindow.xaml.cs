using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using petratracker.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace petratracker
{
    public partial class LoginWindow : MetroWindow
    {
        private App _app;

        #region Constructor 
        
        public LoginWindow(App app)
        {
            InitializeComponent();
            _app = app;
        }

        #endregion

        #region Event Handlers

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
            string user = ((Button)sender).Content.ToString();
            if (user.Equals("Super Admin Login"))
            {
                tbx_password.Password = "dogdog";
                tbx_username.Text = "admin@petratrust.com";
            }
            else if (user.Equals("Super Ops User Login"))
            {
                tbx_password.Password = "dogdog";
                tbx_username.Text = "superops@petratrust.com";
            }
            else if(user.Equals("Ops User Login"))
            {
                tbx_password.Password = "dogdog";
                tbx_username.Text = "opsuser@petratrust.com";
            }
            else
            {
                tbx_password.Password = "dogdog";
                tbx_username.Text = "reporter@petratrust.com";
            }
            doLogin();
        }

        private async void resetPassword(object sender, RoutedEventArgs e)
        {
            var result = await this.ShowInputAsync("Reset Password", "What is your username?");

            if (result == null) //user pressed cancel
                return;

            if (result.Length > 0)
            {
                bool success = false;
                try
                {
                    TrackerUser.ResetPasswordRequest(result);
                    success = true;
                }
                catch (Exception) { success = false; }

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
        
        #endregion

        #region Private Helper Methods

        private void doLogin()
        {
            if (tbx_username.Text.Length > 0 && tbx_password.Password.Length > 0)
            {
                try
                {
                    if (TrackerUser.FirstLogin(tbx_username.Text))
                    {
                        changePassword(tbx_username.Text);
                    }
                    else 
                    {   
                        TrackerUser.SetCurrentUser(tbx_username.Text, tbx_password.Password);

                        _app.SaveOnExit = true;
                        
                        MainWindow mw = new MainWindow();
                        
                        mw.Show();
                        
                        this.Close();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Enter your username and/or password", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void changePassword(string username)
        {
            LoginDialogData result = await this.ShowLoginAsync("Change Password"
                                                               , "This is your first login. Please change your password"
                                                               , new LoginDialogSettings
                                                               {
                                                                   ColorScheme = this.MetroDialogOptions.ColorScheme,
                                                                   InitialUsername = username,
                                                                   AffirmativeButtonText = "Change Password"
                                                                   //, EnablePasswordPreview = true 
                                                               });
            if (result == null)
            {
                MessageDialogResult messageResult = await this.ShowMessageAsync("Change Password", "You need to change  your password");
                changePassword(username);
            }
            else
            {
                if (result.Password.Length > 3)
                {
                    bool success = false;
                    string err = "";
                    try
                    {
                        TrackerUser.UpdateUserPassword(username, result.Password);
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        err = ex.GetBaseException().ToString();
                    }

                    if (success)
                    {
                        MessageDialogResult messageResult = await this.ShowMessageAsync("Change Password", "Password Updated!");

                        TrackerUser.SetCurrentUser(username, result.Password);

                        _app.SaveOnExit = true;
                        MainWindow mw = new MainWindow();
                        mw.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageDialogResult messageResult = await this.ShowMessageAsync("Change Password", err);
                        this.changePassword(username);
                    }
                }
                else
                {
                    MessageDialogResult messageResult = await this.ShowMessageAsync("Change Password", "Password is too short!");
                    this.changePassword(username);
                }
            }
        }

        #endregion
    }
}
