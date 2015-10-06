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
using MahApps.Metro.Controls;
using petratracker.Utility;

namespace petratracker.Pages
{
    public partial class verifySubscription : UserControl
    {
        private int subID = -1;
        private string subType = string.Empty;
        private bool _loadedInFlyout = false;

        public IEnumerable<ComboBoxPairs> ContributionTypes
        {
            private set { ; }
            get { return TrackerSchedule.GetContributionTypes(); }
        }

        public verifySubscription(string subscription_status,int subscription_id,bool inFlyout=false)
        {
            this.DataContext = this;
            InitializeComponent();
            _loadedInFlyout = inFlyout;
            subID = subscription_id;
            subType = subscription_status;
            lblSubscriptionType.Content = (object)subscription_status;
        }

        private string [] get_years(int start_year)
        { 
            string [] ini_year = new string[3];
            for(int ini = 0; ini < 3; ini++)
            {             
                ini_year[ini] = (start_year - 1).ToString();
                start_year++;
            }
            return ini_year;
        }

        private void get_deal_desc_period()
        {
            try
            {
                if (chkSavingsBooster.IsChecked == true)
                {
                    grpSavingsBooster.IsEnabled = true;
                }
                else if (chkSavingsBooster.IsChecked != true)
                {
                    grpSavingsBooster.IsEnabled = false;
                    txtSearchClients.Text = string.Empty;
                    cmbClient.SelectedIndex = -1;
                    txtClientCode.Text = string.Empty;
                }
            }
            catch(Exception dealError)
            {
                MessageBox.Show(dealError.Message);
            }
        }

        private void load_unidentified_subscription()
        {
            var subscription = (from p in Database.Tracker.PPayments
                                 where p.id == this.subID && p.status != "Identified"
                                 select p).Single();

            txtTransRef.Text = subscription.transaction_ref_no.ToString();
            txtTransDate.Text = subscription.transaction_date.ToString("dd-MMM-yyyy");
            txtValueDate.Text = subscription.value_date.ToString("dd-MMM-yyyy");
            txtTranAmount.Text = subscription.transaction_amount.ToString();
            txtTransDetails.Text = subscription.transaction_details.ToString();
            grpTransDetails.IsEnabled = false;
            subID = subscription.id;
            get_deal_desc_period();           
        }

        private void load_rejected_subscription()
        {
            var subscription = (from p in Database.Tracker.PPayments
                                where p.id == this.subID && p.status == Constants.PAYMENT_STATUS_REJECTED
                                select p).Single();

            //Load Transaction Details
            txtTransRef.Text = subscription.transaction_ref_no.ToString();
            txtTransDate.Text = subscription.transaction_date.ToString("dd-MMM-yyyy");
            txtValueDate.Text = subscription.value_date.ToString("dd-MMM-yyyy");
            txtTranAmount.Text = subscription.transaction_amount.ToString();
            txtTransDetails.Text = subscription.transaction_details.ToString();
            txtComments.Text = subscription.comments;
            grpTransDetails.IsEnabled = false;
            subID = subscription.id;

            //Load Company Mapping
            grpCompanyMapping.IsEnabled = false;
            var companies = (from c in Database.Microgen.cclv_AllEntities
                             where c.EntityKey == subscription.company_code
                             select c);

            foreach (cclv_AllEntity ini_comp in companies)
            {
                txtSearchCompany.Text = ini_comp.FullName;
                txtCompanyCode.Text = ini_comp.EntityKey;
            }

            //Load Savings Booster
            if (subscription.savings_booster_client_code != string.Empty)
            {
                chkSavingsBooster.IsChecked = true;

                var clients = (from c in Database.Microgen.cclv_AllEntities
                               where c.EntityKey == subscription.savings_booster_client_code
                               select c);

                foreach (cclv_AllEntity ini_client in clients)
                {
                    txtSearchClients.Text = ini_client.FullName;
                    txtClientCode.Text = ini_client.EntityKey;
                }
            }
        }

