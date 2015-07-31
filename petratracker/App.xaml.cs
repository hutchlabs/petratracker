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

        private bool _saveOnExit = true;

        #endregion

        #region Public Properties

        #endregion

        #region Constructor

        public App()
        {
            //CultureInfo culture;
            //NumberFormatInfo nfi = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
            //culture = CultureInfo.CurrentCulture;
            //nfi.CurrencySymbol = "GHC";
            //culture.NumberFormat = (NumberFormatInfo) nfi.Clone() ;
            //Thread.CurrentThread.CurrentCulture = culture;

            FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentUICulture.IetfLanguageTag)));

            Utility.LogUtil.LogInfo("App", "Constructor", "Starting up");

            try
            {
                Models.TrackerDB.Initialize();
                (new ConfigWindow(true)).Show();

                (new LoginWindow()).Show();
            }
            catch (Exceptions.TrackerDBNotSetupException)
            {
                (new ConfigWindow(true)).Show();
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
                    Models.TrackerDB.Tracker.SubmitChanges();
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