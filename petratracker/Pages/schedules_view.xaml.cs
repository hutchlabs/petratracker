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

        private void ShowValidationEmailPanel(object sender, RoutedEventArgs e)
        {
            CloseAllPanels();
            this.panelValidationEmail.Visibility =  Visibility.Visible;
        }

        private void ShowEscalationIssuePanel(object sender, RoutedEventArgs e)
        {
            CloseAllPanels();
            this.panelEscalationIssue.Visibility = Visibility.Visible;
        }

        private void ShowReportReminderPanel(object sender, RoutedEventArgs e)
        {
            CloseAllPanels();

            if (_schedule.resolution_reminder1_date != null)
            {
                this.lbl_dofr1.Visibility = Visibility.Visible;
                this.lbl_dofrv1.Visibility = Visibility.Visible;
                this.lbl_dofrv1.Content = _schedule.resolution_reminder1_date.ToString();
            }

            if (_schedule.resolution_reminder2_date != null)
            {
                this.lbl_dofr2.Visibility = Visibility.Visible;
                this.lbl_dofrv2.Visibility = Visibility.Visible;
                this.lbl_dofrv2.Content = _schedule.resolution_reminder2_date.ToString();
            }

            if (_schedule.resolution_reminder1_date != null && _schedule.resolution_reminder2_date != null)
            {
                this.lbl_reportdate.Visibility = Visibility.Collapsed;
                this.dp_reportdate.Visibility = Visibility.Collapsed;
                this.btn_report.IsEnabled = false;
            }

            UpdateButtonStatus(_schedule.workflow_status, _schedule.validation_email_sent, _schedule.escalation_email_sent, _schedule.internally_resolved, _schedule.receipt_sent, _schedule.file_downloaded);

            this.btn_reportreminder.IsEnabled = false;
            this.btn_reportreminder.Content = (this.panelReport.Visibility == Visibility.Collapsed) ? "Report Reminder" : "Reporting Reminder";
            this.panelReport.Visibility = (this.panelReport.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ShowResolveIssuePanel(object sender, RoutedEventArgs e)
        {
            CloseAllPanels();

            UpdateButtonStatus(_schedule.workflow_status, _schedule.validation_email_sent, _schedule.escalation_email_sent, _schedule.internally_resolved, _schedule.receipt_sent, _schedule.file_downloaded);


            this.btn_resolveissue.Content = (this.panelResolution.Visibility == Visibility.Collapsed) ? "Resolve Issue" : "Resolving Issue";
            this.btn_resolveissue.IsEnabled = false;
            this.panelResolution.Visibility = (this.panelResolution.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ShowIntResolveIssuePanel(object sender, RoutedEventArgs e)
        {
            CloseAllPanels();
            this.panelIntResolution.Visibility = Visibility.Visible;
        }

        private void cbx_resolutiontype_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem) cbx_resolutiontype.SelectedItem;
            lbl_tn.Visibility = (item.Content.ToString().Equals("Microgen")) ? Visibility.Visible : Visibility.Collapsed;
            tb_resolutioninfo.Visibility = (item.Content.ToString().Equals("Microgen")) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void btn_MarkValidationEmailSent(object sender, RoutedEventArgs e)
        {
            if (dp_validationemaildate.SelectedDate == null)
            {
                MessageBox.Show("Please select a date");
                dp_validationemaildate.Focus();
            }
            else
            {
                _schedule = TrackerSchedule.MarkValidationEmailSent(_schedule, (DateTime) dp_validationemaildate.SelectedDate);
                this.panelValidationEmail.Visibility = Visibility.Collapsed;
                this.btn_validationemail.Content = "Validation Email Sent";
                load_schedule();
            }
        }

        private void btn_MarkEscalationIssueSent(object sender, RoutedEventArgs e)
        {
            if (dp_escalationdate.SelectedDate == null)
            {
                MessageBox.Show("Please select a date");
                dp_escalationdate.Focus();
            }
            else
            {
                _schedule = TrackerSchedule.MarkIssueEscalation(_schedule, (DateTime)dp_escalationdate.SelectedDate);
                this.panelEscalationIssue.Visibility = Visibility.Collapsed;
                this.btn_escalationissue.Content = "Issue Escalated";
                load_schedule();
            }
        }

        private void btn_ReportReminder(object sender, RoutedEventArgs e)
        {
            if (dp_reportdate.SelectedDate == null)
            {
                MessageBox.Show("Please select a date");
                dp_reportdate.Focus();
            }
            else
            {
                if (_schedule.resolution_reminder1_date == null)
                {
                    _schedule.resolution_reminder1_date = dp_reportdate.SelectedDate;
                }
                else 
                { 
                    _schedule.resolution_reminder2_date = dp_reportdate.SelectedDate;
                }
                _schedule = TrackerSchedule.EvaluateReminderStatus(_schedule);
                
                this.panelReport.Visibility = Visibility.Collapsed;
                this.btn_reportreminder.Content = "Report Reminder";

                load_schedule();
            }
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

        private void btn_InternallyResolveIssue(object sender, RoutedEventArgs e)
        {
            if (dp_intresolvedate.SelectedDate == null)
            {
                MessageBox.Show("Please select a date");
                dp_intresolvedate.Focus();
            }
            else
            {
                _schedule = TrackerSchedule.InternallyResolveScheduleIssue(_schedule, (DateTime)dp_intresolvedate.SelectedDate);
                this.panelIntResolution.Visibility = Visibility.Collapsed;
                this.btn_intresolveissue.Content = "Interally Resolved";
                load_schedule();
            }
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

        #endregion

        #region Private Helper Methods

        private void load_schedule()
        {
            this.lbl_company.Content = _schedule.company;
            this.lbl_month.Content = string.Format("GHC {0} for {1}, {2}", _schedule.amount, CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_schedule.month), _schedule.year);
            this.lbl_owner.Content = string.Format("Owner: {0} {1}", _schedule.User.first_name, _schedule.User.last_name);
            this.lbl_tier.Content = _schedule.tier;
            this.lbl_status.Content = _schedule.workflow_status;
            this.lbl_lastupdated.Content = string.Format("Last updated on {0}",_schedule.updated_at.ToString());
            this.status_summary.Text = _schedule.workflow_summary;

            UpdateButtonStatus(_schedule.workflow_status, _schedule.validation_email_sent, _schedule.escalation_email_sent, _schedule.internally_resolved, _schedule.receipt_sent, _schedule.file_downloaded);
        }

        private void CloseAllPanels()
        {
            this.panelValidationEmail.Visibility = Visibility.Collapsed;
            this.panelEscalationIssue.Visibility = Visibility.Collapsed;
            this.panelReport.Visibility = Visibility.Collapsed;
            this.panelResolution.Visibility = Visibility.Collapsed;
            this.panelIntResolution.Visibility = Visibility.Collapsed;
        }

        private void UpdateButtonStatus(string status, bool? vemail_sent, bool? esemail_sent, bool? int_resolve, bool receipt_sent, bool file_downloaded)
        {
            string[] validValidationStates = { Constants.WF_VALIDATION_NOTDONE, Constants.WF_VALIDATION_NOTDONE_REMINDER };
            string[] validResolveIssueStates = { Constants.WF_STATUS_ERROR_SSNIT, Constants.WF_STATUS_ERROR_NAME,  Constants.WF_STATUS_ERROR_SSNIT_NAME,
                                                 Constants.WF_STATUS_ERROR_ALL, Constants.WF_STATUS_ERROR_ESCALATED};

            string[] validReceiptStates = { Constants.WF_STATUS_PAYMENTS_LINKED, Constants.WF_STATUS_PAYMENTS_RECEIVED, Constants.WF_STATUS_RF_NOSENT_DOWNLOAD_NOUPLOAD, Constants.WF_STATUS_RF_NOSENT_DOWNLOAD_UPLOAD };
            
            string[] validFiledownloadStates = { Constants.WF_STATUS_PAYMENTS_LINKED, Constants.WF_STATUS_PAYMENTS_RECEIVED, Constants.WF_STATUS_RF_SENT_NODOWNLOAD_NOUPLOAD };

            if ( ((bool) vemail_sent))
            {
                btn_validationemail.IsEnabled =  false;
                btn_validationemail.Content = "Validation Email Sent";
            } else {
                btn_validationemail.IsEnabled = (validValidationStates.Contains(status)) ? true : false;
            }

            if (((bool)esemail_sent))
            {
                btn_escalationissue.IsEnabled = false;
                btn_escalationissue.Content = "Issue Escalated";
            }
            else
            {
                btn_escalationissue.IsEnabled = (validValidationStates.Contains(status) || validResolveIssueStates.Contains(status)) ? true : false;
            }

            if (((bool)int_resolve))
            {
                btn_intresolveissue.IsEnabled = false;
                btn_intresolveissue.Content = "Resolved Internally";
            }
            else
            {
                btn_intresolveissue.IsEnabled = (validResolveIssueStates.Contains(status)) ? true : false; 
            }

            if (validResolveIssueStates.Contains(status))
            {
                this.btn_reportreminder.IsEnabled = true;
                this.btn_resolveissue.IsEnabled = true;
                this.btn_markfiledownload.IsEnabled = false;
                this.btn_markreceiptsent.IsEnabled = false;
            }

            if (this.btn_resolveissue.IsEnabled==false)
            {
                if (validReceiptStates.Contains(status))
                {
                    this.btn_resolveissue.IsEnabled = false;
                    this.btn_reportreminder.IsEnabled = false;
                    this.btn_markreceiptsent.IsEnabled = true;
                }
                if (validFiledownloadStates.Contains(status))
                {
                    this.btn_resolveissue.IsEnabled = false;
                    this.btn_reportreminder.IsEnabled = false;
                    this.btn_markfiledownload.IsEnabled = true;
                }

                if (_schedule.resolution_reminder1_date != null && _schedule.resolution_reminder2_date != null)
                {
                    this.btn_reportreminder.IsEnabled = false; this.btn_reportreminder.Content = "All Reminders Sent";
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
