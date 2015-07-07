using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using petratracker.Utility;
using System.Data;
using System.Threading;
using System.Globalization;

namespace petratracker.Models
{
    public class TrackerSchedule
    {
        #region Private Members
        
        #endregion

        #region Constructor

        public TrackerSchedule()
        {
        }

        #endregion

        #region Public Helper Methods

        public static Schedule GetSchedule(int id)
        {
           return (from j in TrackerDB.Tracker.Schedules where j.id == id select j).Single();
        }

        public static IEnumerable<Schedule> GetSchedules()
        {
            return (from j in TrackerDB.Tracker.Schedules orderby j.created_at descending select j);
        }

        public static IEnumerable<ComboBoxPairs> GetCBSchedules(string company_id="", string tier="")
        {
            try
            {
                List<ComboBoxPairs> cbpc = new List<ComboBoxPairs>();
                IEnumerable<Schedule> sc;

                if (company_id == string.Empty)
                {
                    sc = (from j in TrackerDB.Tracker.Schedules orderby j.created_at descending select j);
                }
                else
                {
                    sc = (from j in TrackerDB.Tracker.Schedules where (j.company_id == company_id) orderby j.created_at descending select j);
                }
                
                foreach (var s in sc)
                {
                    string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(s.month);
                    string desc = string.Format("{0} {1} - {2} - for {3}-{4}", s.company, s.tier, s.contributiontype, s.year, month);
                    
                    if (tier != string.Empty)
                    {
                        if (s.tier == tier)
                        {
                            cbpc.Add(new ComboBoxPairs(s.id.ToString(), desc));
                        }
                    }
                    else
                    {
                        cbpc.Add(new ComboBoxPairs(s.id.ToString(), desc));
                    }                
                }

                return cbpc.AsEnumerable();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static IEnumerable<Schedule> GetScheduleByStatus(string status)
        {
            return (from j in TrackerDB.Tracker.Schedules where j.workflow_status==status orderby j.created_at descending select j);
        }

        public static IEnumerable<Schedule> GetSchedulesForProcessing()
        {
            return (from j in TrackerDB.Tracker.Schedules
                    where j.processing == false && 
                          j.workflow_status != Constants.WF_STATUS_COMPLETED && 
                          j.workflow_status != Constants.WF_STATUS_ERROR_ESCALATED  &&
                          j.workflow_status != Constants.WF_STATUS_EXPIRED
                    orderby j.updated_at ascending 
                    select j);
        }
        
        public static void Save(Schedule s)
        {
            //s.modified_by = TrackerUser.GetCurrentUser().id;
            s.updated_at = DateTime.Now;
            TrackerDB.Tracker.SubmitChanges();
        }

        public static void AddSchedule(string company, string companyid, string tier, string ct, string month, string year, int parent_id)
        {
            try
            {
                Schedule s = new Schedule();
                s.company = company;
                s.company_id = companyid;
                s.company_email = TrackerDB.GetCompanyEmail(companyid) ;
                s.tier = tier;
                s.contributiontype = ct;
                s.month = int.Parse(month);
                s.year = int.Parse(year);
                s.validated = false;
                s.validation_status = Constants.WF_VALIDATION_NOTDONE;               
                s.file_downloaded = false;
                s.file_uploaded = false;
                s.payment_id = 0;
                s.receipt_sent = false;
                s.workflow_status = Constants.WF_VALIDATION_NOTDONE;
                s.workflow_summary = "Processing of this schedule has not begun";
                s.parent_id = parent_id;
                s.modified_by = TrackerUser.GetCurrentUser().id;
                s.created_at = DateTime.Now;
                s.updated_at = DateTime.Now;
                TrackerDB.Tracker.Schedules.InsertOnSubmit(s);
                TrackerDB.Tracker.SubmitChanges();
                InitiateScheduleWorkFlow(s);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetSummary(int schedule_id)
        {
            var s = GetSchedule(schedule_id);
            return (s.workflow_summary==string.Empty) ? "Processing of this schedule has not begun." : s.workflow_summary;
        }

        public static IEnumerable<ComboBoxPairs> GetYears()
        {
            List<ComboBoxPairs> cbp = new List<ComboBoxPairs>();

            int year = DateTime.Now.Year;
            for(int y=2012; y <= year+1; y++)
            {
                cbp.Add(new ComboBoxPairs(y.ToString(), y.ToString()));
            }

            return cbp.AsEnumerable();
        }

        public static IEnumerable<ComboBoxPairs> GetCompanies()
        {
            try
            {
                var companies = TrackerDB.GetCompanies();
                List<ComboBoxPairs> cbpc = new List<ComboBoxPairs>();

                foreach (var c in companies)
                {
                    cbpc.Add(new ComboBoxPairs(c.EntityID.ToString(), c.FullName));
                }

                return cbpc.AsEnumerable();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static IEnumerable<ComboBoxPairs> GetContributionTypes(string company = "")
        {
            List<ComboBoxPairs> cbp = new List<ComboBoxPairs>();
            IQueryable<ContributionType> cts = null;

            if (company == string.Empty)
            {
                cts = (from j in TrackerDB.PTAS.ContributionTypes select j);
            }
            else
            {
                cts = (from j in TrackerDB.PTAS.ContributionTypes
                       join f in TrackerDB.PTAS.FundDeals on j.ContribTypeID equals f.ContribType_ID
                       where f.CompanyEntityId == company
                       select j);
            }

            foreach (var ct in cts)
            {
                cbp.Add(new ComboBoxPairs(ct.ContribTypeID.ToString(), ct.Description));
            }

            return cbp.AsEnumerable();
        }

        #endregion

        #region Workflow Methods

        public static async void InitiateScheduleWorkFlow(Schedule schedule = null)
        {
            if (schedule != null)
            {
                EvaluateScheduleWorkFlow(schedule);
            }
            var dueTime = TimeSpan.FromSeconds(60);
            var interval = TimeSpan.FromSeconds(int.Parse(TrackerSettings.GetSetting(Constants.SETTINGS_TIME_SCHEDULE_UPDATE_INTERVAL)));
            await Utils.DoPeriodicWorkAsync(new Func<bool>(UpdateScheduleWorkFlowStatus), dueTime, interval, CancellationToken.None);
        }

        public static bool UpdateScheduleWorkFlowStatus()
        {
            var schedules = GetSchedulesForProcessing();

            foreach (var s in schedules)
            {
                EvaluateScheduleWorkFlow(s);
            }

            return true;
        }

        public static Schedule ResolveScheduleIssue(Schedule s)
        {
            DateTime newVT = CheckValidationTime(s.company_id, s.tier, s.contributiontype, s.month, s.year);

            if (s.validation_valuetime == newVT) // Do data start revalidation;
            {
                EvaluateScheduleWorkFlow(s);
            }
            else
            {
                // Add new schedule with similar details and retire the old one
                AddSchedule(s.company, s.company_id, s.tier, s.contributiontype, s.month.ToString(), s.year.ToString(), s.id);
                s.workflow_status = Constants.WF_STATUS_EXPIRED;
                s.workflow_summary = "Schedule issues resolved, but there was a new valuetime so a new schedule has been created.";
                Save(s);
            }

            return s;
        }

        public static Schedule MarkReceiptSent(Schedule schedule)
        {
            schedule.receipt_sent = true;
            schedule.receipt_sent_date = DateTime.Now;
            schedule = EvaluatePaymentReceivedSchedule(schedule);

            TrackerNotification.ResolveByJob(Constants.NF_TYPE_SCHEDULE_REQUEST_RECEIPT_SEND, Constants.JOB_TYPE_SCHEDULE, schedule.id);

            return schedule;
        }

        public static Schedule MarkFileDownloaded(Schedule schedule)
        {
            schedule.file_downloaded = true;
            schedule.file_downloaded_date = DateTime.Now;
            schedule = EvaluatePaymentReceivedSchedule(schedule);

            TrackerNotification.ResolveByJob(Constants.NF_TYPE_SCHEDULE_REQUEST_FILE_DOWNLOAD, Constants.JOB_TYPE_SCHEDULE, schedule.id);

            return schedule;
        }
      

        private static void EvaluateScheduleWorkFlow(Schedule s)
        {
            // Lock this schedule for now
            s.processing = true;
            Save(s);

            if (s.validation_status == Constants.WF_VALIDATION_PASSED)
            {
                EvaluatePassedSchedule(s);
            }
            else
            {
                //  Is the Schedule Validated?
                s.validated = CheckValidation(s.company_id, s.tier, s.contributiontype, s.month, s.year);
                
                if (s.validated)
                {
                    s.validation_valuetime = CheckValidationTime(s.company_id, s.tier, s.contributiontype, s.month, s.year);

                    s.validation_status = CheckValidationStatus(s.company_id, s.tier, s.contributiontype, s.month, s.year);

                    //  Has Schedule has now passed?
                    if (s.validation_status == Constants.WF_VALIDATION_PASSED)
                    {
                        // Yes
                        s.workflow_status = Constants.WF_STATUS_PAYMENTS_PENDING;
                        s.workflow_summary = "Schedule has been validated with no errors. Waiting for payments.";
                        
                        // Expire any pending validation requestions
                        TrackerNotification.ExpireByJob(Constants.NF_TYPE_SCHEDULE_REQUEST_VALIDATION, Constants.JOB_TYPE_SCHEDULE, s.id);
                        TrackerNotification.ExpireByJob(Constants.NF_TYPE_SCHEDULE_REQUEST_ERRORFIX, Constants.JOB_TYPE_SCHEDULE, s.id);
                        TrackerNotification.ExpireByJob(Constants.NF_TYPE_SCHEDULE_REQUEST_ERRORFIX_ESCALATION, Constants.JOB_TYPE_SCHEDULE, s.id);
                        
                        EvaluatePassedSchedule(s);
                    }
                    else
                    {
                        // No
                        s.workflow_status = Constants.WF_STATUS_ERROR_PREFIX + s.validation_status;
                        EvaluateReminderStatus(s);  
                    }
                }
                else
                {
                    // No: send reminders
                    int times_sent = UpdateNotificationStatus(s.id, Constants.NF_TYPE_SCHEDULE_REQUEST_VALIDATION, Constants.SETTINGS_TIME_VALIDATION_REQUEST_INTERVAL);
                    s.workflow_summary = string.Format("Schedule is not validated. {0} notification requests sent.", times_sent);
                    s.workflow_status = Constants.WF_VALIDATION_NOTDONE_REMINDER;
                    s.processing = false;
                    Save(s);
                }
            }
        }

        private static void EvaluateReminderStatus(Schedule s)
        {
            Notification nf = TrackerNotification.GetNotificationByJob(Constants.NF_TYPE_SCHEDULE_REQUEST_ERRORFIX, 
                                                                       Constants.JOB_TYPE_SCHEDULE, 
                                                                       s.id);

            if (nf != null)
            {
                // 1.a Notification exists - 
                DateTime now = DateTime.Now;

                // 2.b.3 Let's go through the escalation process // TODO: replace super admin role
                if (now.Subtract(nf.created_at).Hours > int.Parse(TrackerSettings.GetSetting(Constants.SETTINGS_TIME_ERRORFIX_3_REMINDER_WINDOW)))
                {
                    if (nf.times_sent == 3) // It's the 5th day, send email and esacalate
                    {
                        // Expire old notification
                        nf.status = Constants.NF_STATUS_EXPIRED;
                        TrackerNotification.Save(nf);

                        // Send email and new reminder to super ops
                        TrackerEmail.Add(s.User.username, s.company, s.validation_status, Constants.NF_TYPE_SCHEDULE_REQUEST_ERRORFIX, Constants.JOB_TYPE_SCHEDULE, s.id);
                        TrackerNotification.Add(Constants.ROLES_SUPER_OPS_USER, 
                                                Constants.NF_TYPE_SCHEDULE_REQUEST_ERRORFIX_ESCALATION, 
                                                Constants.JOB_TYPE_SCHEDULE, s.id);
                        
                        s.emails_sent = 1;
                        s.email_last_sent = DateTime.Now;
                        s.workflow_status = Constants.WF_STATUS_ERROR_ESCALATED;
                        s.workflow_summary = "Fourth reminder and email sent to Ops to alert company to fix errors: " + s.validation_status +". Escalated to RM.";
                    }                
                }
                else if (now.Subtract(nf.created_at).Hours > int.Parse(TrackerSettings.GetSetting(Constants.SETTINGS_TIME_ERRORFIX_2_REMINDER_WINDOW)))
                {
                    if (nf.times_sent == 2) // It's the 3rd day, send email
                    {
                        nf.times_sent += 1;
                        nf.last_sent = DateTime.Now;
                        nf.status = Constants.NF_STATUS_NEW;
                        TrackerNotification.Save(nf);
                        s.workflow_status = "Third reminder sent to Ops to alert company to fix errors: " + s.validation_status;
                    }
                }
                else if (now.Subtract(nf.created_at).Hours > int.Parse(TrackerSettings.GetSetting(Constants.SETTINGS_TIME_ERRORFIX_1_REMINDER_WINDOW)))
                {
                    if (nf.times_sent == 1) // It's the 2nd day, send email
                    {                       
                        nf.times_sent += 1;
                        nf.last_sent = DateTime.Now;
                        nf.status = Constants.NF_STATUS_NEW;
                        TrackerNotification.Save(nf);
                        s.workflow_status = "Second reminder sent to Ops to alert company to fix errors: " + s.validation_status;
                    }
                }
            }
            else
            {
                // 2.c Notification has not been sent, send one.
                TrackerNotification.Add(Constants.ROLES_OPS_USER, Constants.NF_TYPE_SCHEDULE_REQUEST_ERRORFIX, Constants.JOB_TYPE_SCHEDULE, s.id);
                s.workflow_summary = "1 reminder sent to Ops to alert company to fix errors: "+s.validation_status;  
            }
            
            s.processing = false;
            Save(s);
        }

        private static void EvaluatePassedSchedule(Schedule s)
        {
            if (s.payment_id == 0)
            {
                // Check payments
                int i = CheckPaymentStatus(s.company_id, s.tier, s.contributiontype, s.month, s.year);
                if (i == 0)
                {
                    // No payments found. Will check later
                    s.processing = false;
                    Save(s);
                }
                else
                {
                    s.workflow_status = Constants.WF_STATUS_PAYMENTS_RECEIVED;
                    s.workflow_summary = "Schedule linked to Payments. Waiting for Receipt to be sent and File download & uploaded";
                    EvaluatePaymentReceivedSchedule(s);
                }
            }
            else
            {
                EvaluatePaymentReceivedSchedule(s);
            }    
        }
    
        private static Schedule EvaluatePaymentReceivedSchedule(Schedule s)
        {
            s.processing = true;
            Save(s);

            if (s.receipt_sent && s.file_downloaded && s.file_uploaded)
            {
                s.workflow_status = Constants.WF_STATUS_COMPLETED;
                s.workflow_summary = string.Format("Receipt sent {0} and File downloaded {1} and File uploaded {2}", s.receipt_sent_date.ToString(), s.file_downloaded_date.ToString(), s.file_uploaded_date.ToString());
            }
            else if (s.receipt_sent && s.file_downloaded && !s.file_uploaded)
            {
                s.workflow_status = Constants.WF_STATUS_RF_SENT_DOWNLOAD_NOUPLOAD;
                s.workflow_summary = string.Format("Receipt sent {0} and File downloaded {1}. No File uploaded", s.receipt_sent_date.ToString(), s.file_downloaded_date.ToString());
                s = EvaluateFileUploadNotificationStatus(s);
            }
            else if (s.receipt_sent && !s.file_downloaded && !s.file_uploaded)
            {
                s.workflow_status = Constants.WF_STATUS_RF_SENT_NODOWNLOAD_NOUPLOAD;
                s.workflow_summary = string.Format("Receipt sent {0}. No File downloaded and no File uploaded", s.receipt_sent_date.ToString());
                UpdateNotificationStatus(s.id, Constants.NF_TYPE_SCHEDULE_REQUEST_FILE_DOWNLOAD, Constants.SETTINGS_TIME_FILE_DOWNLOAD_INTERVAL);
            }
            else if (!s.receipt_sent && s.file_downloaded && s.file_uploaded)
            {
                s.workflow_status = Constants.WF_STATUS_RF_NOSENT_DOWNLOAD_UPLOAD;
                s.workflow_summary = string.Format("No Receipt sent. File downloaded {0} and File uploaded {1}", s.file_downloaded_date.ToString(), s.file_uploaded_date.ToString());
                UpdateNotificationStatus(s.id, Constants.NF_TYPE_SCHEDULE_REQUEST_RECEIPT_SEND, Constants.SETTINGS_TIME_RECEIPT_SEND_INTERVAL);
            }
            else if (!s.receipt_sent && s.file_downloaded && !s.file_uploaded)
            {
                s.workflow_status = Constants.WF_STATUS_RF_NOSENT_DOWNLOAD_NOUPLOAD;
                s.workflow_summary = string.Format("No Receipt sent. File downloaded {0} and no File uploaded", s.file_downloaded_date.ToString());
                UpdateNotificationStatus(s.id, Constants.NF_TYPE_SCHEDULE_REQUEST_RECEIPT_SEND, Constants.SETTINGS_TIME_RECEIPT_SEND_INTERVAL);
                s = EvaluateFileUploadNotificationStatus(s);
            }
            else if (!s.receipt_sent && !s.file_downloaded && !s.file_uploaded)
            {
                s.workflow_status = Constants.WF_STATUS_PAYMENTS_RECEIVED;
                s.workflow_summary = "Schedule linked to Payments. Waiting for Receipt to be sent and File download & uploaded";
                UpdateNotificationStatus(s.id, Constants.NF_TYPE_SCHEDULE_REQUEST_RECEIPT_SEND, Constants.SETTINGS_TIME_RECEIPT_SEND_INTERVAL);
                UpdateNotificationStatus(s.id, Constants.NF_TYPE_SCHEDULE_REQUEST_FILE_DOWNLOAD, Constants.SETTINGS_TIME_FILE_DOWNLOAD_INTERVAL);
            }
            else
            {
                // Shouldn't reach here..but we should handle file_uploaded, but not downloaded yet issue. how?
            }

            s.processing = false;
            Save(s);

            return s;
        }

        private static Schedule EvaluateFileUploadNotificationStatus(Schedule s)
        {
            bool fileuploaded = false;
            try
            {
                fileuploaded = CheckFileUploaded(s.company, s.company_id, s.tier, s.month, s.year, s.Payment.transaction_amount);
            }
            catch(Exception)
            {
            }

            if (fileuploaded)
            {
                 s.file_uploaded = true;
                 s.file_uploaded_date = DateTime.Now;
                               
                if (s.receipt_sent)
                {
                    s.workflow_status = Constants.WF_STATUS_COMPLETED;
                    s.workflow_summary = string.Format("Receipt sent {0} and File downloaded {1} and File uploaded {2}", s.receipt_sent_date.ToString(), s.file_downloaded_date.ToString(), s.file_uploaded_date.ToString());
                }
                else
                {
                    s.workflow_status = Constants.WF_STATUS_RF_NOSENT_DOWNLOAD_UPLOAD;
                    s.workflow_summary = string.Format("No Receipt sent. File downloaded {0} and File uploaded {1}", s.file_downloaded_date.ToString(), s.file_uploaded_date.ToString());
                }

                // Resolve old notifications
                TrackerNotification.ResolveByJob(Constants.NF_TYPE_SCHEDULE_REQUEST_FILE_UPLOAD, Constants.JOB_TYPE_SCHEDULE, s.id);
            }
            else
            {
                if (FileUploadWindowHasExpired(s.file_downloaded_date))
                {
                    s.file_downloaded = false;
                    s.file_downloaded_date = DateTime.Parse("0000-00-00 00:00:00");

                    if (s.receipt_sent)
                    {
                        s.workflow_status = Constants.WF_STATUS_RF_SENT_NODOWNLOAD_NOUPLOAD;
                        s.workflow_summary = string.Format("Receipt sent {0}. No File downloaded and no File uploaded", s.receipt_sent_date.ToString());
                    }
                    else
                    {
                        s.workflow_status = Constants.WF_STATUS_PAYMENTS_RECEIVED;
                        s.workflow_summary = "Schedule linked to Payments. Waiting for Receipt to be sent and File download & uploaded";
                    }

                    // Expire old notifications and send a new one for the file download.
                    TrackerNotification.ExpireByJob(Constants.NF_TYPE_SCHEDULE_REQUEST_FILE_UPLOAD, Constants.JOB_TYPE_SCHEDULE, s.id);
                    UpdateNotificationStatus(s.id, Constants.NF_TYPE_SCHEDULE_REQUEST_FILE_DOWNLOAD, Constants.SETTINGS_TIME_FILE_DOWNLOAD_INTERVAL);
                }
                else
                {
                    UpdateNotificationStatus(s.id, Constants.NF_TYPE_SCHEDULE_REQUEST_FILE_UPLOAD, Constants.SETTINGS_TIME_FILE_UPLOAD_INTERVAL);
                }
            }
            return s;
        }

        #endregion

        #region Private Helper Methods

        private static bool FileUploadWindowHasExpired(DateTime file_downloaded_date)
        {
            DateTime now = DateTime.Now;
            return (now.Subtract(file_downloaded_date).Hours >= int.Parse(TrackerSettings.GetSetting(Constants.SETTINGS_TIME_FILE_UPLOAD_WINDOW)));
        }

        private static int UpdateNotificationStatus(int sch_id, string job, string retry_interval)
        {
            int numNotifications = 1;

            Notification nf = TrackerNotification.GetNotificationByJob(job, Constants.JOB_TYPE_SCHEDULE, sch_id);

            if (nf != null)
            {
                DateTime now = DateTime.Now;
                if (now.Subtract(nf.last_sent).Hours > int.Parse(TrackerSettings.GetSetting(retry_interval)))
                {
                    nf.times_sent += 1;
                    nf.last_sent = DateTime.Now;
                    nf.status = Constants.NF_STATUS_NEW; ;
                    TrackerNotification.Save(nf);
                }
                numNotifications = nf.times_sent;
            }
            else
            {
                TrackerNotification.Add(Constants.ROLES_OPS_USER, job, Constants.JOB_TYPE_SCHEDULE, sch_id);
            }

            return numNotifications;
        }

        private static int CheckPaymentStatus(string companyid, string tier, string ct, int month, int year)
        {
            DateTime dealDate = new DateTime(year, month, 1);

            try
            {
                var fd = (from j in TrackerDB.PTAS.FundDeals
                          where j.ContribType_ID == int.Parse(ct.Trim()) &&
                           j.CompanyEntityId == companyid.Trim() &&
                           j.Tier == tier.Replace(" ", "") &&
                           j.TotalContribution != 0 &&
                           ((DateTime)j.DealDate).Date == dealDate.Date
                          select j).Single();

                Payment pm = TrackerPayment.GetSubscription(companyid, tier, fd.TotalContribution, dealDate);

                return (pm != null) ? pm.id : 0;

            } catch(Exception e) {
                return 0;

                throw(e);
            }
        }

        private static bool CheckValidation(string companyid, string tier, string ct, int month, int year)
        {
            DateTime dealDate = new DateTime(year, month, 1);           
            try
            {
                int fd = (from j in TrackerDB.PTAS.FundDeals
                                where j.ContribType_ID == int.Parse(ct.Trim()) &&
                                      j.CompanyEntityId == companyid.Trim() &&
                                      j.Tier == tier.Replace(" ","") &&
                                      j.TotalContribution != 0 &&
                                ((DateTime)j.DealDate).Date == dealDate.Date
                          select j).Count();

                return (fd > 0);
            } 
            catch(Exception) 
            {
                return false;
            }
        }

        private static bool CheckFileUploaded(string company, string companyid, string tier, int month, int year, decimal? amount)
        {
            DateTime dealDate = new DateTime(year, month, 1);
            
            string fundName = company + tier;

            try
            {
                var fd = from a in TrackerDB.Microgen.Associations
                         join ae2 in TrackerDB.Microgen.cclv_AllEntities on a.TargetEntityID equals ae2.EntityID
                         join ae3 in TrackerDB.Microgen.cclv_AllEntities on a.SourceEntityID equals ae3.EntityID
                         join d in TrackerDB.Microgen.fndDeals on ae3.EntityID equals d.EntityFundID
                         where (a.RoleTypeID == 1003) &&
                               (ae3.FullName == fundName) &&
                               (ae2.EntityKey == companyid.Trim()) &&
                               (((DateTime)d.DealingDate).Date == dealDate.Date)
                         group d by new { d.DealingDate } into s
                         select new
                         {
                             DealDate = s.Key.DealingDate,
                             TotalAmount = s.Sum(y => y.PaymentAmountDealCcy),
                         };
                foreach(var f in fd)
                {
                    if (f.TotalAmount == amount)
                        return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static DateTime CheckValidationTime(string companyid, string tier, string ct, int month, int year)
        {
            DateTime dealDate = new DateTime(year, month, 1);
            try
            {

               var fd = (from j in TrackerDB.PTAS.FundDeals
                          where j.ContribType_ID == int.Parse(ct.Trim()) &&
                                j.CompanyEntityId == companyid.Trim() &&
                                j.Tier == tier.Replace(" ", "") &&
                                j.TotalContribution != 0 &&
                                ((DateTime)j.DealDate).Date == dealDate.Date
                         select j).Single();

               return (DateTime)fd.DealDate;
            }
            catch (Exception)
            {
                return dealDate;
            }
        }

        private static string CheckValidationStatus(string companyid, string tier, string ct, int month, int year)
        {
            DateTime dealDate = new DateTime(year, month, 1);
            string status = Constants.WF_VALIDATION_NOTDONE;

            try
            {
                var fd = (from j in TrackerDB.PTAS.FundDeals
                               where j.ContribType_ID == int.Parse(ct.Trim()) &&
                                j.CompanyEntityId == companyid.Trim() &&
                                j.Tier == tier.Replace(" ", "") &&
                                j.TotalContribution != 0 &&
                                ((DateTime)j.DealDate).Date == dealDate.Date
                                select j).Single();

                var fld = (from k in TrackerDB.PTAS.FundDealLines where k.FundDealID == fd.FundDealID select k);

                bool passed = true;
                bool newemp = false;
                bool ssnit = false;
                bool name = false;
                foreach(var line in fld)
                {
                    if (line.LineStatus.Contains("Employee not found"))
                    {
                        passed = false;
                        newemp = true;
                    }
                    if (line.LineStatus.Contains("Error:SSNIT number") || line.LineStatus.Contains("Error:Staff ID"))
                    {
                        passed = false;
                        ssnit = true;
                    }
                    if (line.LineStatus.Contains("Error:Full Name"))
                    {
                        passed = false;
                        name = true;
                    }
                }

                if (passed) { status = Constants.WF_VALIDATION_PASSED; }
                else if (newemp && (ssnit || name)) { status = Constants.WF_VALIDATION_ERROR_ALL;  }
                else if (ssnit && name) { status = Constants.WF_VALIDATION_ERROR_SSNIT_NAME; }
                else if (newemp) { status = Constants.WF_VALIDATION_ERROR_NEW_EMPLOYEE; }
                else if (ssnit) { status = Constants.WF_VALIDATION_ERROR_SSNIT; }
                else if (name) { status = Constants.WF_VALIDATION_ERROR_NAME; }
                else { status = Constants.WF_VALIDATION_NOTDONE; }

                return status;            
            }
            catch (Exception)
            {
                return status;
            }
        }
    
        #endregion
    }
}
