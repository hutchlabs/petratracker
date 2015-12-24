using MahApps.Metro.Controls;
using petratracker.Models;
using petratracker.Pages;
using petratracker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace petratracker.Controls
{
    public partial class PaymentsSubscriptions : UserControl
    {
        #region Private Members 

        private int _activeJobId;
        private bool _allJobsSelected = false;
        private readonly string[] _tiers = { "Tier 2", "Tier 3", "Tier 4" };


        private readonly string[] _jobsOptions = { "All" }; 
        //private readonly string[] _jobsOptions = { "All", Constants.PAYMENT_STATUS_APPROVED, Constants.PAYMENT_STATUS_INPROGRESS }; 

        private readonly string[] _opsUserOptions   = { "All", Constants.PAYMENT_STATUS_UNIDENTIFIED,
                                                        Constants.PAYMENT_STATUS_IDENTIFIED_APPROVED, 
                                                        Constants.PAYMENT_STATUS_RETURNED, 
                                                        Constants.PAYMENT_STATUS_REJECTED};
        private readonly string[] _superUserOptions = { "All", Constants.PAYMENT_STATUS_UNIDENTIFIED,
                                                        Constants.PAYMENT_STATUS_IDENTIFIED,
                                                        Constants.PAYMENT_STATUS_IDENTIFIED_APPROVED, 
                                                        Constants.PAYMENT_STATUS_RETURNED, 
                                                        Constants.PAYMENT_STATUS_REJECTED};
        
        #endregion

        #region Public Properties

        public string[] JobsFilterOptions
        {
            private set { ; }
            get { return _jobsOptions;  }
        }

        public string[] SubsFilterOptions
        {
            get 
            {
                return (TrackerUser.IsCurrentUserOps()) ? _opsUserOptions : _superUserOptions;
            }
            private set { ;  }
        }

        public IEnumerable<ComboBoxPairs> Companies
        {
            private set { ; }
            get { return TrackerSchedule.GetCompanies(); }
        }

        public string[] Tiers
        {
            private set { ; }
            get { return _tiers; }
        }

        public IEnumerable<ComboBoxPairs> ContributionTypes
        {
            private set { ; }
            get { return TrackerSchedule.GetContributionTypes(); }
        }

        public IEnumerable<ComboBoxPairs> Years
        {
            private set { ; }
            get { return TrackerSchedule.GetYears(); }
        }

        #endregion

        #region Constructor

        public PaymentsSubscriptions()
        {
            this.DataContext = this;
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdatePaymentJobs();
            UpdateSubscriptions();
        }

        #region Jobs Grid Methods
       
        private void JobsListFilter_Click(object sender, RoutedEventArgs e)
        {
            UpdatePaymentJobs();
        }

        private void JobsListFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePaymentJobs();
        }

        private void viewJobs_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {                    
            UpdateSubscriptions();
        }

        private void viewJobs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (viewJobs.SelectedItem != null)
            {
                Job j = viewJobs.SelectedItem as Job;
                _activeJobId = j.id;
                ShowJobsActionBarButtons(j.status.Trim());
            }
        }

        private void btn_Download_Payments_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            object obj = parentWindow.FindName("surrogateFlyout");
            Flyout flyout = (Flyout)obj;

            flyout.ClosingFinished += jobsflyout_ClosingFinished;
            flyout.Content = new subscriptions_download_microgen_data(true);
            flyout.IsOpen = !flyout.IsOpen;
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            ExportToText.doExport(ExportToText.GetDataTableFromDGV(viewSubs));
        }

        private void btn_showAddSubscription_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            object obj = parentWindow.FindName("surrogateFlyout");
            Flyout flyout = (Flyout)obj;

            flyout.ClosingFinished += jobsflyout_ClosingFinished;
            flyout.Content = new AddSubscription(true);
            flyout.IsOpen = !flyout.IsOpen;
        }

        private void btn_approveJobs_Click(object sender, RoutedEventArgs e)
        {
            string[] validStates = { Constants.PAYMENT_STATUS_INPROGRESS };
                    
            MessageBoxResult rs = MessageBox.Show("Are you want to mark all of these as Approved?", "Payments Update", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (rs == MessageBoxResult.Yes)
            {
                foreach (var item in viewJobs.SelectedItems)
                {
                    if (validStates.Contains(((Job)item).status.Trim()))
                    {
                        TrackerJobs.Approve((Job)item);
                    }
                }
                UpdatePaymentJobs();
            }
        }

        #endregion

        #region Subcriptions Grid Methods

        private void SubsListFilter_Click(object sender, RoutedEventArgs e)
        {
            UpdateSubscriptions();
        }

        private void SubsListFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSubscriptions();
        }

        private void cbx_searchfilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cbx_companies.SelectedIndex != -1 ||
                    cbx_tiers.SelectedIndex != -1 ||
                    cbx_months.SelectedIndex != -1 ||
                    cbx_years.SelectedIndex != -1 ||
                    dp_valuedate.SelectedDate != null ||
                    cbx_contributiontypes.SelectedIndex != -1)
                {
                    txtQuery.Text = "";


                    string c = (cbx_companies.SelectedIndex == -1) ? null : ((ComboBoxPairs)cbx_companies.SelectedItem)._Value;
                    string ct = (cbx_contributiontypes.SelectedIndex == -1) ? null : ((ComboBoxPairs)cbx_contributiontypes.SelectedItem)._Value;
                    string vd = null;
                    if (dp_valuedate.SelectedDate != null) { vd = ((DateTime)dp_valuedate.SelectedDate).ToString(); }
                    string t = (cbx_tiers.SelectedIndex == -1) ? null : cbx_tiers.SelectedValue.ToString();
                    string y = (cbx_years.SelectedIndex == -1) ? null : ((ComboBoxPairs)cbx_years.SelectedItem)._Value;
                    string m = (cbx_months.SelectedIndex == -1) ? null : cbx_months.SelectedValue.ToString();

                    string filter = (string)((SplitButton)SubsListFilter).SelectedItem;
                    Job job = (Job)viewJobs.SelectedItem;

                    viewSubs.ItemsSource = TrackerPayment.GetSubscriptionsBySearch(c, t, ct, vd, m, y, job.id, filter);

                    lbl_subsCount.Content = string.Format("{0} Subscriptions for {1}", viewSubs.Items.Count, job.job_description);
                }
                else
                {
                    UpdateSubscriptions(true, false);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message);  }     
        }

        private void txtQuery_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_Query_Click(sender, new RoutedEventArgs());
            }
        }

        private void btn_Query_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cbx_companies.SelectedIndex = -1;

                string v = txtQuery.Text;
                if (v != "")
                {
                    string filter = (string)((SplitButton)SubsListFilter).SelectedItem;
                    viewSubs.ItemsSource = TrackerPayment.GetSubscriptionsBySearch(v, filter);
                    UpdateSubscriptions(true, true);
                }
                else
                {
                    MessageBox.Show("Please enter a search term", "Error Schedule search", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception) { }
        }
     
        private void viewSubs_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SubscriptionsView p = (SubscriptionsView)viewSubs.SelectedItem;

            _activeJobId = (int)p.Job_Id;

            Window parentWindow = Window.GetWindow(this);
            object obj = parentWindow.FindName("surrogateFlyout");
            Flyout flyout = (Flyout)obj;

            flyout.ClosingFinished += subsflyout_ClosingFinished;
            flyout.Content = new verifySubscription(p.Status.Trim(), p.Id, true);
            flyout.IsOpen = !flyout.IsOpen;
        }

        private void viewSubs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (viewSubs.SelectedItem != null)
            {
                PPayment p = viewSubs.SelectedItem as PPayment;
                ShowSubsActionBarButtons(p.status.Trim());
            }
        }

        private void btn_approveSubs_Click(object sender, RoutedEventArgs e)
        {
            string[] validStates = { Constants.PAYMENT_STATUS_IDENTIFIED };

            MessageBoxResult rs = MessageBox.Show("Are you want to mark all Identified as Approved?", "Payments Update", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (rs == MessageBoxResult.Yes)
            {
                foreach (var item in viewSubs.SelectedItems)
                {
                    _activeJobId = (int) ((PPayment) item).job_id;
                        
                    if (validStates.Contains(((PPayment)item).status.Trim()))
                    {
                        TrackerPayment.Approve((PPayment)item);
                    }
                }
                UpdateSubscriptions(true);
            }
        }

        private void btn_rejectSubs_Click(object sender, RoutedEventArgs e)
        {
            string[] validStates = { Constants.PAYMENT_STATUS_IDENTIFIED };

            MessageBoxResult rs = MessageBox.Show("Are you want to mark all Identified as Rejected?", "Payments Update", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (rs == MessageBoxResult.Yes)
            {
                foreach (var item in viewSubs.SelectedItems)
                {
                    _activeJobId = (int)((PPayment)item).job_id;

                    if (validStates.Contains(((PPayment)item).status.Trim()))
                    {
                        TrackerPayment.Reject((PPayment)item);
                    }
                }
                UpdateSubscriptions(true);
            }
        }

        #endregion

        private void jobsflyout_ClosingFinished(object sender, RoutedEventArgs e)
        {
            UpdatePaymentJobs();
        }

        private void subsflyout_ClosingFinished(object sender, RoutedEventArgs e)
        {
            UpdateSubscriptions(true);
        }
         
        #endregion

        #region Private Helper Methods
        
        private void UpdatePaymentJobs()
        {
            string filter = (string)((SplitButton)JobsListFilter).SelectedItem;
    
            if (filter == "All") { viewJobs.ItemsSource = TrackerJobs.GetJobs(Constants.JOB_TYPE_SUBSCRIPTION); }
            else { viewJobs.ItemsSource = TrackerJobs.GetJobs(Constants.JOB_TYPE_SUBSCRIPTION, filter); }

            lbl_jobsCount.Content = string.Format("{0} Payments", viewJobs.Items.Count);

            if (viewJobs.Items.Count > 0) { viewJobs.SelectedIndex = 0; }

            if (this.chx_jobsfilter.IsChecked == true)
            {
                ShowJobsActionBarButtons(filter);
                viewJobs.SelectAll();
                _allJobsSelected = true;
                LoadAllSubs();
            }
            else
            {
                ShowJobsActionBarButtons();
                viewJobs.UnselectAll();
                _allJobsSelected = false;
                if (viewJobs.Items.Count > 0) { viewJobs.SelectedIndex = 0; }
                UpdateSubscriptions();
            }
        }

        private void LoadAllSubs()
        {
            string filter = (string)((SplitButton)SubsListFilter).SelectedItem;
            filter = (filter == null) ? "" : filter;

            if (filter == "All") { viewSubs.ItemsSource = TrackerPayment.GetAllSubscriptions(); }
            else { viewSubs.ItemsSource = TrackerPayment.GetAllSubscriptions(filter); }

            lbl_subsCount.Content = string.Format("{0} Subscriptions for all jobs", viewSubs.Items.Count);

            if (this.chx_subsfilter.IsChecked == true)
            {
                ShowSubsActionBarButtons(filter);
                viewSubs.SelectAll();
            }
            else
            {
                ShowSubsActionBarButtons();
                viewSubs.UnselectAll();
            }
        }

        private void UpdateSubscriptions(bool useActiveId=false, bool search=false)
        {
            if (!search)
            {
                if (_allJobsSelected)
                {
                    LoadAllSubs();
                }
                else
                {
                    Job job;
                    if (useActiveId)
                    {
                        job = TrackerJobs.GetJob(_activeJobId);
                    }
                    else
                    {
                        job = (Job)viewJobs.SelectedItem;
                    }

                    if (job != null)
                    {
                        string filter = (string)((SplitButton)SubsListFilter).SelectedItem;
                        filter = (filter == null) ? "" : filter;

                        string v;
                        try { v = ((ComboBoxPairs)cbx_companies.SelectedItem)._Value; }
                        catch (Exception) { v = ""; }

                        if (txtQuery.Text != "") { viewSubs.ItemsSource = TrackerPayment.GetSubscriptionsBySearch(txtQuery.Text, filter); }
                        else if (v != "") { viewSubs.ItemsSource = TrackerPayment.GetSubscriptionsByCompany(v, filter); }
                        else if (filter == "All") { viewSubs.ItemsSource = TrackerPayment.GetSubscriptions(job.id); }
                        else { viewSubs.ItemsSource = TrackerPayment.GetSubscriptions(job.id, filter); }

                        lbl_subsCount.Content = string.Format("{0} Subscriptions for {1}", viewSubs.Items.Count, job.job_description);

                        if (this.chx_subsfilter.IsChecked == true)
                        {
                            ShowSubsActionBarButtons(filter);
                            viewSubs.SelectAll();
                        }
                        else
                        {
                            ShowSubsActionBarButtons();
                            viewSubs.UnselectAll();
                        }
                    }
                }
            }
        }

        private bool ShowJobsActionBarButtons(string filter="")
        {
            bool activebuttons = false;
            /* Not allowing this funcationality for now
            btn_approveJobs.Visibility = Visibility.Collapsed;

            if (filter == "All" || filter == Constants.PAYMENT_STATUS_INPROGRESS)
            {
                btn_approveJobs.Visibility = Visibility.Visible;
                activebuttons = true;
            }
     
            jobs_actionBar.Visibility = (activebuttons) ? Visibility.Visible : Visibility.Collapsed;
            */
            return activebuttons;
        }

        private bool ShowSubsActionBarButtons(string filter="")
        {
            bool activebuttons = false;

            btn_approveSubs.Visibility = Visibility.Collapsed;
            btn_rejectSubs.Visibility = Visibility.Collapsed;

            if (TrackerUser.IsCurrentUserSuperOps() && (filter == "All" || filter == Constants.PAYMENT_STATUS_IDENTIFIED))
            {
                btn_approveSubs.Visibility = Visibility.Visible;
                btn_rejectSubs.Visibility = Visibility.Visible;
                activebuttons = true;
            }

            subs_actionBar.Visibility = (activebuttons) ? Visibility.Visible : Visibility.Collapsed;
            
            return activebuttons;
        }

      #endregion

    }
}
