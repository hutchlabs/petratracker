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


        public static IEnumerable<TrackerReport_RMReport> get_RM_Report()
        {
            try
            {
                string rm_report_sql = @"SELECT isnull(s.company, p.company_code) Company_Name
	                                             , isnull(s.company_id, p.company_code) Company_ID
	                                             , isnull(p.company_code, s.company_id) Company_Code
	                                             , isnull(s.tier, p.tier) Tier
	                                             , isnull(p.deal_description_period, CONCAT(s.month,' ',s.year)) Contributionperiod
	                                             , isnull(s.contributiontype, p.deal_description) contributiontype
	                                             , isnull(p.value_date, '') Payment_Date
	                                             , isnull(p.transaction_amount, 0) Payment_Amount
	                                             , isnull(s.validation_status, 'No Schedule') Validation_Status
	                                             , Isnull((case when s.validation_status like '%passed%' 
	                                                            then '' else (case when s.resolution_reminder2_date Is null 
	                                                                               then (case when s.resolution_reminder1_date is null 
						                                                                      then '' 
	                                                                                          else s.resolution_reminder1_date end) 
					                                                               else s.resolution_reminder2_date end) end), '') Last_Reminder
	                                             , c.Description Client_Category
                                              FROM (SELECT * FROM [Petra_tracker].[dbo].[PPayments] WHERE status = 'Identified and approved') p
                                              FULL JOIN (SELECT s.*, e.EntityKey FROM [Petra_tracker].[dbo].[Schedules] s
                                                           JOIN Petra5.dbo.Entity e ON CONVERT(nvarchar, e.EntityID) = s.company_id) s ON s.entitykey = p.company_code 
                                              LEFT JOIN	(SELECT r.EntityID, rt.Description FROM Petra5.dbo.EntityRoles r
			                                               JOIN  Petra5.dbo.RoleType rt ON rt.RoleTypeID = r.RoleTypeID
			                                              WHERE r.RoleTypeID = 10) c ON CONVERT(nvarchar, c.EntityID) = s.company_id";
                return Database.Tracker.ExecuteQuery<TrackerReport_RMReport>(rm_report_sql);

            }
            catch(Exception)
            {
                return null;
            }
        }

        public static IEnumerable<TrackerReport_CompanyUpdate> get_RM_Company_Update(string company=null)
        {
            try
            {
                string limit = (company == null) ? "" : "WHERE upd.CompanyName= '" + company + "'";

                string rm_update_sql = @"SELECT upd.CompanyName as Company, upd.Tier
                                                , upd.PaymentID, upd.Cont_Date as Contribution_Date, upd.Value_Date
                                                , upd.Cont_Month Contribution_Month, upd.Validation_Status, ISNULL(cat.Description, 'Red') Client_Category
                                           FROM (SELECT ISNULL(p.company_id, s.company_id) CompanyID
	                                                    , ISNULL(p.company_name, s.company) CompanyName
	                                                    , ISNULL(p.tier, s.tier) Tier
	                                                    , ISNULL(p.id, 0) PaymentID
	                                                    , isnull(p.value_date, '') Cont_Date
	                                                    , isnull(p.value_date, '') Value_Date
	                                                    , ISNULL(s.month + s.year, '') Cont_Month
	                                                    , ISNULL(s.validation_status, 'No Schedule') Validation_Status
	                                              FROM [Petra_tracker].[dbo].[PPayments] p
	                                              FULL JOIN [Petra_tracker].[dbo].[Schedules] s on s.payment_id = p.id) upd
                                        LEFT JOIN (SELECT er.EntityID, rt.Description 
                                                      FROM [Petra5].[dbo].[EntityRoles] er
                                                      JOIN [Petra5].[dbo].[RoleType] rt ON rt.RoleTypeID = er.RoleTypeID
                                                     WHERE rt.RoleTypeID in (1019, 1020, 1021)) cat ON cat.EntityID = upd.CompanyID "+limit;

                return Database.Tracker.ExecuteQuery<TrackerReport_CompanyUpdate>(rm_update_sql);
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



        internal static IEnumerable<TrackerReport_ExpectedPayment> get_ExpectedPayment()
        {
            try
            {
                string expected_payment_sql = @"select 'Expected_Payments' Description,
                                                (
                                                select COUNT(id) num
                                                from [Petra_tracker].[dbo].[PPayments] p
                                                where value_date between '' and '' -- 1st and last dates of previous month
                                                and tier = 'Tier 2' --select tier
                                                )
                                                +
                                                (
                                                select COUNT(companyid) num 
                                                from (select Companyid, CompanyKey, tier, CreatedOn from
                                                (select cm.EntityKey CompanyKey, cm.EntityID Companyid, left(p.Description, 6) Tier, cm.CreatedOn
                                                from [Petra5].[dbo].[Entity] cm
                                                join [Petra5].[dbo].[Association] sp on sp.SourceEntityID = cm.EntityID
                                                join [Petra5].[dbo].[EntityClient] ec on ec.EntityID = sp.TargetEntityID
                                                join [Petra5].[dbo].[Entityfund] ef on ef.EntityFundID = ec.EntityID
                                                join [Petra5].[dbo].[Purpose] p on p.PurposeID = ec.PurposeID
                                                where sp.RoleTypeID = 1003
                                                and cm.EntityTypeID = 2) sp
                                                group by Companyid, CompanyKey, tier, CreatedOn) tb
                                                where CreatedOn between '2015-05-01' and '2015-05-31' -- 1st and last dates of previous month
                                                and tier = 'Tier 2' --select tier
                                                ) Number
                                                union
                                                select 'Actual_Payments',
                                                (
                                                select COUNT(id) num
                                                from [Petra_tracker].[dbo].[PPayments] p
                                                where value_date between '' and '' -- 1st and last dates of current month
                                                and tier = 'Tier 2' --select tier
                                                )";
                return Database.Tracker.ExecuteQuery<TrackerReport_ExpectedPayment>(expected_payment_sql);
            }
            catch(Exception)
            {
                return null;
            }
        }

        internal static IEnumerable<TrackerReport_DealDescriptionMatch> get_DealDescriptionMatch()
        {
            try
            {
                string ddm_sql = @"select tracker_descs.company_code CompanyCode, tracker_descs.company_id CompanyID, tracker_descs.tier Tier
                                   , tracker_descs.value_date ValueDate
                                   , (CASE WHEN LEN(left(tracker_descs.dealdesc, patindex('%[^0-9]%', tracker_descs.dealdesc+'.') - 1))=5 
								          THEN CONCAT('0',tracker_descs.dealdesc)
										  ELSE tracker_descs.dealdesc 
										  END)  TrackerDealDesc
	                               , mic_desc.DealDesc MiscDealDesc
                            from
                            (
                                select p.company_code, CONVERT(nvarchar,p.company_id) company_id, p.tier, p.value_date, 
                                      (CONVERT(nvarchar,pd.month) + CONVERT(nvarchar,pd.year) + (case when pd.contribution_type not like '%backpay%' then 'B' else '' end)) dealdesc 
                                  from [Petra_tracker].[dbo].[PPayments] p
                                  join [Petra_tracker].[dbo].[PDealDescriptions] pd on p.id = pd.payment_id
                            ) tracker_descs
                            left join
                            (
                                select CompanyKey, Companyid, Tier, ValueDate, DealDesc from
                                (select et.EntityKey CompanyKey, et.EntityID Companyid, left(P.Description, 6) Tier
                                , ef.FieldValue ValueDate, fd.Explanation DealDesc
                                from [Petra5].[dbo].[EntityField] ef
                                join [Petra5].[dbo].[FieldDef] fd on ef.FieldId = fd.FieldId
                                join [Petra5].[dbo].[Association] asn on asn.TargetEntityID = ef.EntityID
                                join [Petra5].[dbo].[Entity] et on asn.SourceEntityID = et.EntityID
                                join [Petra5].[dbo].[EntityClient] ec on ef.EntityID = ec.EntityID
                                join [Petra5].[dbo].[Purpose] p on p.PurposeID = ec.PurposeID
                                where fd.GroupId = 1049
                                and asn.RoleTypeID = 1003
                                and asn.ParentID is not null) tb
                                group by CompanyKey, Companyid, Tier, ValueDate, DealDesc
                            ) mic_desc on mic_desc.ValueDate = tracker_descs.value_date 
                            and mic_desc.DealDesc = tracker_descs.dealdesc
                            and mic_desc.ValueDate = CONVERT(nvarchar, tracker_descs.value_date)";

                return Database.Tracker.ExecuteQuery<TrackerReport_DealDescriptionMatch>(ddm_sql);
            } 
            catch(Exception)
            {
                return null;
            }
        }
    }

    #region Report Models

    class TrackerReport_ExpectedPayment
    {
        public string Description { get; set; }
        public int Number { get; set; }
    }

    class TrackerReport_DealDescriptionMatch
    {
        public string CompanyCode { get; set; } 
        public string CompanyId { get; set; }
        public string Tier { get; set; }
        public DateTime ValueDate { get; set; }
        public string TrackerDealDesc 
        { 
            get; 
            set; 
        }
        public string MiscDealDesc {get; set; }
    }
    
    class TrackerReport_RMReport
    {
        public string Company_ID { get; set; }
        public string Company_Name { get; set; }
	    public string Company_Code { get; set; }
	    public string Tier { get; set; }
	    public string Contributionperiod { get; set; }
	    public string contributiontype { get; set; }
	    public DateTime Payment_Date { get; set; }
	    public decimal? Payment_Amount { get; set; }
	    public string Validation_Status { get; set; }
	    public DateTime Last_Reminder { get; set; }
        public string Client_Category { get; set; }
    }
    class TrackerReport_CompanyUpdate
    {
        public string Company { get; set; }
        public string Tier { get; set; }
        public Int32 PaymentID { get; set; }
        public DateTime Contribution_Date { get; set; }
        public DateTime Value_Date { get; set; }
        public int Contribution_Month { get; set; }
        public string Validation_Status { get; set; }
        public string Client_Category { get; set; }
    }
    
    #endregion
}
