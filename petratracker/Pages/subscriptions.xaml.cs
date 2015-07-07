using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using petratracker.Models;

namespace petratracker.Pages
{
    public partial class subscriptions : Page
    {
        string [] ops_user = { "Unidentified", "Identified and Approved", "Returned", "Rejected" };
        string [] super_ops_user = { "Identified", "Unidentified", "Identified and Approved", "Returned", "Rejected" };
        private int _jobid;

        public subscriptions()
        {
            InitializeComponent();
        }

        public subscriptions(int job_id)
        {
            _jobid = job_id;
        }
        
        private void load_subscriptions(string sub_status)
        { 
            try
            {
                var subscriptions = (from p in TrackerDB.Tracker.Payments
                                     where p.job_id == _jobid && p.status == sub_status
                                     select p
                                    );

                viewSubscriptions.ItemsSource = subscriptions;

                viewSubscriptions.Columns[0].Visibility = System.Windows.Visibility.Hidden;
                viewSubscriptions.Columns[1].Visibility = System.Windows.Visibility.Hidden;
                viewSubscriptions.Columns[4].Visibility = System.Windows.Visibility.Hidden;
                viewSubscriptions.Columns[5].Visibility = System.Windows.Visibility.Hidden;
                viewSubscriptions.Columns[6].Visibility = System.Windows.Visibility.Hidden;
                viewSubscriptions.Columns[7].Visibility = System.Windows.Visibility.Hidden;
                viewSubscriptions.Columns[8].Visibility = System.Windows.Visibility.Hidden;
                //viewSubscriptions.Columns[9].Visibility = System.Windows.Visibility.Hidden;
                viewSubscriptions.Columns[10].Visibility = System.Windows.Visibility.Hidden;
                viewSubscriptions.Columns[11].Visibility = System.Windows.Visibility.Hidden;
                viewSubscriptions.Columns[12].Visibility = System.Windows.Visibility.Hidden;
                viewSubscriptions.Columns[13].Visibility = System.Windows.Visibility.Hidden;
                viewSubscriptions.Columns[14].Visibility = System.Windows.Visibility.Hidden;
                viewSubscriptions.Columns[15].Visibility = System.Windows.Visibility.Hidden;
                viewSubscriptions.Columns[16].Visibility = System.Windows.Visibility.Hidden;
            }
            catch(Exception)
            {
                //MessageBox.Show(subsError.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            load_subscriptions("Unidentified");

            if (TrackerUser.GetCurrentUser().Role.role1.ToLower().Equals("ops user"))
            {
                cmbSubType.ItemsSource = ops_user.ToList();
            }
            else if (TrackerUser.GetCurrentUser().Role.role1.ToLower().Equals("super ops user"))
            {
                cmbSubType.ItemsSource = super_ops_user.ToList();
            }
            else
            {
                MessageBox.Show("Your user role does not have previleges to handle subscriptions.", "Unauthorized User", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }


        }

        private void viewSubscriptions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var option = cmbSubType.SelectedValue.ToString();
                Payment selVal = (Payment)viewSubscriptions.SelectedItem;
                verifySubscription openVerification = new verifySubscription(option.ToString(), selVal.id);
                openVerification.ShowDialog();               
            }
            catch(Exception subError)
            {
                MessageBox.Show(subError.Message+subError.Source+subError.StackTrace);
            }
        }

        private void cmbSubType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var option = cmbSubType.SelectedValue.ToString();

                switch (option.ToString())
                {
                    case "Identified": load_subscriptions("Identified"); break;
                    case "Unidentified": load_subscriptions("Unidentified"); break;
                    case "Returned": load_subscriptions("Returned"); break;
                    case "Identified and Approved": load_subscriptions("Identified and Approved"); break;
                    case "Rejected": load_subscriptions("Rejected"); break;
                    default:
                        load_subscriptions("Unidentified");
                        break;
                }
            }
            catch (Exception loadErr) 
            {
                MessageBox.Show(loadErr.Message);
            }
        }
    }
}
