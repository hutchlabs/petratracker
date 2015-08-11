using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
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
using MahApps.Metro.Controls;
using petratracker.Models;





using petratracker.Utility;

namespace petratracker.Models
{
    class TrackerReports
    {


        #region private variables
        MicroGenExportDataTable rep = new MicroGenExportDataTable();  
        #endregion

        #region Tracker Reports Methods

        public static IEnumerable<Tracker_Report_Payment_Schedule_Status> get_payment_schedule_status_rep(string status)
        {
            try
            {

                

                switch (status)
                {
                    case "Overpayment":
                     return (from p in Database.Tracker.PPayments
                             join s in Database.Tracker.Schedules on p.id equals s.payment_id
                                    where (p.subscription_amount > s.amount) && (p.status == Constants.PAYMENT_STATUS_IDENTIFIED_APPROVED)
                             select new Tracker_Report_Payment_Schedule_Status() { Company = s.company, Tier = p.tier, Contribution_Month = p.deal_description_period, Status = p.status });

                    case "Underpayment":
                     return (from p in Database.Tracker.PPayments
                             join s in Database.Tracker.Schedules on p.id equals s.payment_id
                             where (p.subscription_amount < s.amount) && (p.status == Constants.PAYMENT_STATUS_IDENTIFIED_APPROVED)
                             select new Tracker_Report_Payment_Schedule_Status() { Company = s.company, Tier = p.tier, Contribution_Month = p.deal_description_period, Status = p.status });

                    case "No schedule":
                     return (from p in Database.Tracker.PPayments
                             where !Database.Tracker.Schedules.Any(s => s.payment_id == p.id) && (p.status == Constants.PAYMENT_STATUS_IDENTIFIED_APPROVED)
                             select new Tracker_Report_Payment_Schedule_Status() { Company = p.company_code, Tier = p.tier, Contribution_Month = p.deal_description_period, Status = p.status });

                    case "Schedule available":
                     return (from p in Database.Tracker.PPayments
                             where Database.Tracker.Schedules.Any(s => s.payment_id == p.id) && (p.status == Constants.PAYMENT_STATUS_IDENTIFIED_APPROVED)
                             select new Tracker_Report_Payment_Schedule_Status() { Company = p.company_code, Tier = p.tier, Contribution_Month = p.deal_description_period, Status = p.status });

                    default:
                        return null;
                        

                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<Tracker_Report_Slack_Days> get_Slack_Days(string status)
        { 
            try
            {

                

                switch (status)
                {
                    case "Receipts sent":
                        return (from p in Database.Tracker.PPayments
                                join s in Database.Tracker.Schedules on p.id equals s.payment_id
                                select new Tracker_Report_Slack_Days()
                                {
                                    Company = s.company,
                                    Tier = s.tier,
                                    Contribution_Month = p.deal_description_period,
                                    Status = s.receipt_sent== true ? "SENT" : "NOT SENT",
                                    Slack_Days = s.receipt_sent == true ? (TimeSpan)(s.receipt_sent_date - (s.created_at > p.value_date ? s.created_at : p.value_date)) : (TimeSpan)(DateTime.Now - (s.created_at > p.value_date ? s.created_at : p.value_date)),
                                    Comments = s.receipt_sent == true ? "Slack Time" : "Time Remaining"
                                });

                    case "Validation":
                        return (from p in Database.Tracker.PPayments
                                join s in Database.Tracker.Schedules on p.id equals s.payment_id
                                select new Tracker_Report_Slack_Days()                            
                                {   Company = s.company, 
                                    Tier = s.tier,
                                    Contribution_Month = p.deal_description_period,
                                    Status =  s.validated == true?"SENT":"NOT SENT",
                                    Slack_Days = s.validated == true?(TimeSpan)(s.validation_valuetime - s.created_at):(TimeSpan)(DateTime.Now - s.created_at),
                                    Comments = s.validated == true?"Slack Time":"Time Remaining"
                                });

                    case "Files downloaded":
                        return (from p in Database.Tracker.PPayments
                                join s in Database.Tracker.Schedules on p.id equals s.payment_id
                                select new Tracker_Report_Slack_Days()
                                {
                                    Company = s.company,
                                    Tier = s.tier,
                                    Contribution_Month = p.deal_description_period,
                                    Status = s.file_downloaded== true ? "SENT" : "NOT SENT",
                                    Slack_Days = s.file_downloaded == true ? (TimeSpan)(s.file_downloaded_date - (s.created_at > p.value_date ? s.created_at : p.value_date)) : (TimeSpan)(DateTime.Now - (s.created_at > p.value_date ? s.created_at : p.value_date)),
                                    Comments = s.validated == true ? "Slack Time" : "Time Remaining"
                                });


                    case "Files loaded":
                        return (from p in Database.Tracker.PPayments
                                join s in Database.Tracker.Schedules on p.id equals s.payment_id
                                select new Tracker_Report_Slack_Days()
                                {
                                    Company = s.company,
                                    Tier = s.tier,
                                    Contribution_Month = p.deal_description_period,
                                    Status = s.file_downloaded == true ? "SENT" : "NOT SENT",
                                    Slack_Days = s.file_uploaded == true ? (TimeSpan)(s.file_uploaded_date - (s.created_at > p.value_date ? s.created_at : p.value_date)) : (TimeSpan)(DateTime.Now - (s.created_at > p.value_date ? s.created_at : p.value_date)),
                                    Comments = s.validated == true ? "Slack Time" : "Time Remaining"
                                });

                    case "Reminders sent":
                        return null;


                    case "Ticket resolution":
                        return null;

                    case "Revalidation":
                        return null;

                    default:
                        return null;
                        

                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<Tracker_Report_Schedule_Status> get_Schedules_Status(string status)
        {
            try
            {
                switch (status)
                {
                    case "Payment received":
                         return (from s in Database.Tracker.Schedules
                                where Database.Tracker.PPayments.Any(p => p.id == s.payment_id)
                                select new Tracker_Report_Schedule_Status() { Company = s.company, Tier = s.tier, Contribution_Month = s.month.ToString(), Status = s.validation_status });
     
                    case "Payment not received":
                         return (from s in Database.Tracker.Schedules
                                where !Database.Tracker.PPayments.Any(p => p.id == s.payment_id)
                                select new Tracker_Report_Schedule_Status() { Company = s.company, Tier = s.tier, Contribution_Month = s.month.ToString(), Status = s.validation_status });

                    case "Ssnit no errors":
                         return (from s in Database.Tracker.Schedules
                                 where s.validation_status == Constants.WF_VALIDATION_ERROR_SSNIT
                                 select new Tracker_Report_Schedule_Status() { Company = s.company, Tier = s.tier, Contribution_Month = s.month.ToString(), Status = s.validation_status });


                    case "Name errors":
                         return (from s in Database.Tracker.Schedules
                                 where s.validation_status == Constants.WF_VALIDATION_ERROR_SSNIT_NAME
                                 select new Tracker_Report_Schedule_Status() { Company = s.company, Tier = s.tier, Contribution_Month = s.month.ToString(), Status = s.validation_status });

                    case "New employees":
                         return (from s in Database.Tracker.Schedules
                                 where s.validation_status == Constants.WF_VALIDATION_NEW_EMPLOYEE
                                 select new Tracker_Report_Schedule_Status() { Company = s.company, Tier = s.tier, Contribution_Month = s.month.ToString(), Status = s.validation_status });

                    case "Passed":
                         return (from s in Database.Tracker.Schedules
                                 where s.validation_status == Constants.WF_VALIDATION_PASSED
                                 select new Tracker_Report_Schedule_Status() { Company = s.company, Tier = s.tier, Contribution_Month = s.month.ToString(), Status = s.validation_status });

                    default:
                        return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<Tracker_Report_Schedules_Pending> get_Schedules_Pending(string status)
        {
            try
            {
                switch (status)
                {
                    case "Schedules to be validated":
                        return (from s in Database.Tracker.Schedules
                                where s.validated == false
                                select new Tracker_Report_Schedules_Pending() { Company = s.company, Tier = s.tier, Contribution_Month = s.month.ToString(), Status = s.validation_status });


                    case "Files to be downloaded":
                        return (from s in Database.Tracker.Schedules
                                where s.file_downloaded == false
                                select new Tracker_Report_Schedules_Pending() { Company = s.company, Tier = s.tier, Contribution_Month = s.month.ToString(), Status = s.validation_status });


                    case "Receipts to be sent":
                        return (from s in Database.Tracker.Schedules
                                where s.receipt_sent == false
                                select new Tracker_Report_Schedules_Pending() { Company = s.company, Tier = s.tier, Contribution_Month = s.month.ToString(), Status = s.validation_status });


                    case "Payments not linked to schedules":
                       
                        
                         //var from_tracker_schedules = (from p in Database.Tracker.PPayments 
                         //                              join s in Database.Tracker.Schedules on p.id equals s.payment_id into allPS
                         //                              select allPS).ToList();

                         //var from_tracker_PTAS = (from fd in Database.PTAS.PaymentScheduleLinks
                         //                         from allPS in from_tracker_schedules
                         //                         where allPS
                         
                         //                               select new { et.EntityKey, fd.OrderReference, fd.NAVPrice, fd.Group1Units, fd.DealID }
                         //                               ).ToList();        
                                     

                         //       select new Tracker_Report_Schedules_Pending() { Company = s.company, Tier = s.tier, Contribution_Month = s.month.ToString(), Status = s.validation_status });



                    default:
                        return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<TrackerReportTransRefType> get_Trans_Ref_Match()
        {
            try
            {
                
                         var from_tracker_payments = (from p in Database.Tracker.PPayments select p).ToList();
                            var from_microgen_tables = (from fd in Database.Microgen.fndDeals
                                                        join ass in Database.Microgen.Associations on fd.EntityFundID equals ass.TargetEntityID
                                                        join et in Database.Microgen.Entities on ass.SourceEntityID equals et.EntityID
                                                        where ass.RoleTypeID == 1003
                                                        select new { et.EntityKey, fd.OrderReference, fd.NAVPrice, fd.Group1Units, fd.DealID }
                                                        ).ToList();

                            var trans_ref = (from tp in from_tracker_payments
                                             join mt in from_microgen_tables on tp.transaction_ref_no equals mt.OrderReference into newTbl
                                             from newRes in newTbl.DefaultIfEmpty()
                                             //where tp.company_code != newRes.EntityKey
                                             select new TrackerReportTransRefType() { transaction_ref_no = tp.transaction_ref_no, transaction_amount = tp.transaction_amount, tier = tp.tier, company_code = tp.company_code, EntityKey = newRes.EntityKey, OrderReference = newRes.OrderReference, NAVPrice = newRes.NAVPrice, Group1Units = newRes.Group1Units, DealID = newRes.DealID }
                                            );

                            return trans_ref;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public static IEnumerable<TrackerReportTransRefType> get_RM_Company_Update()
        {
            try
            {
                Pages.selectCompany openSelectCompany = new Pages.selectCompany();
                bool? result = openSelectCompany.ShowDialog();
                if (result ?? false) { }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }


        #endregion

        #region private helpers

        private void get_trans_ref_file()
        {
            var from_tracker_payments = (from p in Database.Tracker.PPayments select p).ToList();
            var from_microgen_tables = (from fd in Database.Microgen.fndDeals
                                        join ass in Database.Microgen.Associations on fd.EntityFundID equals ass.TargetEntityID
                                        join et in Database.Microgen.Entities on ass.SourceEntityID equals et.EntityID
                                        where ass.RoleTypeID == 1003
                                        select new { et.EntityKey, fd.OrderReference, fd.NAVPrice, fd.Group1Units, fd.DealID }
                                        ).ToList();

            var trans_ref = (from tp in from_tracker_payments
                             join mt in from_microgen_tables on tp.transaction_ref_no equals mt.OrderReference
                             where tp.company_code != mt.EntityKey
                             select new TrackerReportTransRefType() { transaction_ref_no = tp.transaction_ref_no, transaction_amount = tp.transaction_amount, tier = tp.tier, company_code = tp.company_code, EntityKey =  mt.EntityKey, OrderReference = mt.OrderReference, NAVPrice = mt.NAVPrice, Group1Units = mt.Group1Units, DealID = mt.DealID }
                            );




            foreach (TrackerReportTransRefType p in trans_ref)
            {
                
                string[] codes = TrackerPayment.get_microgen_data(p.company_code, p.tier);
                DataRow newData = rep.NewRow();
                newData["FundCode"] = codes[0];
                newData["FundHolderCode"] = codes[1];
                newData["TransReference"] = p.transaction_ref_no;
                newData["TransUnitsGrp1"] = p.transaction_amount.ToString();
                newData["DealCcyPayAmnt"] = p.transaction_amount.ToString();
                newData["DealCcyDealAmnt"] = p.transaction_amount.ToString();
                newData["PayCcyPayAmnt"] = p.transaction_amount.ToString();
                newData["PayCcyDealAmnt"] = p.transaction_amount.ToString();
                newData["DealDate"] = (string)p.transaction_date.ToString("yyyy-MM-dd");
                newData["ValueDate"] = (string)p.value_date.ToString("yyyy-MM-dd");
                newData["BookDate"] = (string)p.value_date.ToString("yyyy-MM-dd");
                newData["PriceDate"] = (string)p.value_date.ToString("yyyy-MM-dd");
                rep.Rows.Add(newData);

            }
        }


        #endregion


    }
}
