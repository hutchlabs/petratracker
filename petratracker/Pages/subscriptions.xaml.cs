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
        #region Private Members 
        
        TrackerDataContext trackerDB = (App.Current as App).TrackerDBo;
        User ini_user;
        private string[] _ops_user = { "Unidentified", "Identified and Approved", "Returned" };
        private string[] _super_ops_user = { "Identified", "Un-Identified", "Identified and Approved", "Returned" };
        
        #endregion

        #region Constructor

        public subscriptions()
        {
            InitializeComponent();
            ini_user = trackerDB.Users.Single(p => p.username == Properties.Settings.Default.username);
        }

        #endregion

        #region Event Handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            load_subscriptions("Unidentified");
            if (ini_user.Role.role1.ToLower().Equals("ops user"))
            {
                cmbSubType.ItemsSource = _ops_user.ToList();
            }
            else if (ini_user.Role.role1.ToLower().Equals("super ops user"))
            {
                cmbSubType.ItemsSource = _super_ops_user.ToList();
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
                if((cmbSubType.SelectedValue as string).Equals("Unidentified"))
                {
                    verifySubscription openVerification = new verifySubscription();
                    Payment selVal = (Payment)viewSubscriptions.SelectedItem;
                    openVerification.subID = selVal.id;
                    openVerification.ShowDialog();           
                }
                else if(cmbSubType.SelectedValue == "identified")
                {
                
                }
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

        #endregion

        #region Private Helper Methods

        private void load_subscriptions(string sub_status)
        {
            try
            {
                var subscriptions = (from p in trackerDB.Payments
                                     where p.job_id == Code.ActiveScript.job_id && p.status == sub_status
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
            catch (Exception)
            {
                //MessageBox.Show(subsError.Message);
            }
        }

        #endregion
    }
}
