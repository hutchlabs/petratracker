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
    /// <summary>
    /// Interaction logic for subscriptions.xaml
    /// </summary>
    public partial class subscriptions : Window
    {
        public subscriptions()
        {
            InitializeComponent();
        }

        TrackerDataContext trackerDB = (App.Current as App).TrackerDBo;


        private void load_subscriptions(string sub_status)
        { 
            try
            {

                var subscriptions = (from p in trackerDB.Payments
                                     where p.status == sub_status 
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
               


                lblSubStatus.Content = sub_status + " Subscriptions";
            }
            catch(Exception subsError)
            {
                MessageBox.Show(subsError.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            load_subscriptions("Pending");
        }

        private void btnIdentified_Click(object sender, RoutedEventArgs e)
        {
            load_subscriptions("Identified");
        }

        private void btnUnidentified_Click(object sender, RoutedEventArgs e)
        {
            load_subscriptions("Unidentified");
        }

        private void btnReturned_Click(object sender, RoutedEventArgs e)
        {
            load_subscriptions("Returned");
        }

        private void viewSubscriptions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                verifySubscription openVerification = new verifySubscription();
                Payment selVal = (Payment)viewSubscriptions.SelectedItem;
                openVerification.subID = selVal.id;
                openVerification.ShowDialog();
            }
            catch(Exception subError)
            {
                MessageBox.Show(subError.Message+subError.Source+subError.StackTrace);
            }
        }
    }
}
