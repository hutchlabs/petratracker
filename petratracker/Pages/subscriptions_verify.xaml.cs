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
    public partial class SubscriptionsVerify : Window
    {
        public SubscriptionsVerify()
        {
            InitializeComponent();
        }


        TrackerDataContext trackerDB = (App.Current as App).TrackerDBo;
        MicrogenDataContext microgenDB = (App.Current as App).MicrogenDBo;
        private User ini_user = new User();
        public int subID = -1;
        //DateTime nowDate = new DateTime();

        private void get_deal_desc_period()
        { 
            if(chkSavingsBooster.IsChecked == true)
            {
                txtDealDescDate.Text = dtSubscriptionDate.SelectedDate.Value.ToString("MMM yyyy");
                btnGetAllClients.IsEnabled = true;
            }
            else if (chkSavingsBooster.IsChecked != true)
            {               
                txtDealDescDate.Text = dtSubscriptionDate.SelectedDate.Value.AddMonths(-1).ToString("MMM yyyy");
                btnGetAllClients.IsEnabled = false;
            }
        }

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
            //
            dtSubscriptionDate.SelectedDate = subscription.value_date;
            dtSubscriptionDate.SelectedDate.Value.ToString("dd-MMM-yyyy");
            //
            get_deal_desc_period(); 
            
            
                     
            //Slip Transaction Details into String Array
            //List<string> slipVals = txtTransDetails.Text.Split(' ').ToList<string>();
            //slipVals.RemoveAll(x => x == "");
            //cmbTransArray.ItemsSource = slipVals;          
        }

        private void load_company_suggestions()
        {
            try
            {  
                cmbCompanies.Items.Clear();       
     
                var companies = (from c in microgenDB.cclv_AllEntities

                                 where c.FullName.Contains(txtSearchCompany.Text) && c.EntityTypeDesc == "Company"
                                 
                                 select new { c.EntityKey, c.FullName });

                foreach(var comp in companies)
                {
                    cmbCompanies.Items.Add(new KeyValuePair<string,string>(comp.FullName,comp.EntityKey));                       
                }
                lblCompsFound.Content = cmbCompanies.Items.Count.ToString() + " suggestions found.";
            }
            catch(Exception sugError)
            {
                MessageBox.Show(sugError.Message);
            }
        }

        private void load_client_suggestions()
        {
            try
            {
                cmbClient.Items.Clear();

                var clients = (from c in microgenDB.cclv_AllEntities

                               where c.FullName.Contains(txtSearchClients.Text) && c.EntityKey.Contains("HI")

                               select new { c.EntityKey, c.FullName });

                foreach (var client in clients)
                {
                    cmbClient.Items.Add(new KeyValuePair<string, string>(client.FullName, client.EntityKey));
                }
                lblClientsFound.Content = cmbClient.Items.Count.ToString() + " suggestions found.";
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

            cmbClient.DisplayMemberPath = "Key";
            cmbClient.SelectedValuePath = "Value";

            //btnIdentified.Content = (ini_user.Role.role1.ToLower().Equals("ops user"))
              //                    ? "Send for Approval"
                //                  : "Send for Approval";
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
                        p.savings_booster = chkSavingsBooster.IsChecked;
                        p.savings_booster_client_code = txtClientCode.Text;
                        p.deal_description_period = txtDealDescDate.Text;
                        p.deal_description = txtDealDescription.Text;
                        p.identified_by = ini_user.id;
                        p.modified_by = ini_user.id;
                        p.date_identified = DateTime.Today;
                        p.updated_at = DateTime.Today;                      
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

        private bool verify_update()
        {
            bool cont = false;
            if (chkSavingsBooster.IsChecked == true)
            {
                if (txtClientCode.Text != string.Empty)
                {
                    cont = true;
                }
            }
            else
            {
                cont = true;
            }
            return cont;
        }

        private void btnIdentified_Click(object sender, RoutedEventArgs e)
        {
            if (txtCompanyCode.Text != string.Empty)
            {
                if ((chkReturned.IsChecked == false))
                {
                    String conf_str = "Transaction Details \n\n";
                    conf_str += "Transaction Ref No. : "+txtTransRef.Text+"\n";
                    conf_str += "Transaction Date : "+txtTransDate.Text+"\n";
                    conf_str += "Subscription/Value Date : "+txtValueDate.Text+"\n";
                    conf_str += "Amount : " + txtTranAmount.Text + "\n";
                    conf_str += "Deal Period : "+ txtDealDescDate.Text +"\n";
                    conf_str += "Deal Description : "+ txtDealDescription.Text +"\n";
                    conf_str += "Identified Company : " + cmbCompanies.Text + "\n";
                    conf_str += "Savings Booster Client : "+ cmbClient.Text +"\n\n";
                    conf_str += "Please click Yes to commit changes to this transaction.";
                    if (MessageBox.Show(conf_str, "Confirm Identification", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        if (update_payment("Identified"))
                        {
                            MessageBox.Show("Payment has been flagged as identified.", "Identified", MessageBoxButton.OK, MessageBoxImage.Information);
                            TrackerNotification.Add(3, "Subscription Approval", "Payments", Code.ActiveScript.subscription_id);
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("No changes have been committed to this transaction.");
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

        private void btnGetAllComapnies_Click(object sender, RoutedEventArgs e)
        {
            if (txtSearchCompany.Text != string.Empty)
            {
                load_company_suggestions();
            }
            else
            {
                MessageBox.Show("Please enter a search text.");
                txtSearchCompany.Focus();
            }
        }

        private void btnGetAllClients_Click(object sender, RoutedEventArgs e)
        {
            if(txtSearchClients.Text != string.Empty)
            {
                load_client_suggestions();
            }
            else
            {
                MessageBox.Show("Please enter a search text.");
                txtSearchClients.Focus();
            }
        }

        private void cmbClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                txtClientCode.Text = cmbClient.SelectedValue.ToString();
            }
            catch (Exception compError)
            {
                MessageBox.Show(compError.Message);
            }
        }

        private void chkSavingsBooster_Checked(object sender, RoutedEventArgs e)
        {
            get_deal_desc_period();
        }

        private void chkSavingsBooster_Unchecked(object sender, RoutedEventArgs e)
        {
            get_deal_desc_period();
        }

       




    }
}
