using System;
using System.Data;
using System.Data.OleDb;
using System.Threading;
using System.Threading.Tasks;

namespace petratracker.Utility
{
    public class ComboBoxPairs
    {
        public string _Key { get; set; }
        public string _Value { get; set; }

        public ComboBoxPairs(string _key, string _value)
        {
            _Key = _key;
            _Value = _value;
        }
    }

    
    public static class Constants
    {
        // Role ids
        public const int ROLES_OPS_USER = 4;
        public const int ROLES_SUPER_OPS_USER = 3;
 
        // Job types for Jobs table
        public const string JOB_TYPE_SCHEDULE = "Schedule";

        // Notification status and types
        public const string NF_STATUS_NEW = "New";
        public const string NF_STATUS_RESOLVED = "Resolved";
        public const string NF_STATUS_EXPIRED = "Expired";

        public const string NF_TYPE_SCHEDULE_REQUEST_VALIDATION = "Schedule Validation Request";
        public const string NF_TYPE_SCHEDULE_REQUEST_ERRORFIX = "Schedule Error Fix Request";
        public const string NF_TYPE_SCHEDULE_REQUEST_ERRORFIX_ESCALATION = "Escalation: Schedule Error Fix Request";
        public const string NF_TYPE_SCHEDULE_REQUEST_RECEIPT_SEND = "Schedule Send Receipt Request";
        public const string NF_TYPE_SCHEDULE_REQUEST_FILE_DOWNLOAD = "Schedule File Download Request";
        public const string NF_TYPE_SCHEDULE_REQUEST_FILE_UPLOAD = "Schedule File Upload Request";

        // Workflow Status
        public const string WF_VALIDATION_NOTDONE = "Not Validated";
        public const string WF_VALIDATION_NOTDONE_REMINDER = "Not Validated. Reminders sent";
        public const string WF_VALIDATION_ERROR_SSNIT = "SSNIT Number Error";
        public const string WF_VALIDATION_ERROR_NAME = "Name Error";  
        public const string WF_VALIDATION_ERROR_SSNIT_NAME = "SSNIT Number & Name Errors"; 
        public const string WF_VALIDATION_ERROR_NEW_EMPLOYEE = "New Employee"; 
        public const string WF_VALIDATION_ERROR_ALL = "New Employee & Errors";
        public const string WF_VALIDATION_ERROR_ESCALATED = "Issue Escalated";
        public const string WF_VALIDATION_PASSED = "Passed";
        public const string WF_STATUS_ERROR_PREFIX = "Validated with Errors: ";
        public const string WF_STATUS_ERROR_SSNIT = WF_STATUS_ERROR_PREFIX + WF_VALIDATION_ERROR_SSNIT;
        public const string WF_STATUS_ERROR_NAME = WF_STATUS_ERROR_PREFIX + WF_VALIDATION_ERROR_NAME;
        public const string WF_STATUS_ERROR_SSNIT_NAME = WF_STATUS_ERROR_PREFIX + WF_VALIDATION_ERROR_SSNIT_NAME;
        public const string WF_STATUS_ERROR_NEW_EMPLOYEE = WF_STATUS_ERROR_PREFIX + WF_VALIDATION_ERROR_NEW_EMPLOYEE;
        public const string WF_STATUS_ERROR_ALL = WF_STATUS_ERROR_PREFIX + WF_VALIDATION_ERROR_ALL;
        public const string WF_STATUS_ERROR_ESCALATED = WF_STATUS_ERROR_PREFIX + WF_VALIDATION_ERROR_ESCALATED;
        public const string WF_STATUS_PAYMENTS_PENDING = "Passed. Payment Pending";
        public const string WF_STATUS_PAYMENTS_RECEIVED = "Payment Received. No Receipt. No Download. No Upload";
        public const string WF_STATUS_RF_SENT_NODOWNLOAD_NOUPLOAD = "Receipt Sent. No Download. No Upload";
        public const string WF_STATUS_RF_SENT_DOWNLOAD_NOUPLOAD = "Receipt Sent. File Downloaded. No Upload";
        public const string WF_STATUS_RF_NOSENT_DOWNLOAD_UPLOAD = "No Receipt Sent. File Downloaded. File Uploaded";
        public const string WF_STATUS_RF_NOSENT_DOWNLOAD_NOUPLOAD = "No Receipt Sent. File Downloaded. No Upload";
        public const string WF_STATUS_COMPLETED = "Completed";
        public const string WF_STATUS_EXPIRED = "Resolved and Expired";


        // Settings
        public const string SETTINGS_TIME_VALIDATION_REQUEST_INTERVAL = "time_retry_validationrequest";
        public const string SETTINGS_TIME_ERRORFIX_1_REMINDER_WINDOW = "time_retry_errorfix1st";
        public const string SETTINGS_TIME_ERRORFIX_2_REMINDER_WINDOW = "time_retry_errorfix2nd";
        public const string SETTINGS_TIME_ERRORFIX_3_REMINDER_WINDOW = "time_retry_errorfix3rd";
        public const string SETTINGS_TIME_RECEIPT_SEND_INTERVAL = "time_retry_receiptsendrequest";
        public const string SETTINGS_TIME_FILE_DOWNLOAD_INTERVAL = "time_retry_filedownloadrequest";
        public const string SETTINGS_TIME_FILE_UPLOAD_INTERVAL = "time_retry_fileuploadrequest";
        public const string SETTINGS_TIME_FILE_UPLOAD_WINDOW = "time_window_fileupload";
        public const string SETTINGS_TIME_SCHEDULE_UPDATE_INTERVAL = "time_schedulestatus_update";
    }

    public class Utils
    {
        public static async Task DoPeriodicWorkAsync(Delegate todoTask, TimeSpan dueTime, TimeSpan interval, CancellationToken token)
        {
            // Initial wait time before we begin the periodic loop.
            if (dueTime > TimeSpan.Zero)
                await Task.Delay(dueTime, token);

            // Repeat this loop until cancelled.
            while (!token.IsCancellationRequested)
            {
                // Update Task
                todoTask.DynamicInvoke();

                // Wait to repeat again.
                if (interval > TimeSpan.Zero)
                    await Task.Delay(interval, token);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static DataTable GetDataTable(string excelQL, string connectionString)
        {
            DataTable dt = new DataTable();
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(excelQL, conn))
                    {
                        using (OleDbDataReader rdr = cmd.ExecuteReader())
                        {
                            dt.Load(rdr);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }
    }
}
