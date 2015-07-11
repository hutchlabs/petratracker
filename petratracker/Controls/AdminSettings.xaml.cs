using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using petratracker.Models;
using petratracker.Utility;
using System.ComponentModel;

namespace petratracker.Controls
{
    public partial class AdminSettings : UserControl, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Error Handling Properties

        private string _smtpProperty;
        private string _emailProperty;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Error { get { return string.Empty; } }

        public string SMTPProperty
        {
            get { return this._smtpProperty; }
            set
            {
                if (Equals(value, _smtpProperty))
                {
                    return;
                }

                _smtpProperty = value;
                RaisePropertyChanged("SMTPProperty");
            }
        }

        public string EmailProperty
        {
            get { return this._emailProperty; }
            set
            {
                if (Equals(value, _emailProperty))
                {
                    return;
                }

                _emailProperty = value;
                RaisePropertyChanged("EmailProperty");
            }
        }

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "SMTPProperty" && this.SMTPProperty==string.Empty)
                {
                    return "Invalid SMTP host address. Please re-enter.";
                }

                if (columnName == "EmailProperty" && this.EmailProperty==string.Empty)
                {
                    return "Invalid email address. Please re-enter.";
                }
              
                return null;
            }
        }

        #endregion

        #region Constructor

        public AdminSettings()
        {
            this.DataContext = this;
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // set time values
                this.time_interval_updatenotifications.Value = Double.Parse(TrackerSettings.GetSetting(Constants.SETTINGS_TIME_INTERVAL_UPDATE_NOTIFICATIONS));
                this.time_interval_updateschedule.Value = Double.Parse(TrackerSettings.GetSetting(Constants.SETTINGS_TIME_INTERVAL_UPDATE_SCHEDULES));
                this.time_interval_schedulevalidation.Value = Double.Parse(TrackerSettings.GetSetting(Constants.SETTINGS_TIME_INTERVAL_VALIDATION_REQUEST));
                this.time_error_fix1.Value = Double.Parse(TrackerSettings.GetSetting(Constants.SETTINGS_TIME_ERRORFIX_1_REMINDER_WINDOW));
                this.time_error_fix2.Value = Double.Parse(TrackerSettings.GetSetting(Constants.SETTINGS_TIME_ERRORFIX_2_REMINDER_WINDOW));
                this.time_error_fix3.Value = Double.Parse(TrackerSettings.GetSetting(Constants.SETTINGS_TIME_ERRORFIX_3_REMINDER_WINDOW));
                this.time_interval_sendreceipt.Value = Double.Parse(TrackerSettings.GetSetting(Constants.SETTINGS_TIME_INTERVAL_SEND_RECEIPT));
                this.time_interval_download.Value = Double.Parse(TrackerSettings.GetSetting(Constants.SETTINGS_TIME_FILE_DOWNLOAD_INTERVAL));
                this.time_interval_upload.Value = Double.Parse(TrackerSettings.GetSetting(Constants.SETTINGS_TIME_FILE_UPLOAD_INTERVAL));
                this.time_window_upload.Value = Double.Parse(TrackerSettings.GetSetting(Constants.SETTINGS_TIME_FILE_UPLOAD_WINDOW));

                // set mail values
                this.tb_emailsmtphost.Text = TrackerSettings.GetSetting(Constants.SETTINGS_EMAIL_SMTP_HOST);
                this.tb_emailfrom.Text = TrackerSettings.GetSetting(Constants.SETTINGS_EMAIL_FROM);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().ToString());
            }
        }

        private void time_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            string setting = "";
            bool save = false;

            switch(((MahApps.Metro.Controls.NumericUpDown)sender).Name)
            {
                case "time_interval_updatenotifications": { setting = Constants.SETTINGS_TIME_INTERVAL_UPDATE_NOTIFICATIONS; save = validate_time_value(setting, e.NewValue.ToString()); break; }
                case "time_interval_updateschedule": { setting = Constants.SETTINGS_TIME_INTERVAL_UPDATE_SCHEDULES; save = validate_time_value(setting, e.NewValue.ToString()); break; }
                case "time_interval_schedulevalidation": { setting = Constants.SETTINGS_TIME_INTERVAL_VALIDATION_REQUEST; save = validate_time_value(setting, e.NewValue.ToString()); break; }
                case "time_error_fix1": { setting = Constants.SETTINGS_TIME_ERRORFIX_1_REMINDER_WINDOW; save = validate_time_value(setting, e.NewValue.ToString()); break; }
                case "time_error_fix2": { setting = Constants.SETTINGS_TIME_ERRORFIX_2_REMINDER_WINDOW; save = validate_time_value(setting, e.NewValue.ToString()); break; }
                case "time_error_fix3": { setting = Constants.SETTINGS_TIME_ERRORFIX_3_REMINDER_WINDOW; save = validate_time_value(setting, e.NewValue.ToString()); break; }
                case "time_interval_sendreceipt": { setting = Constants.SETTINGS_TIME_INTERVAL_SEND_RECEIPT; save = validate_time_value(setting, e.NewValue.ToString()); break; }
                case "time_interval_download": { setting = Constants.SETTINGS_TIME_FILE_DOWNLOAD_INTERVAL; save = validate_time_value(setting, e.NewValue.ToString()); break; }
                case "time_interval_upload": { setting = Constants.SETTINGS_TIME_FILE_UPLOAD_INTERVAL; save = validate_time_value(setting, e.NewValue.ToString()); break; }
                case "time_window_upload": { setting = Constants.SETTINGS_TIME_FILE_UPLOAD_WINDOW; save = validate_time_value(setting, e.NewValue.ToString()); break; }
                default: save = false; break;
            }

            if (save)
            {
                //string val = (((MahApps.Metro.Controls.NumericUpDown)sender).Name.Equals("time_interval_updatenotifications")) ? e.NewValue *  : e.NewValue.ToString();
                TrackerSettings.Save(setting, e.NewValue.ToString());
            }
            else
            {
                ((MahApps.Metro.Controls.NumericUpDown)sender).Value = e.OldValue;
            }
        }

        private void email_LostFocus(object sender, RoutedEventArgs e)
        {
            string setting = "";
            string value = ((TextBox) sender).Text;
            bool save = false;

            switch (((TextBox) sender).Name)
            {
                case "tb_emailsmtphost": { setting = Constants.SETTINGS_EMAIL_SMTP_HOST; save = validate_email_value(setting, value); break; }
                case "tb_emailfrom": { setting = Constants.SETTINGS_EMAIL_FROM; save = validate_email_value(setting, value); break; }
                default: save = false; break;
            }

            this.smtpring.IsActive = false;

            if (save)
            {
                TrackerSettings.Save(setting, value);
            }
            else
            {
                ((TextBox)sender).Text = "";
            }
        }

        private void perm_Changed(object sender, RoutedEventArgs e)
        {
            string setting = "";
            bool value = (bool)((MahApps.Metro.Controls.ToggleSwitch)sender).IsChecked;
            bool save = false;

            switch (((MahApps.Metro.Controls.ToggleSwitch)sender).Name)
            {
                case "sw_approveown": { setting = Constants.SETTINGS_PERM_APPROVE_OWN; save = true;  break; }
                default: save = false; break;
            }

            if (save)
            {
                TrackerSettings.Save(setting, value.ToString());
            }
        }

        #endregion

        #region Private Helper Methods

        private bool validate_time_value(string setting, string value)
        {
            bool pass = true;

            if (value == string.Empty || value == null)
                pass = false;

            return pass;
        }

        private bool validate_email_value(string setting, string value)
        {
            bool pass = true;

            if (setting == Constants.SETTINGS_EMAIL_SMTP_HOST)
                this.smtpring.IsActive = true;

            if (value == string.Empty || value == null)
                pass = false;

            pass = (setting == Constants.SETTINGS_EMAIL_SMTP_HOST) ? SendEmail.IsValidSMTP(value) : SendEmail.IsValidEmail(value);

            return pass;
        }

        #endregion
    }
}
