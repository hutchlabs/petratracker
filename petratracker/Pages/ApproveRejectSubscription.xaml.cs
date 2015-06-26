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
using System.Windows.Shapes;
using petratracker.Models;

namespace petratracker.Pages
{
    /// <summary>
    /// Interaction logic for ApproveRejectSubscription.xaml
    /// </summary>
    public partial class ApproveRejectSubscription : Window
    {
        public ApproveRejectSubscription()
        {
            InitializeComponent();
        }



        TrackerDataContext trackerDB = (App.Current as App).TrackerDBo;
        MicrogenDataContext microgenDB = (App.Current as App).MicrogenDBo;
        private User ini_user = new User();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            load_subscription();
        }

        private void load_subscription()
        {
            var subscription = (from p in trackerDB.Payments
                                where p.id == Code.ActiveScript.subscription_id && p.status == "Identified"
                                select p).Single();

            txtTransRef.Text = subscription.transaction_ref_no.ToString();
            txtTransDate.Text = subscription.transaction_date.ToString("dd-MMM-yyyy");
            txtValueDate.Text = subscription.value_date.ToString("dd-MMM-yyyy");
            txtTranAmount.Text = subscription.transaction_amount.ToString();
            txtTransDetails.Text = subscription.transaction_details.ToString();
            txtSubscriptionAmount.Text = subscription.subscription_amount.ToString();
            dtSubscriptionDate.SelectedDate = subscription.subscription_value_date;
            txtCompanyCode.Text = subscription.company_code;
        }


        private void btnReject_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
