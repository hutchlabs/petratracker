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
using System.Windows.Navigation;
using System.Windows.Shapes;
using petratracker.Models;
using petratracker.Utility;
using petratracker.Pages;
using MahApps.Metro.Controls;
using System.Threading;

namespace petratracker.Controls
{
    public partial class Schedules : UserControl
    {
        #region Private Members

        private CancellationTokenSource _stopGridUpgrade = new CancellationTokenSource();
        private readonly string[] _scheduleFilterOptions = { 
                                                             "All", 
                                                             Constants.WF_VALIDATION_NOTDONE,
                                                             Constants.WF_VALIDATION_NOTDONE_REMINDER,
                                                             Constants.WF_VALIDATION_DONE_EMAILSENT,
                                                             Constants.WF_STATUS_ERROR_SSNIT,
                                                             Constants.WF_STATUS_ERROR_NAME,
                                                             Constants.WF_STATUS_ERROR_SSNIT_NAME,
                                                             Constants.WF_STATUS_ERROR_ALL,
                                                             Constants.WF_STATUS_ERROR_ESCALATED,
                                                             Constants.WF_STATUS_REVALIDATE,
                                                             Constants.WF_STATUS_EXPIRED,
                                                             Constants.WF_STATUS_PASSED_NEW_EMPLOYEE,
                                                             Constants.WF_STATUS_PAYMENTS_PENDING,
                                                             Constants.WF_STATUS_PAYMENTS_LINKED,
                                                             Constants.WF_STATUS_PAYMENTS_RECEIVED,
                                                             Constants.WF_STATUS_RF_SENT_NODOWNLOAD_NOUPLOAD,
                                                             Constants.WF_STATUS_RF_SENT_DOWNLOAD_NOUPLOAD,
                                                             Constants.WF_STATUS_RF_NOSENT_DOWNLOAD_NOUPLOAD,
                                                             Constants.WF_STATUS_RF_NOSENT_DOWNLOAD_UPLOAD,
                                                             Constants.WF_STATUS_COMPLETED
                                                           };

        #endregion

        #region Public Properties

        public string[] ScheduleFilterOptions
        {
            get { return _scheduleFilterOptions; }
            private set { ;  }
        }

        public IEnumerable<ComboBoxPairs> Companies
        {
            private set { ; }
            get { return TrackerSchedule.GetCompanies(); }
        }

        #endregion

        #region Constructor

        public Schedules()
        {
            this.DataContext = this;  
            InitializeComponent();
        }

        #endregion

        #region Event Handlers
        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            StartUpgradeGridService();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            _stopGridUpgrade.Cancel(false);
        }

        private void ScheduleListFilter_Click(object sender, RoutedEventArgs e)
        {
            UpdateGrid();
        }

