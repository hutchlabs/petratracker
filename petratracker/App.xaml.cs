using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace petratracker
{
	public partial class App : Application
    {
        #region Private Members

        private bool _saveOnExit = false;
        private string _sbi = "";

        #endregion

        #region Public Properties

        public string StatusBarInfo
        {
            get { return _sbi; }
            set { _sbi = value; }
        }


        public bool SaveOnExit
        {
            get { return _saveOnExit;  }
            set { _saveOnExit = value;  }
        }

        #endregion

        #region Constructor

        public App()
        {
            Utility.LogUtil.LogInfo("App", "Constructor", "Starting up");

            try
            {
                Models.Database.Initialize();
                (new LoginWindow(this)).Show();
            }
            catch (Exceptions.TrackerDBNotSetupException)
            {
                (new ConfigWindow(this,true)).Show();
            }
            catch (Exceptions.TrackerDBConnectionException e)
            {
                MessageBox.Show("Cannot connect to the database. Please ensure database is available and restart the application. More Info: " + e.Message,
                                "Database Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                CloseWithoutSaving();
            }
            catch(Exception ex)
            {
                Utility.LogUtil.LogError("App", "Constructor", ex);
                MessageBox.Show("An error occurred. Please restart the application. More Info: " + ex.Message,
                "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
                CloseWithoutSaving();
            }
        }

        #endregion

        #region Public Methods
        
        public void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Utility.LogUtil.LogError("App", "AppDispatcherUnhandledException", e.Exception);
            e.Handled = true;
        }
        
        #endregion

        #region Override Methods

        protected override void OnExit(ExitEventArgs e)
        {
            Utility.LogUtil.LogInfo("App", "OnExit", "Exiting Application");

            if (_saveOnExit)
            {
                try
                {
                    Models.TrackerUser.CurrentUser.logged_in = false;
                    Models.TrackerUser.CurrentUser.last_login = DateTime.Now;
                    Models.TrackerUser.CurrentUser.updated_at = DateTime.Now;
                    Models.Database.Tracker.SubmitChanges();
                }
                catch (Exception ex)
                {
                    Utility.LogUtil.LogError("App", "OnExit", ex);
                }
            }
        }

        #endregion
 
        #region Private Methods
        
        private void CloseWithoutSaving()
        {
            _saveOnExit = false;
            Application.Current.Shutdown();
        }
        
        #endregion
    }
}