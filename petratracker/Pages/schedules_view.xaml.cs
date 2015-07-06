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
using System.Globalization;

namespace petratracker.Pages
{
    public partial class ScheduleView : Page
    {
        #region Private Members
        
        private int _schedule_id;
        private Schedules _sch;
        
        #endregion

        public ScheduleView()
        {
            InitializeComponent();
        }

        public ScheduleView(int id, Schedules s) : this()
        {
            _schedule_id = id;
            _sch = s;
        }
   
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

            if (s.workflow_status.Contains("No upload though."))
            {
                this.upload.IsEnabled = true;
            }

            if (s.receipt_sent) { this.sentr.IsEnabled = false; this.sentr.Content = "Receipt Sent"; }
            if (s.file_downloaded) { this.download.IsEnabled = false; this.download.Content = "File Downloaded"; }
            if (s.file_uploaded) { this.upload.IsEnabled = false; this.upload.Content = "File Uploaded"; }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            load_schedule();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _sch.SwitchToGrid();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Schedule s = TrackerSchedule.GetSchedule(_schedule_id);
            s.receipt_sent = true;
            s.receipt_sent_date = DateTime.Now;
            s.workflow_status = "Schedule linked to payment and receipt sent";
            TrackerSchedule.Save(s);
            this.sentr.IsEnabled = false;
            this.status_summary.Text = TrackerSchedule.GetSummary(_schedule_id);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Schedule s = TrackerSchedule.GetSchedule(_schedule_id);
            s.file_downloaded = true;
            s.file_downloaded_date = DateTime.Now;
            s.workflow_status = "Schedule linked to payment, receipt sent and file downloaded";
            TrackerSchedule.Save(s);
            this.download.IsEnabled = false;
            this.status_summary.Text = TrackerSchedule.GetSummary(_schedule_id);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Schedule s = TrackerSchedule.GetSchedule(_schedule_id);
            s.file_uploaded = true;
            s.file_uploaded_date = DateTime.Now;
            s.workflow_status = "Complete";
            TrackerSchedule.Save(s);
            this.upload.IsEnabled = false;
            this.status_summary.Text = TrackerSchedule.GetSummary(_schedule_id);
        }
    }
}
