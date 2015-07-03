using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using petratracker.Code;
using System.Data;
using System.Threading;

namespace petratracker.Models
{
    public class TrackerSchedule
    {
        #region Private Members
        
        private static TrackerDataContext _trackerDB = (App.Current as App).TrackerDBo;
        private static PTASDataContext _ptasDB = (App.Current as App).PTASDBo;
        private static string[] _validationStatus = { "Not Validated", "SSNIT Number Error", "Name Error", "SSNIT Number & Name Errors", "New Employee", "New Employee & Errors", "Passed" };
       
        #endregion

        #region Constructor

        public TrackerSchedule()
        {
        }

        #endregion

        #region Public Schedule Methods

        public static Schedule GetSchedule(int id)
        {
           return (from j in _trackerDB.Schedules where j.id == id select j).Single();
        }

        public static IEnumerable<Schedule> GetSchedules(string status="", bool is_checking=false)
        {
            return (from j in _trackerDB.Schedules  orderby j.created_at descending select j);
        }

        public static IEnumerable<Schedule> GetSchedulesForProcessing()
        {
            return (from j in _trackerDB.Schedules 
                    where j.processing==false && j.workflow_status != "Complete"
                    orderby j.updated_at ascending 
                    select j);
        }
        
        public static void Save(Schedule s)
        {
            //s.modified_by = TrackerUser.GetCurrentUser().id;
            s.updated_at = DateTime.Now;
            _trackerDB.SubmitChanges();
        }

