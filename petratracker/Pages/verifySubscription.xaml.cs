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
using System.Data;

using petratracker.Models;

namespace petratracker.Pages
{
    /// <summary>
    /// Interaction logic for verifySubscription.xaml
    /// </summary>
    public partial class verifySubscription : Window
    {
        public verifySubscription()
        {
            InitializeComponent();

        }


        TrackerDataContext trackerDB = (App.Current as App).TrackerDBo;
        MicrogenDataContext microgenDB = (App.Current as App).MicrogenDBo;

        public int subID = -1;



        private void load_subscription()
        {

            var subscription = (from p in trackerDB.Payments
                                 where p.id == this.subID && p.status != "Identified"
                                 select p).Single();


            txtTransRef.Text = subscription.transaction_ref_no.ToString();
            txtTransDate.Text = subscription.transaction_date.ToString("dd-MMM-yyyy");
            txtValueDate.Text = subscription.value_date.ToString("dd-MMM-yyyy");
            txtTranAmount.Text = subscription.transaction_amount.ToString();
            txtTransDetails.Text = subscription.transaction_details.ToString();
                     
            
        }


        private void load_suggestions()
        {
            try
            {
                var companies = (from c in microgenDB.cclv_AllEntities
                                 select c.FullName).ToList();


                List<string> slipVals = txtTransDetails.Text.Split(' ').ToList<string>();


                //cmbCompanies.ItemsSource = companies.FindAll();
            }
            catch(Exception sugError)
            {
                MessageBox.Show(sugError.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            load_subscription();
        }

        private void btnGetSuggestions_Click(object sender, RoutedEventArgs e)
        {
            load_suggestions();
        }

        private void btnIdentified_Click(object sender, RoutedEventArgs e)
        {

            var subscription = from p in trackerDB.Payments
                                where p.id == this.subID && p.status != "Identified"
                                select p;

            foreach(Payment p in subscription)
            {
                p.company_code = txtCompanyCode.Text;
                p.status = "Identified";
            }

            trackerDB.SubmitChanges();
        }




    }
}
