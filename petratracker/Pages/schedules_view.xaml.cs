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
            _schedule = TrackerSchedule.MarkReceiptSent(_schedule);            
            load_schedule();
        }

        private void btn_MarkFileDownloaded(object sender, RoutedEventArgs e)
        {
            _schedule = TrackerSchedule.MarkFileDownloaded(_schedule);
            load_schedule();
        }

        private void ShowResolveIssue(object sender, RoutedEventArgs e)
        {
            this.panelResolution.Visibility = (this.panelResolution.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
            this.btn_resolveissue.Content = (this.panelResolution.Visibility == Visibility.Collapsed) ? "Resolve Issue" : "Resolving Issue";
            this.btn_resolveissue.IsEnabled = false;
        }

        private void cbx_resolutiontype_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem) cbx_resolutiontype.SelectedItem;
            lbl_tn.Visibility = (item.Content.ToString().Equals("Microgen")) ? Visibility.Visible : Visibility.Collapsed;
            tb_resolutioninfo.Visibility = (item.Content.ToString().Equals("Microgen")) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void btn_ResolveIssue(object sender, RoutedEventArgs e)
        {
            if (dp_resolutiondate.SelectedDate == null) 
            {
                MessageBox.Show("Please select a date");
                dp_resolutiondate.Focus();
            }
            else if (cbx_resolutiontype.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a resolution type");
                cbx_resolutiontype.Focus();
            }
            else if (tb_resolutioninfo.Visibility==Visibility.Visible && tb_resolutioninfo.Text==string.Empty)
            {
                MessageBox.Show("Please enter a ticket number");
                tb_resolutioninfo.Focus();
            }
            else
            {
                _schedule.resolution_date = dp_resolutiondate.SelectedDate;
                _schedule.resolution_type = cbx_resolutiontype.SelectedItem as string;
                _schedule.resolution_info = tb_resolutioninfo.Text;
                _schedule = TrackerSchedule.ResolveScheduleIssue(_schedule);

                this.panelResolution.Visibility = Visibility.Collapsed;
                this.btn_resolveissue.Content = "Resolve Issue";

                load_schedule();
            }
        }

        #endregion

        #region Private Helper Methods

        private void load_schedule()
        {
            this.lbl_company.Content = _schedule.company;
            this.lbl_month.Content = string.Format("Contribution for {0}, {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_schedule.month), _schedule.year);
            this.lbl_owner.Content = string.Format("Owner: {0} {1}", _schedule.User.first_name, _schedule.User.last_name);
            this.lbl_tier.Content = _schedule.tier;
            this.lbl_status.Content = _schedule.workflow_status;
            this.lbl_lastupdated.Content = string.Format("Last updated on {0}",_schedule.updated_at.ToString());
            this.status_summary.Text = _schedule.workflow_summary;

            UpdateButtonStatus(_schedule.workflow_status, _schedule.receipt_sent, _schedule.file_downloaded);
        }

        private void UpdateButtonStatus(string status, bool receipt_sent, bool file_downloaded)
        {
            string[] validResolveIssueStates = { Constants.WF_STATUS_ERROR_SSNIT, Constants.WF_STATUS_ERROR_NAME,  Constants.WF_STATUS_ERROR_SSNIT_NAME,
                                                 Constants.WF_STATUS_ERROR_NEW_EMPLOYEE, Constants.WF_STATUS_ERROR_ALL, Constants.WF_STATUS_ERROR_ESCALATED};

            string[] validReceiptStates = { Constants.WF_STATUS_PAYMENTS_RECEIVED, Constants.WF_STATUS_RF_NOSENT_DOWNLOAD_NOUPLOAD, Constants.WF_STATUS_RF_NOSENT_DOWNLOAD_UPLOAD };
            
            string[] validFiledownloadStates = { Constants.WF_STATUS_PAYMENTS_RECEIVED, Constants.WF_STATUS_RF_SENT_NODOWNLOAD_NOUPLOAD };

            if (validResolveIssueStates.Contains(status))
            {
                this.btn_resolveissue.IsEnabled = true;
                this.btn_markfiledownload.IsEnabled = false;
                this.btn_markreceiptsent.IsEnabled = false;
            }
            if (this.btn_resolveissue.IsEnabled==false)
            {
                if (validReceiptStates.Contains(status))
                {
                    this.btn_resolveissue.IsEnabled = false;
                    this.btn_markreceiptsent.IsEnabled = true;
                }
                if (validFiledownloadStates.Contains(status))
                {
                    this.btn_resolveissue.IsEnabled = false;
                    this.btn_markfiledownload.IsEnabled = true;
                }

                if (receipt_sent) { this.btn_markreceiptsent.IsEnabled = false; this.btn_markreceiptsent.Content = "Receipt Sent"; }
                if (file_downloaded) { this.btn_markfiledownload.IsEnabled = false; this.btn_markfiledownload.Content = "File Downloaded"; }
            }
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
