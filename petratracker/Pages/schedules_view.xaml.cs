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
using petratracker.Controls;
using petratracker.Utility;
using System.Globalization;
using MahApps.Metro.Controls;

namespace petratracker.Pages
{
    public partial class ScheduleView : UserControl
    {
        #region Private Members
        
        private int _schedule_id;

        private Schedule _schedule;

        private bool _loadedInFlyout = false;
        
        #endregion

        #region Constructors

        public ScheduleView()
        {
            InitializeComponent();
        }

        public ScheduleView(int id, bool inFlyout = false) : this()
        {
            _schedule_id = id;
            _schedule = TrackerSchedule.GetSchedule(_schedule_id);
            _loadedInFlyout = inFlyout;
        }

        #endregion

        #region Event Handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            load_schedule();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_loadedInFlyout)
                close_flyout();
        }

        private void btn_MarkReceiptSent(object sender, RoutedEventArgs e)
        {
            _schedule.receipt_sent = true;
            _schedule.receipt_sent_date = DateTime.Now; 
            _schedule = TrackerSchedule.EvaluatePaymentReceivedSchedule(_schedule);

            TrackerNotification.ResolveByJob(Constants.NF_TYPE_SCHEDULE_REQUEST_RECEIPT_SEND, Constants.JOB_TYPE_SCHEDULE, _schedule.id);
            
            load_schedule();
        }

        private void btn_MarkFileDownloaded(object sender, RoutedEventArgs e)
        {
            _schedule.file_downloaded = true;
            _schedule.file_downloaded_date = DateTime.Now;
            _schedule = TrackerSchedule.EvaluatePaymentReceivedSchedule(_schedule);

            TrackerNotification.ResolveByJob(Constants.NF_TYPE_SCHEDULE_REQUEST_FILE_DOWNLOAD, Constants.JOB_TYPE_SCHEDULE, _schedule.id);

            load_schedule();
        }

        #endregion

        #region Private Helper Methods

        private void load_schedule()
        {
            Schedule s = TrackerSchedule.GetSchedule(_schedule_id);

            this.lbl_company.Content = s.company;
            this.lbl_month.Content = string.Format("{0}, {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(s.month), s.year);
            this.lbl_owner.Content = string.Format("{0} {1}", s.User.first_name, s.User.last_name);
            this.lbl_tier.Content = s.tier;
            this.status_summary.Text = TrackerSchedule.GetSummary(_schedule_id);

            if (s.workflow_status.Equals("Schedule linked to Payments. Waiting for Receipt to be sent."))
            {
                this.sentr.IsEnabled = true;
            }

            if (s.workflow_status.Contains("No download though."))
            {
                this.download.IsEnabled = true;
            }

            if (s.receipt_sent) { this.sentr.IsEnabled = false; this.sentr.Content = "Receipt Sent"; }
            if (s.file_downloaded) { this.download.IsEnabled = false; this.download.Content = "File Downloaded"; }
        }

        private void close_flyout()
        {
            Window parentWindow = Window.GetWindow(this);
            object obj = parentWindow.FindName("surrogateFlyout");
            Flyout flyout = (Flyout)obj;
            flyout.Content = null;
            flyout.IsOpen = !flyout.IsOpen;
        }

        #endregion
    }
}
