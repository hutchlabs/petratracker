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
        private Models.MicrogenDataContext microgenDBo;
        private Models.PTASDataContext ptasDBo;


        public App()
        {

            if (petratracker.Properties.Settings.Default.database_tracker == string.Empty)
            {
                Views.DatabaseConnSetup dbsetupwin = new Views.DatabaseConnSetup(true);
                dbsetupwin.Show();
            }
            else
            {
                try {
                    trackerDBo = new Models.TrackerDataContext(petratracker.Properties.Settings.Default.database_tracker);
                    microgenDBo = new Models.MicrogenDataContext(petratracker.Properties.Settings.Default.database_microgen);
                    ptasDBo = new Models.PTASDataContext(petratracker.Properties.Settings.Default.database_ptas);

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

        public Models.MicrogenDataContext MicrogenDBo
        {
            get { return this.microgenDBo; }
            set { this.microgenDBo = value; }
        }


        public Models.PTASDataContext PTASDBo
        {
            get { return this.ptasDBo; }
            set { this.ptasDBo = value; }
        }
	}
}