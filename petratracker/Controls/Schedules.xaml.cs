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

namespace petratracker.Controls
{
    public partial class Schedules : UserControl
    {
        #region Private Members
        
        private readonly string[] _scheduleFilterOptions = { 
                                                             "All", 
                                                             Constants.WF_VALIDATION_NOTDONE,
                                                             Constants.WF_VALIDATION_NOTDONE_REMINDER,
                                                             Constants.WF_STATUS_ERROR_SSNIT,
                                                             Constants.WF_STATUS_ERROR_NAME,
                                                             Constants.WF_STATUS_ERROR_SSNIT_NAME,
                                                             Constants.WF_STATUS_ERROR_NEW_EMPLOYEE,
                                                             Constants.WF_STATUS_ERROR_ALL,
                                                             Constants.WF_STATUS_ERROR_ESCALATED,
                                                             Constants.WF_STATUS_PAYMENTS_PENDING,
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
            UpdateGrid();
        }

        private void ScheduleListFilter_Click(object sender, RoutedEventArgs e)
        {
            UpdateGrid();
        }

        private void ScheduleListFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGrid();
        }

        private void viewSchedules_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Schedule j = viewSchedules.SelectedItem as Schedule;

            Window parentWindow = Window.GetWindow(this);
            object obj = parentWindow.FindName("surrogateFlyout");
            Flyout flyout = (Flyout)obj;

            flyout.ClosingFinished += flyout_ClosingFinished;
            flyout.Content = new ScheduleView(j.id, true);
            flyout.IsOpen = !flyout.IsOpen;
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

        private void UpdateGrid()
        {
            string filter = (string)((SplitButton)ScheduleListFilter).SelectedItem;

            // Get items
            if (filter == "All") { viewSchedules.ItemsSource = TrackerSchedule.GetSchedules(); }
            else { viewSchedules.ItemsSource = TrackerSchedule.GetScheduleByStatus(filter); }
                
            lbl_scheduleCount.Content = string.Format("{0} Schedules", viewSchedules.Items.Count);

            // Highlight items if checked.
            if (this.chx_schedulefilter.IsChecked == true)
            {
                actionBar.Visibility = Visibility.Visible;
                viewSchedules.SelectAll();
            }
            else
            {
                actionBar.Visibility = Visibility.Collapsed;
                viewSchedules.UnselectAll();
            }
        }

        #endregion
    }
}
