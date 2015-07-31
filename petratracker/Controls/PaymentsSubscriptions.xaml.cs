using MahApps.Metro.Controls;
using petratracker.Models;
using petratracker.Pages;
using petratracker.Utility;
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

        private readonly string[] _jobsOptions = { "All", Constants.PAYMENT_STATUS_APPROVED, Constants.PAYMENT_STATUS_INPROGRESS }; 

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
     
        private void viewSubs_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PPayment p = (PPayment) viewSubs.SelectedItem;

            _activeJobId = (int) p.job_id;

            Window parentWindow = Window.GetWindow(this);
            object obj = parentWindow.FindName("surrogateFlyout");
            Flyout flyout = (Flyout)obj;

            flyout.ClosingFinished += subsflyout_ClosingFinished;
            flyout.Content = new verifySubscription(p.status.Trim(), p.id, true);
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
            }
            else
            {
                ShowJobsActionBarButtons();
                viewJobs.UnselectAll();
                if (viewJobs.Items.Count > 0) { viewJobs.SelectedIndex = 0; }
            }
        }

        private void UpdateSubscriptions(bool useActiveId=false)
        {
            Job job = (Job)viewJobs.SelectedItem;

            if (job != null)
            {
                string filter = (string)((SplitButton)SubsListFilter).SelectedItem;
                filter = (filter == null) ? "" : filter;

                int id = (useActiveId) ? _activeJobId : job.id;

                if (filter == "All") { viewSubs.ItemsSource = TrackerPayment.GetSubscriptions(id); }
                else { viewSubs.ItemsSource = TrackerPayment.GetSubscriptions(id, filter); }

                /*
                viewSubs.Columns[0].Visibility = System.Windows.Visibility.Hidden;
                viewSubs.Columns[1].Visibility = System.Windows.Visibility.Hidden;
                viewSubs.Columns[4].Visibility = System.Windows.Visibility.Hidden;
                viewSubs.Columns[5].Visibility = System.Windows.Visibility.Hidden;
                viewSubs.Columns[6].Visibility = System.Windows.Visibility.Hidden;
                viewSubs.Columns[7].Visibility = System.Windows.Visibility.Hidden;
                viewSubs.Columns[8].Visibility = System.Windows.Visibility.Hidden;
                viewSubs.Columns[10].Visibility = System.Windows.Visibility.Hidden;
                viewSubs.Columns[11].Visibility = System.Windows.Visibility.Hidden;
                viewSubs.Columns[12].Visibility = System.Windows.Visibility.Hidden;
                viewSubs.Columns[13].Visibility = System.Windows.Visibility.Hidden;
                viewSubs.Columns[14].Visibility = System.Windows.Visibility.Hidden;
                viewSubs.Columns[15].Visibility = System.Windows.Visibility.Hidden;
                viewSubs.Columns[16].Visibility = System.Windows.Visibility.Hidden;
                */

                lbl_subsCount.Content = string.Format("{0} Subscriptions", viewSubs.Items.Count);

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

        private bool ShowJobsActionBarButtons(string filter="")
        {
            bool activebuttons = false;

            btn_approveJobs.Visibility = Visibility.Collapsed;

            if (filter == "All" || filter == Constants.PAYMENT_STATUS_INPROGRESS)
            {
                btn_approveJobs.Visibility = Visibility.Visible;
                activebuttons = true;
            }
     
            jobs_actionBar.Visibility = (activebuttons) ? Visibility.Visible : Visibility.Collapsed;
   
            return activebuttons;
        }

        private bool ShowSubsActionBarButtons(string filter="")
        {
            bool activebuttons = false;

            /*btn_approveJobs.Visibility = Visibility.Collapsed;

            if (filter == "All" || filter == Constants.PAYMENT_STATUS_INPROGRESS)
            {
                btn_approveJobs.Visibility = Visibility.Visible;
                activebuttons = true;
            }

            jobs_actionBar.Visibility = (activebuttons) ? Visibility.Visible : Visibility.Collapsed;
            */
            return activebuttons;
        }


      #endregion

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

    }
}
