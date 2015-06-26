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
using System.Windows.Navigation;
using System.Windows.Shapes;
using petratracker.Models;

namespace petratracker.Pages
{
    /// <summary>
    /// Interaction logic for jobs.xaml
    /// </summary>
    public partial class jobs : Page
    {

        TrackerDataContext trackerDB = (App.Current as App).TrackerDBo;
        User ini_user;

        public jobs()
        {
            InitializeComponent();
            ini_user = trackerDB.Users.Single(p => p.username == Properties.Settings.Default.username);
        }


        private void load_jobs()
        {
            try
            {
                var jobs = (from j in trackerDB.Jobs
                            select j
                           );
                viewJobs.ItemsSource = jobs;
            }
            catch(Exception jobsErr)
            {
                MessageBox.Show(jobsErr.Message);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            load_jobs();
        }

        private void viewJobs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Job selVal = (Job)viewJobs.SelectedItem;
            Code.ActiveScript.job_id = selVal.id;
            subscriptions openSubs = new subscriptions();
            this.NavigationService.Navigate(new Uri("pages/subscriptions.xaml", UriKind.Relative));
            //show subscriptions
        }

       
    }
}
