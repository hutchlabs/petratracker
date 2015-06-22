using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Threading;

namespace petratracker
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
        private Models.TrackerDataContext trackerDBo;

        public App()
        {
            if (petratracker.Properties.Settings.Default.database == string.Empty)
            {
                Views.DatabaseConnSetup dbsetupwin = new Views.DatabaseConnSetup(true);
                dbsetupwin.Show();
            }
            else
            {
                try {
                    trackerDBo = new Models.TrackerDataContext(petratracker.Properties.Settings.Default.database);
                    (new LoginWindow()).Show();
                } catch(Exception) {
                    Views.DatabaseConnSetup dbsetupwin = new Views.DatabaseConnSetup(true);
                    dbsetupwin.Show();
                }
            }
        }

        public Models.TrackerDataContext TrackerDBo
        {
            get { return this.trackerDBo; }
            set { this.trackerDBo = value; }
        }

	}
}