        private void ScheduleListFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void cbx_companies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cbx_companies.SelectedIndex != -1) { 
                    txtQuery.Text = "";
                }

                string v = ((ComboBoxPairs)cbx_companies.SelectedItem)._Value;
                if (v != "")
                {
                    string filter = (string)((SplitButton)ScheduleListFilter).SelectedItem;
                    viewSchedules.ItemsSource = TrackerSchedule.GetScheduleByCompany(v, filter);
                    UpdateGrid(true);
                }
            }
            catch (Exception){}
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
                    string filter = (string)((SplitButton)ScheduleListFilter).SelectedItem;
                    viewSchedules.ItemsSource = TrackerSchedule.GetScheduleBySearch(v, filter);
                    UpdateGrid(true);
                }
                else
                {
                    MessageBox.Show("Please enter a search term", "Error Schedule search", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception){}
        }

        private void viewSchedules_SelectionChanged(object sender, MouseButtonEventArgs e)
        {
            if (viewSchedules.SelectedItem != null)
            {
                Schedule j = viewSchedules.SelectedItem as Schedule;
                ShowActionBarButtons(j.workflow_status);
            }
        }

        private void viewSchedules_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Schedule j = viewSchedules.SelectedItem as Schedule;

                Window parentWindow = Window.GetWindow(this);
                object obj = parentWindow.FindName("surrogateFlyout");
                Flyout flyout = (Flyout)obj;

                flyout.ClosingFinished += flyout_ClosingFinished;
                flyout.Content = new ScheduleView(j.id, true);
                flyout.IsOpen = !flyout.IsOpen;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void btn_groupMarkReceiptSent_Click(object sender, RoutedEventArgs e)
        {
            string[] validReceiptStates = {  Constants.WF_STATUS_PAYMENTS_RECEIVED,
                                             Constants.WF_STATUS_PAYMENTS_LINKED,
                                             Constants.WF_STATUS_RF_NOSENT_DOWNLOAD_NOUPLOAD,
                                             Constants.WF_STATUS_RF_NOSENT_DOWNLOAD_UPLOAD,
                                          };
            MessageBoxResult rs = MessageBox.Show("Are you want to mark all of these as Receipt Sent?", "Schedules Update",MessageBoxButton.YesNo,MessageBoxImage.Question);

            if (rs == MessageBoxResult.Yes)
            {           
                foreach (var item in viewSchedules.SelectedItems)
                {
                    if (validReceiptStates.Contains(((Schedule)item).workflow_status))
                    {
                        TrackerSchedule.MarkReceiptSent((Schedule)item);
                    }
                }
                UpdateGrid();
            }
        }

        private void btn_groupMarkValidationEmailSent_Click(object sender, RoutedEventArgs e)
        {
            DateTime now = DateTime.Now;

             MessageBoxResult rs = MessageBox.Show("Are you want to mark all of these as Validation Email sent?", "Schedules Update", MessageBoxButton.YesNo, MessageBoxImage.Question);

             if (rs == MessageBoxResult.Yes)
             {
                 foreach (var item in viewSchedules.SelectedItems)
                 {
                     TrackerSchedule.MarkValidationEmailSent(((Schedule)item), now);
                 }
                 UpdateGrid();
             }
            
        }

        private void btn_groupMarkFileDownload_Click(object sender, RoutedEventArgs e)
        {
            string[] validFiledownloadStates = { Constants.WF_STATUS_PAYMENTS_RECEIVED,
                                                 Constants.WF_STATUS_PAYMENTS_LINKED,
                                                 Constants.WF_STATUS_RF_SENT_NODOWNLOAD_NOUPLOAD
                                                };

            MessageBoxResult rs = MessageBox.Show("Are you want to mark all of these as File Downloaded?", "Schedules Update", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (rs == MessageBoxResult.Yes)
            {
                foreach (var item in viewSchedules.SelectedItems)
                {
                    if (validFiledownloadStates.Contains(((Schedule)item).workflow_status))
                    {
                        TrackerSchedule.MarkFileDownloaded((Schedule)item);
                    }
                }
                UpdateGrid();
            }
        }

        private void btn_groupDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult rs = MessageBox.Show("Are you want to delete the selected schedules?", "Schedules Update", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (rs == MessageBoxResult.Yes)
            {
                foreach (var item in viewSchedules.SelectedItems)
                {
                        TrackerSchedule.DeleteSchedule((Schedule)item);
                }
                UpdateGrid();
            }
        }

        private void btn_showAddSchedule_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            object obj = parentWindow.FindName("surrogateFlyout");
            Flyout flyout = (Flyout) obj;

            flyout.ClosingFinished += flyout_ClosingFinished;
            flyout.Content = new AddSchedule(true);
            flyout.IsOpen = !flyout.IsOpen;
        }

        private void flyout_ClosingFinished(object sender, RoutedEventArgs e)
        {
            UpdateGrid();
        }

        #endregion

        #region Private Methods

        private async void StartUpgradeGridService()
        {
            var dueTime = TimeSpan.FromSeconds(0);
            var interval = TimeSpan.FromSeconds(60);
            try
            {
                await Utils.DoPeriodicWorkAsync(new Func<bool>(UpdateGrid), dueTime, interval, _stopGridUpgrade.Token);
            }
            catch (Exception) { }
        }

        private bool UpdateGrid()
        {
            return UpdateGrid(false);
        }

        private bool UpdateGrid(bool search)
        {
            string filter = (string)((SplitButton)ScheduleListFilter).SelectedItem;

            if (!search)
            {
                // Get items
                string v;
                try {  v = ((ComboBoxPairs)cbx_companies.SelectedItem)._Value; } catch (Exception) { v = "";  }

                if (txtQuery.Text != "") { viewSchedules.ItemsSource = TrackerSchedule.GetScheduleBySearch(txtQuery.Text, filter); }
                else if (v != "") { viewSchedules.ItemsSource = TrackerSchedule.GetScheduleByCompany(v, filter);  }
                else if (filter == "All" || filter == null) { viewSchedules.ItemsSource = TrackerSchedule.GetSchedules(); }
                else { viewSchedules.ItemsSource = TrackerSchedule.GetScheduleByStatus(filter); }
            }

            lbl_scheduleCount.Content = string.Format("{0} Schedules", viewSchedules.Items.Count);

            // Highlight items if checked.
            if (this.chx_schedulefilter.IsChecked == true)
            {
                ShowActionBarButtons(filter);
                viewSchedules.SelectAll();
            }
            else
            {
                ShowActionBarButtons();
                viewSchedules.UnselectAll();
            }
            return true;
        }

        private bool ShowActionBarButtons(string filter="")
        {
            bool activebuttons = false;

            btn_groupDelete.Visibility = Visibility.Collapsed;
            btn_groupMarkValidationEmailSent.Visibility = Visibility.Collapsed;
            btn_groupMarkReceiptSent.Visibility = Visibility.Collapsed;
            btn_groupMarkFileDownload.Visibility = Visibility.Collapsed;

            if (TrackerUser.IsCurrentUserSuperParser())
            {
                btn_groupDelete.Visibility = Visibility.Visible;
                activebuttons = true;
            }

            if (filter=="All" || filter == Constants.WF_STATUS_PAYMENTS_RECEIVED || filter==Constants.WF_STATUS_PAYMENTS_LINKED)
            {
                btn_groupMarkReceiptSent.Visibility = Visibility.Visible;
                btn_groupMarkFileDownload.Visibility = Visibility.Visible;
                btn_groupMarkValidationEmailSent.Visibility = Visibility.Visible;
                activebuttons = true;
            } else if (filter==Constants.WF_STATUS_RF_SENT_NODOWNLOAD_NOUPLOAD){
                btn_groupMarkFileDownload.Visibility = Visibility.Visible;
                btn_groupMarkValidationEmailSent.Visibility = Visibility.Visible;
                activebuttons = true;
            } else if ((filter==Constants.WF_STATUS_RF_NOSENT_DOWNLOAD_NOUPLOAD) || filter==Constants.WF_STATUS_RF_NOSENT_DOWNLOAD_UPLOAD) {
                btn_groupMarkReceiptSent.Visibility = Visibility.Visible;
                btn_groupMarkValidationEmailSent.Visibility = Visibility.Visible;
                activebuttons = true;
            }

             if (activebuttons)
             {
                 actionBar.Visibility = Visibility.Visible;
             }
             else
             {
                 actionBar.Visibility = Visibility.Collapsed;
             }

            return activebuttons;
        }
   
        #endregion

 
    }
}
