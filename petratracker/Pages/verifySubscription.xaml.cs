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
        private User ini_user = new User();
        public int subID = -1;
        DateTime nowDate = new DateTime();



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
            txtSubscriptionAmount.Text = subscription.transaction_amount.ToString();
            dtSubscriptionDate.SelectedDate = DateTime.Today;
                     
            //Slip Transaction Details into String Array

            List<string> slipVals = txtTransDetails.Text.Split(' ').ToList<string>();
            slipVals.RemoveAll(x => x == "");
            cmbTransArray.ItemsSource = slipVals;          
        }

        private void load_suggestions()
        {
            try
            {  
                cmbCompanies.Items.Clear();       
     
                var companies = (from c in microgenDB.cclv_AllEntities
                                 
                                 where c.FullName.Contains(cmbTransArray.SelectedValue.ToString())
                                 
                                 select new { c.EntityID, c.FullName });

                foreach(var comp in companies)
                {
                    cmbCompanies.Items.Add(new KeyValuePair<string,int>(comp.FullName,comp.EntityID));                       
                }
                lblCompsFound.Content = cmbCompanies.Items.Count.ToString() + " suggestions found.";
            }
            catch(Exception sugError)
            {
                MessageBox.Show(sugError.Message);
            }
        }

        private void load_all_clients()
        {
            try
            {
                cmbCompanies.Items.Clear();

                var companies = (from c in microgenDB.cclv_AllEntities
                                 select new { c.EntityID, c.FullName });

                foreach (var comp in companies)
                {
                    cmbCompanies.Items.Add(new KeyValuePair<string, int>(comp.FullName, comp.EntityID));
                }
                lblCompsFound.Content = cmbCompanies.Items.Count.ToString() + " suggestions found.";
            }
            catch (Exception sugError)
            {
                MessageBox.Show(sugError.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            load_subscription();
            cmbCompanies.DisplayMemberPath = "Key";
            cmbCompanies.SelectedValuePath = "Value";
        }

        private bool update_payment(string verifyType)
        {
            try
            {
                var subscription = from p in trackerDB.Payments
                                   where p.id == this.subID && p.status != verifyType
                                   select p;

                foreach (Payment p in subscription)
                {
                    if (verifyType == "Identified")
                    {
                        p.company_code = txtCompanyCode.Text;
                        p.subscription_value_date = dtSubscriptionDate.SelectedDate;
                        p.identified_by = ini_user.id;
                        p.modified_by = ini_user.id;
                        p.date_identified = DateTime.Today;
                        p.updated_at = DateTime.Today;
                        p.subscription_amount = decimal.Parse(txtSubscriptionAmount.Text);
                        p.status = verifyType;
                    }
                    else
                    {
                        p.status = verifyType;
                        p.updated_at = DateTime.Today;
                        p.modified_by = ini_user.id;                      
                    }
                }

                trackerDB.SubmitChanges();
                
            }
            catch(Exception updateError)
            {
                MessageBox.Show(updateError.StackTrace);
                //Log Error
            }
            return true;
        }

        private void btnIdentified_Click(object sender, RoutedEventArgs e)
        {
            if (txtCompanyCode.Text != string.Empty)
            {
                if ((chkReturned.IsChecked == false))
                {
                    if (update_payment("Identified"))
                    {
                        MessageBox.Show("Payment has been flagged as identified.", "Identified", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }
                else
                {
                    if (MessageBox.Show("This subscription would be flagged as returned, please click Yes to proceed.", "Returened Subscription", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        if (update_payment("Returned"))
                        {
                            MessageBox.Show("Payment has been flagged as returned.", "Identified", MessageBoxButton.OK, MessageBoxImage.Information);
                            //create notification and send email
                            this.Close();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please specify a company.");
            }
        }

        private void btnUnidentified_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnReturned_Click(object sender, RoutedEventArgs e)
        {
            if (update_payment("Returned"))
            {
                MessageBox.Show("Payment has been flagged as Returned.", "Returned", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }  
        }

        private void cmbCompanies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                txtCompanyCode.Text = cmbCompanies.SelectedValue.ToString();
            }
            catch(Exception compError)
            {
                MessageBox.Show(compError.Message);
            }
        }

        private void cmbTransArray_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            load_suggestions();
        }

        private void btnGetAllComapnies_Click(object sender, RoutedEventArgs e)
        {
            
            load_all_clients();
        }

       




    }
}
