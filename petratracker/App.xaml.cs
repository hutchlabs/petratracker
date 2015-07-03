using System;
using System.Windows;

namespace petratracker
{
	public partial class App : Application
    {
        #region Private Members

        private Models.TrackerDataContext _trackerDBo;
        private Models.MicrogenDataContext _microgenDBo;
        private Models.PTASDataContext _ptasDBo;
        private Models.User _user;

        #endregion

        #region Public Properties

        public Models.User CurrentUser
        {
            get
            {
                if (_user == null)
                    throw new Exception("Current user is not set");
                return _user;
            }
            set { _user = value; }
        }

        public Models.TrackerDataContext TrackerDBo
        {
            get { return this._trackerDBo; }
            set { this._trackerDBo = value; }
        }

        public Models.MicrogenDataContext MicrogenDBo
        {
            get { return this._microgenDBo; }
            set { this._microgenDBo = value; }
        }

        public Models.PTASDataContext PTASDBo
        {
            get { return this._ptasDBo; }
            set { this._ptasDBo = value; }
        }

        #endregion

        #region Constructor

        public App()
        {            
            if (petratracker.Properties.Settings.Default.database_tracker == string.Empty)
            {
                ConfigWindow dbsetupwin = new ConfigWindow(true);
                dbsetupwin.Show();
            }
            else
            {
                try {
                    _trackerDBo = new Models.TrackerDataContext(petratracker.Properties.Settings.Default.database_tracker);
                    _microgenDBo = new Models.MicrogenDataContext(petratracker.Properties.Settings.Default.database_microgen);
                    _ptasDBo = new Models.PTASDataContext(petratracker.Properties.Settings.Default.database_ptas);

                    (new LoginWindow()).Show();

                } catch(Exception) {
                    ConfigWindow dbsetupwin = new ConfigWindow(true);
                    dbsetupwin.Show();
                }
            }
        }

        #endregion

        #region Override Methods

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                CurrentUser.logged_in = false;
                CurrentUser.last_login = DateTime.Now;
                CurrentUser.updated_at = DateTime.Now;
                TrackerDBo.SubmitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().ToString(), "Logout Error");
            }
        }

        #endregion
    }
}