        public static void AddSchedule(string company, string companyid, string tier, string ct, string month, string year)
        {
            try
            {
                Schedule s = new Schedule();
                s.company = company;
                s.company_id = companyid;
                s.company_email = Utils.GetCompanyEmail(companyid) ;
                s.tier = tier;
                s.month = int.Parse(month);
                s.year = int.Parse(year);
                s.validated = false;
                s.validation_status = "Not Validated";               
                s.file_downloaded = false;
                s.file_uploaded = false;
                s.payment_id = 0;
                s.receipt_sent = false;
                s.workflow_status = "Processing of this schedule has not begun.s";
                s.modified_by = TrackerUser.GetCurrentUser().id;
                s.created_at = DateTime.Now;
                s.updated_at = DateTime.Now;
                _trackerDB.Schedules.InsertOnSubmit(s);
                _trackerDB.SubmitChanges();
                InitiateScheduleWorkFlow(s.id);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public static string GetSummary(int schedule_id)
        {
            var s = GetSchedule(schedule_id);
            return (s.workflow_status==string.Empty) ? "Processing of this schedule has not begun." : s.workflow_status;
        }

        #endregion

        #region Public Helper Methods

        public static IEnumerable<string> GetValidationStatus()
        {
            return _validationStatus.AsEnumerable();
        }

        public static IEnumerable<ComboBoxPairs> GetYears()
        {
            int year = DateTime.Now.Year;
            List<ComboBoxPairs> cbp = new List<ComboBoxPairs>();
            cbp.Add(new ComboBoxPairs((year - 1).ToString(), (year - 1).ToString()));
            cbp.Add(new ComboBoxPairs(year.ToString(), year.ToString()));
            cbp.Add(new ComboBoxPairs((year + 1).ToString(), (year + 1).ToString()));

            return cbp.AsEnumerable();
        }

        public static IEnumerable<ComboBoxPairs> GetCompanies()
        {
            try
            {
                var companies = Utils.GetCompanies();
                List<ComboBoxPairs> cbpc = new List<ComboBoxPairs>();

                foreach (var c in companies)
                {
                    cbpc.Add(new ComboBoxPairs(c.EntityID.ToString(), c.FullName));
                }

                return cbpc.AsEnumerable();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static IEnumerable<ComboBoxPairs> GetContributionTypes(string company = "")
        {
            List<ComboBoxPairs> cbp = new List<ComboBoxPairs>();
            IQueryable<ContributionType> cts = null;

            if (company == string.Empty)
            {
                cts = (from j in _ptasDB.ContributionTypes select j);
            }
            else
            {
                cts = (from j in _ptasDB.ContributionTypes
                       join f in _ptasDB.FundDeals on j.ContribTypeID equals f.ContribType_ID
                       where f.CompanyEntityId == company
                       select j);
            }

            foreach (var ct in cts)
            {
                cbp.Add(new ComboBoxPairs(ct.ContribTypeID.ToString(), ct.Description));
            }

            return cbp.AsEnumerable();
        }

        public static bool UpdateScheduleWorkFlowStatus()
        {
            var schedules = GetSchedulesForProcessing();

            foreach (var s in schedules)
            {
                EvaluateScheduleWorkFlow(s.id);
            }

            return true;
        }
        #endregion

        #region Private Helper Methods

        #region Workflow Methods

        private static async void InitiateScheduleWorkFlow(int schedule_id)
        {
            EvaluateScheduleWorkFlow(schedule_id);
            //var dueTime = TimeSpan.FromSeconds(60);
            //var interval = TimeSpan.FromSeconds(30); // TODO: Use settings
            //await Utils.DoPeriodicWorkAsync(new Func<bool>(UpdateScheduleWorkFlowStatus), dueTime, interval, CancellationToken.None);
        }

        private static void EvaluateScheduleWorkFlow(int schedule_id)
        {
            var s = GetSchedule(schedule_id);

            // Lock this schedule for now
            s.processing = true;
            Save(s);

            if (s.validation_status == "Passed")
            {
                EvaluatePassedSchedule(s);
            }
            else
            {
                //  Is the Schedule Validated?
                s.validated = CheckValidation(s.company_id, s.tier, s.contributiontype, s.month, s.year);
                
                if (s.validated)
                {
                    s.validation_status = CheckValidationStatus(s.company_id, s.tier, s.contributiontype, s.month, s.year);

                    //  Has Schedule has now passed?
                    if (s.validation_status == "Passed")
                    {
                        // Yes
                        EvaluatePassedSchedule(s);
                    }
                    else
                    {
                        // No
                        EvaluateReminderStatus(s);
                    }
                }
                else
                {
                    // No: send reminders
                    EvaluateValidationNotificationStatus(s);
                }
            }
        }

        private static void EvaluateValidationNotificationStatus(Schedule s)
        {
            // 1.b Schedule is not Validated. Has Notification been sent?
            Notification nf = TrackerNotification.GetNotificationByJob("Schedule Validation Request", "Schedule", s.id);
            if (nf != null)
            {
                // 1.a Notification exists - resend if it's after 24hrs and set status back to new
                DateTime now = DateTime.Now;
                if (now.Subtract(nf.last_sent).Hours > 24)
                {
                    nf.times_sent += 1;
                    nf.last_sent = DateTime.Now;
                    nf.status = "New";
                    TrackerNotification.Save(nf);
                    s.workflow_status = string.Format("Schedule is not validated. {0} notification requests sent to owner",nf.times_sent);
                }
            }
            else
            {
                // 1.b No notification has been sent, so send one.
                TrackerNotification.Add(TrackerUser.GetCurrentUser().role_id, "Schedule Validation Request", "Schedule", s.id);
                s.workflow_status = "Schedule is not validated. A notification requests sent to owner";
            }

            s.processing = false;
            Save(s);
        }

        private static void EvaluateReminderStatus(Schedule s)
        {
            IEnumerable<Email> ems = TrackerEmail.GetEmailsByJob("Schedule Error Fix Request", "Schedule", s.id);
            
            if (ems.Count() > 0)
            {
                Email em = ems.ElementAt(0);

                // 2.b.3 Let's go through the escalation process // TODO: replace super admin role
                DateTime now = DateTime.Now;
                if (now.Subtract(em.created_at).Hours > 120)
                {
                    if (s.emails_sent == 3) // It's the 5th day, send email and esacalate
                    {
                        TrackerEmail.Add(s.company_email, s.company_id, s.validation_status, "Schedule Error Fix Request", "Schedule", s.id);
                        TrackerNotification.Add(3, "Escalation: Schedule Error Fix Request", "Schedule", s.id);
                        s.emails_sent += 1;
                        s.email_last_sent = DateTime.Now;
                        s.workflow_status = "Emailed 4 reminders to fix errors: " + s.validation_status +". Escalated to RM.";  
                    }                
                }  
                else if (now.Subtract(em.created_at).Hours >= 72)
                {
                    if (s.emails_sent == 2) // It's the 3rd day, send email
                    {
                        TrackerEmail.Add(s.company_email, s.company_id, s.validation_status, "Schedule Error Fix Request", "Schedule", s.id);
                        s.emails_sent += 1;
                        s.email_last_sent = DateTime.Now;
                        s.workflow_status = "Emailed 3 reminders to fix errors: " + s.validation_status;  
                    }
                }
                else if (now.Subtract(em.created_at).Hours >= 48)
                {
                    if (s.emails_sent == 1) // It's the 2nd day, send email
                    {
                        TrackerEmail.Add(s.company_email, s.company_id, s.validation_status, "Schedule Error Fix Request", "Schedule", s.id);
                        s.emails_sent += 1;
                        s.email_last_sent = DateTime.Now;
                        s.workflow_status = "Emailed 2 reminders to fix errors: " + s.validation_status;  
                    }
                }

            }
            else
            {
                // 2.c Email has not been sent, send one
                TrackerEmail.Add(s.company_email, s.company_id,s.validation_status, "Schedule Error Fix Request", "Schedule", s.id);
                s.workflow_status = "Emailed request to fix errors: "+s.validation_status;  
            }

            s.processing = false;
            Save(s);
        }

        private static void EvaluatePassedSchedule(Schedule s)
        {
            // Expire any pending validation requestions
            TrackerNotification.ExpireByJob("Schedule Validation Request", "Schedule", s.id);
            TrackerNotification.ExpireByJob("Escalation: Schedule Error Fix Request", "Schedule", s.id);

            if (s.payment_id == 0)
            {
                // Check payments
                int i = CheckPaymentStatus(s.company_id, s.tier, s.contributiontype, s.month, s.year);
                if (i == 0)
                {
                    // No payments found. Will check later
                    s.workflow_status = "Schedule has been validated and passed. Waiting for payments.";
                    s.processing = false;
                    Save(s);
                }
                else
                {
                    s.workflow_status = "Schedule linked to Payments. Waiting for Receipt to be sent.";
                    EvaluatePaymentReceivedSchedule(s);
                }
            }
            else
            {
                EvaluatePaymentReceivedSchedule(s);
            }    
        }
        
        private static void EvaluatePaymentReceivedSchedule(Schedule s)
        {
            // Handle receipts
            if (!s.receipt_sent)
            {
                s.workflow_status = "Schedule linked to Payments. Waiting for Receipt to be sent.";
                s.processing = false;
                Save(s);
            }
            else
            {
                s.workflow_status = string.Format("Receipt sent {0}", s.receipt_sent_date.ToString());

                if (!s.file_downloaded)
                {
                    EvaluateFileDownloadNotificationStatus(s);
                }
                else
                {
                    s.workflow_status = string.Format("Receipt sent {0} and File downloaded {1}", s.receipt_sent_date.ToString(),s.file_downloaded_date.ToString());

                    if (!s.file_uploaded)
                    {
                        EvaluateFileUploadNotificationStatus(s);
                    }
                }
            }
        }

        private static void EvaluateFileDownloadNotificationStatus(Schedule s)
        {
            Notification nf = TrackerNotification.GetNotificationByJob("Schedule File Download Request", "Schedule", s.id);
            if (nf != null)
            {
                DateTime now = DateTime.Now;
                if (now.Subtract(nf.last_sent).Hours > 24)
                {
                    nf.times_sent += 1;
                    nf.last_sent = DateTime.Now;
                    nf.status = "New";
                    TrackerNotification.Save(nf);
                    s.workflow_status = string.Format("Schedule Receipt sent {0). No download though. {1} notification requests sent to owner", s.receipt_sent_date.ToString(), nf.times_sent);
                }
            }
            else
            {
                TrackerNotification.Add(TrackerUser.GetCurrentUser().role_id, "Schedule File Download Request", "Schedule", s.id);
                s.workflow_status = string.Format("Schedule Receipt sent {0). No download though. A notification requests sent to owner", s.receipt_sent_date.ToString());
            }

            s.processing = false;
            Save(s);
        }

        private static void EvaluateFileUploadNotificationStatus(Schedule s)
        {
            Notification nf = TrackerNotification.GetNotificationByJob("Schedule File Upload Request", "Schedule", s.id);
            if (nf != null)
            {
                DateTime now = DateTime.Now;
                if (now.Subtract(nf.last_sent).Hours > 24)
                {
                    nf.times_sent += 1;
                    nf.last_sent = DateTime.Now;
                    nf.status = "New";
                    TrackerNotification.Save(nf);
                    s.workflow_status = string.Format("Schedule File downloaded {0). No upload though. {1} notification requests sent to owner", s.file_downloaded_date.ToString(), nf.times_sent);
                }
            }
            else
            {
                DateTime now = DateTime.Now;
                if (now.Subtract(s.file_downloaded_date).Hours >= 36)
                {
                    TrackerNotification.Add(TrackerUser.GetCurrentUser().role_id, "Schedule File Upload Request", "Schedule", s.id);
                    s.workflow_status = string.Format("Schedule File downloaded {0}. No upload though. A notification requests sent to owner", s.file_downloaded_date.ToString());
                }
            }

            s.processing = false;
            Save(s);
        }

        private static int CheckPaymentStatus(string companyid, string tier, string ct, int month, int year)
        {
            DateTime dealDate = new DateTime(year, month, 1);

            try
            {
                var fd = (from j in _ptasDB.FundDeals
                          where j.ContribType_ID == int.Parse(ct.Trim()) &&
                           j.CompanyEntityId == companyid.Trim() &&
                           j.Tier == tier.Replace(" ", "") &&
                           j.TotalContribution != 0 &&
                           j.DealDate == dealDate
                          select j).Single();

                Payment pm = TrackerPayment.GetSubscription(companyid, fd.TotalContribution, dealDate);

                return (pm != null) ? pm.id : 0;

            } catch(Exception e) {
                throw(e);
                return 0;
            }
        }

        private static bool CheckValidation(string companyid, string tier, string ct, int month, int year)
        {
            DateTime dealDate = new DateTime(year, month, 1);           
            try
            {
                int fd = (from j in _ptasDB.FundDeals
                                where j.ContribType_ID == int.Parse(ct.Trim()) &&
                                      j.CompanyEntityId == companyid.Trim() &&
                                      j.Tier == tier.Replace(" ","") &&
                                      j.TotalContribution != 0 &&
                                      j.DealDate == dealDate
                                select j).Count();

                return (fd > 0);
            } 
            catch(Exception) 
            {
                return false;
            }
        }

        private static string CheckValidationStatus(string companyid, string tier, string ct, int month, int year)
        {
            DateTime dealDate = new DateTime(year, month, 1);
           
            try
            {
                var fd = (from j in _ptasDB.FundDeals
                               where j.ContribType_ID == int.Parse(ct.Trim()) &&
                                j.CompanyEntityId == companyid.Trim() &&
                                j.Tier == tier.Replace(" ", "") &&
                                j.TotalContribution != 0 &&
                                j.DealDate == dealDate
                                select j).Single();

                var fld = (from k in _ptasDB.FundDealLines where k.FundDealID == fd.FundDealID select k);

                string status = "Not Validated";
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

                if (passed) { status = "Passed";  }
                else if (newemp && (ssnit || name)) { status = "New Employee & Errors";  }
                else if (ssnit && name) { status = "SSNIT Number & Name Errors"; }
                else if (newemp) { status = "New Employee"; }
                else if (ssnit) { status = "SSNIT Number Error"; }
                else if (name) { status = "Name Error"; }
                else { status = "Not Validated"; }

                return status;            
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        #endregion

        #endregion
    }
}