        private void load_identified_subscription()
        {
            var subscription = (from p in Database.Tracker.PPayments
                                where p.id == this.subID && p.status == "Identified"
                                select p).Single();

            //Load Transaction Details
            txtTransRef.Text = subscription.transaction_ref_no.ToString();
            txtTransDate.Text = subscription.transaction_date.ToString("dd-MMM-yyyy");
            txtValueDate.Text = subscription.value_date.ToString("dd-MMM-yyyy");
            txtTranAmount.Text = subscription.transaction_amount.ToString();
            txtTransDetails.Text = subscription.transaction_details.ToString();
            txtComments.Text = subscription.comments;
            grpTransDetails.IsEnabled = false;
            subID = subscription.id;

            //Load Company Mapping
            grpCompanyMapping.IsEnabled = false;
            var companies = (from c in Database.Microgen.cclv_AllEntities
                             where c.EntityKey == subscription.company_code
                             select c);

            foreach(cclv_AllEntity ini_comp in companies)
            {
                txtSearchCompany.Text = ini_comp.FullName;
                txtCompanyCode.Text = ini_comp.EntityKey;
            }

            //Load Savings Booster
            if(subscription.savings_booster_client_code != string.Empty)
            {
                chkSavingsBooster.IsChecked = true;

                var clients = (from c in Database.Microgen.cclv_AllEntities
                                 where c.EntityKey == subscription.savings_booster_client_code
                                 select c);

                foreach (cclv_AllEntity ini_client in clients)
                {
                    txtSearchClients.Text = ini_client.FullName;
                    txtClientCode.Text = ini_client.EntityKey;
                }
            }

           //Set save button to Approve & cancel button to Reject
            if (CanApproveReject(subscription.owner)) 
            { 
                btnSave.Content = "Approve";
                btnCancel.Content = "Reject";
            } else {
                btnSave.Content = "Can't approve your own request.";
                btnCancel.Content = "Can't reject your own request.";
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;
            }
        }

        private bool CanApproveReject(int? owner_id)
        {
            if (TrackerSettings.GetSetting(Constants.SETTINGS_PERM_APPROVE_OWN)=="True")
                return true;

            if (TrackerUser.GetCurrentUser().id != owner_id)
                return true;

            return false;
        }

        private void load_approved_subscription()
        {
            var subscription = (from p in Database.Tracker.PPayments
                                where p.id == this.subID && p.status == "Identified and Approved"
                                select p).Single();

            //Load Transaction Details
            txtTransRef.Text = subscription.transaction_ref_no.ToString();
            txtTransDate.Text = subscription.transaction_date.ToString("dd-MMM-yyyy");
            txtValueDate.Text = subscription.value_date.ToString("dd-MMM-yyyy");
            txtTranAmount.Text = subscription.transaction_amount.ToString();
            txtTransDetails.Text = subscription.transaction_details.ToString();
            txtComments.Text = subscription.comments;
            grpTransDetails.IsEnabled = false;
            subID = subscription.id;
            grpCompanyMapping.IsEnabled = false;

            //Load Company Mapping
            var companies = (from c in Database.Microgen.cclv_AllEntities
                             where c.EntityKey == subscription.company_code
                             select c);

            foreach (cclv_AllEntity ini_comp in companies)
            {
                txtSearchCompany.Text = ini_comp.FullName;
                txtCompanyCode.Text = ini_comp.EntityKey;
            }

            //Load Savings Booster
            if (subscription.savings_booster_client_code != string.Empty)
            {
                chkSavingsBooster.IsChecked = true;

                var clients = (from c in Database.Microgen.cclv_AllEntities
                               where c.EntityKey == subscription.savings_booster_client_code
                               select c);

                foreach (cclv_AllEntity ini_client in companies)
                {
                    txtSearchClients.Text = ini_client.FullName;
                    txtClientCode.Text = ini_client.EntityKey;
                }
            }

            //Hide Save button
            btnSave.Visibility = System.Windows.Visibility.Hidden;

        }

        private void load_company_suggestions()
        {
            try
            {  
                cmbCompanies.Items.Clear();       
     
                var companies = (from c in Database.Microgen.cclv_AllEntities

                                 where c.FullName.Contains(txtSearchCompany.Text) && c.EntityTypeDesc == "Company"
                                 
                                 select new { c.EntityKey, c.FullName });

                foreach(var comp in companies)
                {
                    cmbCompanies.Items.Add(new KeyValuePair<string, string>(comp.EntityKey+" - "+comp.FullName, comp.EntityKey));                       
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

                var clients = (from c in Database.Microgen.cclv_AllEntities

                               where c.FullName.Contains(txtSearchClients.Text) && c.EntityKey.Contains("HI")

                               select new { c.EntityKey, c.FullName });

                foreach (var client in clients)
                {
                    cmbClient.Items.Add(new KeyValuePair<string, string>(client.EntityKey+" - "+client.FullName, client.EntityKey));
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
            
            cmbCompanies.DisplayMemberPath = "Key";
            cmbCompanies.SelectedValuePath = "Value";

            cmbClient.DisplayMemberPath = "Key";
            cmbClient.SelectedValuePath = "Value";

            load_payment_deal_descriptions();

            if(subType == Constants.PAYMENT_STATUS_IDENTIFIED){load_identified_subscription(); }
            else if(subType == Constants.PAYMENT_STATUS_UNIDENTIFIED){load_unidentified_subscription(); }
            else if (subType == Constants.PAYMENT_STATUS_RETURNED) { load_unidentified_subscription(); }
            else if (subType == Constants.PAYMENT_STATUS_IDENTIFIED_APPROVED) { load_approved_subscription(); }
            else if (subType == Constants.PAYMENT_STATUS_REJECTED) { load_rejected_subscription(); }
            else
            {
                MessageBox.Show("Transaction type unknown.", "Invalid Id", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                this.Close();
            }        
        }

        private bool update_payment(string verifyType, bool saveToPTas)
        {
            try
            {
                var subscription = from p in Database.Tracker.PPayments
                                   where p.id == this.subID && p.status != verifyType
                                   select p;

                foreach (PPayment p in subscription)
                {
                    if (verifyType == "Identified")
                    {
                        p.company_code = txtCompanyCode.Text;
                        p.company_name = cmbCompanies.Text;
                        p.savings_booster = chkSavingsBooster.IsChecked;
                        p.savings_booster_client_code = txtClientCode.Text;
                        p.identified_by = TrackerUser.GetCurrentUser().id;
                        p.modified_by = TrackerUser.GetCurrentUser().id;
                        p.date_identified = DateTime.Today;
                        p.updated_at = DateTime.Today;
                        p.comments = txtComments.Text;
                        p.status = verifyType;
                    }
                    if (verifyType == "Approved")
                    {                      
                        p.modified_by = TrackerUser.GetCurrentUser().id;
                        p.approved_by = TrackerUser.GetCurrentUser().id;
                        p.date_approved = DateTime.Today;
                        p.comments = txtComments.Text;
                        p.status = "Identified and Approved";
                    }
                    if (verifyType == "Rejected")
                    {
                        p.modified_by = TrackerUser.GetCurrentUser().id;
                        p.approved_by = TrackerUser.GetCurrentUser().id;
                        p.date_approved = DateTime.Today;
                        p.comments = txtComments.Text;
                        p.status = "Rejected";
                    }
            
                    else
                    {
                        p.status = verifyType;
                        p.updated_at = DateTime.Today;
                        p.modified_by = TrackerUser.GetCurrentUser().id;                      
                    }

                    //Save payment to PTas
                    if (saveToPTas) { TrackerPayment.push_payment_to_PTAS(p.transaction_ref_no); }
                }

                Database.Tracker.SubmitChanges();
                
                
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtCompanyCode.Text != string.Empty)
            {
                if ((chkReturned.IsChecked == false))
                {
                    if(btnSave.Content.ToString() == "Save")
                    {
                        String conf_str = "Transaction Details \n\n";
                        conf_str += "Transaction Ref No. : "+txtTransRef.Text+"\n";
                        conf_str += "Transaction Date : "+txtTransDate.Text+"\n";
                        conf_str += "Subscription/Value Date : "+txtValueDate.Text+"\n";
                        conf_str += "Amount : " + txtTranAmount.Text + "\n";
                        conf_str += "Identified Company : " + cmbCompanies.Text + "\n";
                        conf_str += "Savings Booster Customer : "+ cmbClient.Text +"\n\n";
                        conf_str += "Please click Yes to commit changes to this transaction.";

                        if (MessageBox.Show(conf_str, "Confirm Identification", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            if (update_payment("Identified",false)){
                                // resolve any pending rejected tickets
                                TrackerNotification.ResolveByJob(Constants.NF_TYPE_SUBSCRIPTION_APPROVAL_REJECTED,
                                                                 Constants.JOB_TYPE_SUBSCRIPTION, subID);
                                TrackerNotification.Add(Constants.ROLES_SUPER_OPS_USER_ID,
                                                        Constants.NF_TYPE_SUBSCRIPTION_APPROVAL_REQUEST,
                                                        Constants.JOB_TYPE_SUBSCRIPTION, subID);
                                MessageBox.Show("Payment has been flagged as identified.", "Identified", MessageBoxButton.OK, MessageBoxImage.Information); 
                                this.Close(); 
                            }
                        }
                        else{ MessageBox.Show("No changes have been committed to this transaction."); }
                    }
                    else if (btnSave.Content.ToString() == "Approve")
                    {
                        if (MessageBox.Show("This subscription would be flagged as identified and approved, please click Yes to proceed.", "Identified and Approved Subscription", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            if (update_payment("Identified and Approved",true)) {
                                
                                
                                TrackerNotification.ResolveByJob(Constants.NF_TYPE_SUBSCRIPTION_APPROVAL_REQUEST, Constants.JOB_TYPE_SUBSCRIPTION, subID);
                                MessageBox.Show("Payment has been flagged as approved.", "Approved", MessageBoxButton.OK, MessageBoxImage.Information); this.Close(); }
                        }
                    }
                }
                else
                {
                    if (MessageBox.Show("This subscription would be flagged as returned, please click Yes to proceed.", "Returened Subscription", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        if (update_payment("Returned",false)){MessageBox.Show("Payment has been flagged as returned.", "Identified", MessageBoxButton.OK, MessageBoxImage.Information); this.Close();}
                    }
                }
            }
            else
            {
                MessageBox.Show("Please specify a company.");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if(btnCancel.Content.ToString() == "Cancel"){ this.Close(); }
            else if (btnCancel.Content.ToString() == "Reject")
            {
                if(txtComments.Text == string.Empty)
                {
                    MessageBox.Show("Please comment on the reason for the rejection.","Comment Required",MessageBoxButton.OK,MessageBoxImage.Exclamation);
                    txtComments.Focus();
                }
                else if (MessageBox.Show("This subscription would be flagged as rejected, please click Yes to proceed.", "Rejected Subscription", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (update_payment("Rejected",false)) {
                        TrackerNotification.ResolveByJob(Constants.NF_TYPE_SUBSCRIPTION_APPROVAL_REQUEST, Constants.JOB_TYPE_SUBSCRIPTION, subID);
                        TrackerNotification.Add(Constants.ROLES_OPS_USER_ID,
                                                Constants.NF_TYPE_SUBSCRIPTION_APPROVAL_REJECTED,
                                                Constants.JOB_TYPE_SUBSCRIPTION, subID);
                        MessageBox.Show("Payment has been flagged as rejected.", "Rejected", MessageBoxButton.OK, MessageBoxImage.Information); this.Close(); }
                }
            }
            
        }

        private void btnReturned_Click(object sender, RoutedEventArgs e)
        {
            if (update_payment("Returned",false))
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void Close()
        {
            if (_loadedInFlyout)
                close_flyout();
        }
        private void close_flyout()
        {
            Window parentWindow = Window.GetWindow(this);
            object obj = parentWindow.FindName("surrogateFlyout");
            Flyout flyout = (Flyout)obj;
            flyout.Content = null;
            flyout.IsOpen = !flyout.IsOpen;
        }

        private void load_payment_deal_descriptions()
        {
            dgPaymentDealDescriptions.ItemsSource = TrackerPayment.GetPaymentDealDescriptions(subID);
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            subscriptions_verify_deal_description deal = new subscriptions_verify_deal_description(subID, false);
            deal.ShowDialog();
            load_payment_deal_descriptions();
        }

        private void dgPaymentDealDescriptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void dgPaymentDealDescriptions_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgPaymentDealDescriptions.SelectedItem != null)
            {
                TrackerPaymentDealDescriptions p = (TrackerPaymentDealDescriptions)dgPaymentDealDescriptions.SelectedItem;
                subscriptions_verify_deal_description openEdit = new subscriptions_verify_deal_description((int)p.id, true);
                openEdit.ShowDialog();
            }
        }

    }
}
