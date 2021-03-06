﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

using petratracker.Utility;
using System.Data.Linq.SqlClient;
using System.Collections;

namespace petratracker.Models
{
    public class TrackerPayment
    {
        #region Private Members

        private bool _isUploaded = false;

        #endregion
     
        #region Public Properties
        
        public bool IsUploaded
        {
            set { _isUploaded = value; }
            get { return _isUploaded; }
        }
        
        #endregion

        #region Constructor

        public TrackerPayment()
        {
        }

        #endregion     

        #region Public Helper Methods
        private static string GetMonth(int month)
        {
            string name = "";
            switch (month)
            {
                case 1: name= "January"; break;
                case 2: name= "February"; break;
                case 3: name= "March"; break;
                case 4: name= "April"; break;
                case 5: name= "May"; break;
                case 6: name= "June"; break;
                case 7: name= "July"; break;
                case 8: name= "August"; break;
                case 9: name= "September"; break;
                case 10: name= "October"; break;
                case 11: name= "November"; break;
                case 12: name= "December"; break;
            }

            return name;
        }

        public static bool IsLinkedSubscription(int funddealid)
        {
            try
            {
                var fd = (from p in Database.PTAS.PaymentScheduleLinks                      
                          where p.FundDealID == funddealid
                          select p).Count();
                return (fd > 0) ? true : false;
            }
            catch (Exception ex)
            {
                LogUtil.LogError("TrackerPayments", "IsLinkedSubscription", ex);
                return false;
            }
        }

        public static PPayment GetSubscription(int id)
        {
            return (from p in Database.Tracker.PPayments where p.id == id select p).First();
        }

        public static PPayment GetSubscription(string company_id, string tier, int month, int year, int ctid)
        {
            try
            {
                return (from p in Database.Tracker.PPayments
                        join pd in Database.Tracker.PDealDescriptions on p.id equals pd.payment_id
                        where p.company_code == company_id &&
                              p.tier == tier &&
                              pd.year == year && pd.month == month &&
                              pd.contribution_type_id == ctid
                        select p).First();
            } 
            catch(Exception)
            {
                throw;
            }
        }

        public static IEnumerable<SubscriptionsView> GetSubscriptions(int job_id, string sub_status = "", bool showAll = false)
        {
            if (TrackerUser.IsCurrentUserOps())
            {
                return GetOpsUserSubscriptions(job_id, sub_status);
            }
            else
            {
                if (sub_status != string.Empty)
                {
                    return (from p in Database.Tracker.PPayments
                            where p.job_id == job_id && p.status.Trim() == sub_status    
                            select new SubscriptionsView
                            {
                                Id = p.id,
                                Job_Id = p.job_id ?? 0,
                                Transaction_Ref = p.transaction_ref_no,
                                Value_Date = p.value_date,
                                Trans_Details = p.transaction_details,
                                Subscription_Value_Date = p.subscription_value_date,
                                Subscription_Amount = p.subscription_amount ?? 0,
                                Company_Code = p.company_code,
                                Company_Name = p.company_name,
                                Tier = p.tier,
                                Status = p.status,
                                Deal_Description = GetPaymentDealDesc(p.id)
                            });

                }
                else
                {
                    return (from p in Database.Tracker.PPayments
                            where p.job_id == job_id
                            select new SubscriptionsView
                            {
                                Id = p.id,
                                Job_Id = p.job_id ?? 0,
                                Transaction_Ref = p.transaction_ref_no,
                                Value_Date = p.value_date,
                                Trans_Details = p.transaction_details,
                                Subscription_Value_Date = p.subscription_value_date,
                                Subscription_Amount = p.subscription_amount ?? 0,
                                Company_Code = p.company_code,
                                Company_Name = p.company_name,
                                Tier = p.tier,
                                Status = p.status,
                                Deal_Description = GetPaymentDealDesc(p.id)
                            });

                }
            }
        }

        public static IEnumerable GetSubscriptionsBySearch(string term, string status)
        {
            term = "%" + term + "%";
            if (status != null && status != "All")
            {
                return (from p in Database.Tracker.PPayments
                        where p.status.Trim() == status &&
                            (SqlMethods.Like(p.company_name, term) || SqlMethods.Like(p.tier, term) ||
                             SqlMethods.Like(p.company_code, term) || SqlMethods.Like(p.transaction_ref_no, term) ||
                             SqlMethods.Like(p.transaction_details, term) || SqlMethods.Like(p.User.first_name, term) ||
                             SqlMethods.Like(p.User.last_name, term))
                        orderby p.created_at descending
                        select new SubscriptionsView
                            {
                                Id = p.id,
                                Job_Id = p.job_id ?? 0,
                                Transaction_Ref = p.transaction_ref_no,
                                Value_Date = p.value_date,
                                Trans_Details = p.transaction_details,
                                Subscription_Value_Date = p.subscription_value_date,
                                Subscription_Amount = p.subscription_amount ?? 0,
                                Company_Code = p.company_code,
                                Company_Name = p.company_name,
                                Tier = p.tier,
                                Status = p.status,
                                Deal_Description = GetPaymentDealDesc(p.id)
                            });
            }
            else
            {
                return (from p in Database.Tracker.PPayments
                        where
                            (SqlMethods.Like(p.company_name, term) || SqlMethods.Like(p.status, term)  ||
                             SqlMethods.Like(p.company_code, term) || SqlMethods.Like(p.transaction_ref_no, term) ||
                             SqlMethods.Like(p.transaction_details, term) || SqlMethods.Like(p.User.first_name, term) ||
                             SqlMethods.Like(p.User.last_name, term))
                        orderby p.created_at descending
                        select new SubscriptionsView
                        {
                            Id = p.id,
                            Job_Id = p.job_id ?? 0,
                            Transaction_Ref = p.transaction_ref_no,
                            Value_Date = p.value_date,
                            Trans_Details = p.transaction_details,
                            Subscription_Value_Date = p.subscription_value_date,
                            Subscription_Amount = p.subscription_amount ?? 0,
                            Company_Code = p.company_code,
                            Company_Name = p.company_name,
                            Tier = p.tier,
                            Status = p.status,
                            Deal_Description = GetPaymentDealDesc(p.id)
                        });
            }
        }

        public static IEnumerable GetSubscriptionsBySearch(string com, string t, string ct, string vd, string m, string y, int jid, string status = null)
        {
            #region Build sql
            string sql = (jid == -1) ? "SELECT p.* FROM PPayments p {0} WHERE 1=1 " : "SELECT p.* FROM PPayments p {0} WHERE job_id = " + jid.ToString();
            string join = "";

            if (status != null && status != "All") { sql += " AND status='" + status + "'"; }

            if (com != null) { sql += " AND p.company_name LIKE '%" + com.Trim() + "%'"; }
            if (t != null) { sql += " AND p.tier='" + t.Trim() + "'"; }
            if (vd != null) {
                string[] s = vd.Split('/');
                vd = s[2].Substring(0,4) + '-' + s[0] + '-' + s[1];
                sql += " AND p.value_date='" + vd + "'"; 
            }

            if (ct != null || m != null || y != null)
            {
                join = " JOIN PDealDescriptions dd ON p.id = dd.payment_id ";
                if (ct != null) { join += " AND dd.contribution_type = '" + ct + "'"; }
                if (m != null) { join += " AND dd.month= '" + m.Trim() + "'"; }
                if (y != null) { join += " AND dd.year= '" + y.Trim() + "'"; }
            }

            sql += " ORDER BY p.created_at DESC";

            sql = string.Format(sql, join);

            //Console.WriteLine(sql);
            #endregion

            try
            {
                // hack to return right type of object
                List<SubscriptionsView> ids = new List<SubscriptionsView>();
                IEnumerable x = Database.Tracker.ExecuteQuery<PPayment>(sql);
                foreach (PPayment p in x)
                {
                    ids.Add(new SubscriptionsView
                            {
                                Id = p.id,
                                Job_Id = p.job_id ?? 0,
                                Transaction_Ref = p.transaction_ref_no,
                                Value_Date = p.value_date,
                                Trans_Details = p.transaction_details,
                                Subscription_Value_Date = p.subscription_value_date,
                                Subscription_Amount = p.subscription_amount ?? 0,
                                Company_Code = p.company_code,
                                Company_Name = p.company_name,
                                Tier = p.tier,
                                Status = p.status,
                                Deal_Description = GetPaymentDealDesc(p.id)
                            });
                }
                return ids.AsEnumerable();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        public static IEnumerable GetSubscriptionsByCompany(string company, string status)
        {
            if (status != null && status != "All")
            {
                return (from j in Database.Tracker.PPayments where j.company_name == company && j.status.Trim() == status orderby j.created_at descending select j);
            }
            else
            {
                return (from j in Database.Tracker.PPayments where j.company_name == company orderby j.created_at descending select j);
            }
        }

        public static IEnumerable<SubscriptionsView> GetAllSubscriptions(string sub_status = "", bool showAll = false)
        {
            if (TrackerUser.IsCurrentUserOps())
            {
                return GetAllOpsUserSubscriptions(sub_status);
            }
            else
            {
                if (sub_status != string.Empty)
                {
                    return (from p in Database.Tracker.PPayments
                            join d in Database.Tracker.PDealDescriptions on p.id equals d.payment_id
                            where p.status.Trim() == sub_status && 
                            !Database.Tracker.Schedules.Any(y=>y.payment_id==p.id)
                            select new SubscriptionsView
                            {
                                Id = p.id,
                                Job_Id = p.job_id ?? 0,
                                Transaction_Ref = p.transaction_ref_no,
                                Value_Date = p.value_date,
                                Trans_Details = p.transaction_details,
                                Subscription_Value_Date = p.subscription_value_date,
                                Subscription_Amount = p.subscription_amount ?? 0,
                                Company_Code = p.company_code,
                                Company_Name = p.company_name,
                                Tier = p.tier,
                                Status = p.status,
                                Deal_Description = GetPaymentDealDesc(p.id)
                            });

                        
                }
                else
                {
                    return (from p in Database.Tracker.PPayments
                            join d in Database.Tracker.PDealDescriptions on p.id equals d.payment_id
                            where ! Database.Tracker.Schedules.Any(y => y.payment_id == p.id)
                            select new SubscriptionsView
                            {
                                Id = p.id,
                                Job_Id = p.job_id ?? 0,
                                Transaction_Ref = p.transaction_ref_no,
                                Value_Date = p.value_date,
                                Trans_Details = p.transaction_details,
                                Subscription_Value_Date = p.subscription_value_date,
                                Subscription_Amount = p.subscription_amount ?? 0,
                                Company_Code = p.company_code,
                                Company_Name = p.company_name,
                                Tier = p.tier,
                                Status = p.status,
                                Deal_Description = GetPaymentDealDesc(p.id)
                            });

                }
            }
        }

        public static IEnumerable<TrackerPaymentDealDescriptions> GetPaymentDealDescriptions(int payment_id)
        {

            if (payment_id > 0)
            {
                return (from pdd in Database.Tracker.PDealDescriptions where pdd.payment_id == payment_id select new TrackerPaymentDealDescriptions() { id = pdd.id, month = GetMonth(pdd.month), year = pdd.year.ToString(), contribution_type = pdd.contribution_type  });
            }
            else
            {
                return null;
            }

        }

        public static string GetPaymentDealDesc(int payment_id)
        {
            string desc = string.Empty;

            if (payment_id > 0)
            {
               var deals = (from pdd in Database.Tracker.PDealDescriptions
                        where pdd.payment_id == payment_id
                        select new TrackerPaymentDealDescriptions() 
                        { id = pdd.id, month = GetMonth(pdd.month), year = pdd.year.ToString(), contribution_type = pdd.contribution_type });

              foreach(TrackerPaymentDealDescriptions vals in deals)
              {
                  desc += string.Concat(vals.month," ",vals.year,",");
                  
              }
              if (desc.Length > 0) { return desc.Substring(0, desc.Length - 1); }
              else { return desc; }
               
            }
            else
            {
                return desc;
            }

        }

        public static IEnumerable<SubscriptionsView> GetAllOpsUserSubscriptions(string sub_status = "")
        {
            if (sub_status != string.Empty)
            {
                return (from p in Database.Tracker.PPayments
                        join d in Database.Tracker.PDealDescriptions on p.id equals d.payment_id
                        where p.status.Trim() == sub_status && p.status.Trim() != Constants.PAYMENT_STATUS_IDENTIFIED
                        && !Database.Tracker.Schedules.Any(y => y.payment_id == p.id)
                        select new SubscriptionsView
                        {
                            Id = p.id,
                            Job_Id = p.job_id ?? 0,
                            Transaction_Ref = p.transaction_ref_no,
                            Value_Date = p.value_date,
                            Trans_Details = p.transaction_details,
                            Subscription_Value_Date = p.subscription_value_date,
                            Subscription_Amount = p.subscription_amount ?? 0,
                            Company_Code = p.company_code,
                            Company_Name = p.company_name,
                            Tier = p.tier,
                            Status = p.status,
                            Deal_Description = GetPaymentDealDesc(p.id)
                        });

            }
            else
            {
                return (from p in Database.Tracker.PPayments
                        join d in Database.Tracker.PDealDescriptions on p.id equals d.payment_id
                        where p.status.Trim() != Constants.PAYMENT_STATUS_IDENTIFIED &&
                        !Database.Tracker.Schedules.Any(y => y.payment_id == p.id)
                        select new SubscriptionsView
                        {
                            Id = p.id,
                            Job_Id = p.job_id ?? 0,
                            Transaction_Ref = p.transaction_ref_no,
                            Value_Date = p.value_date,
                            Trans_Details = p.transaction_details,
                            Subscription_Value_Date = p.subscription_value_date,
                            Subscription_Amount = p.subscription_amount ?? 0,
                            Company_Code = p.company_code,
                            Company_Name = p.company_name,
                            Tier = p.tier,
                            Status = p.status,
                            Deal_Description = GetPaymentDealDesc(p.id)
                        });

            }
        }

        public static IEnumerable<SubscriptionsView> GetOpsUserSubscriptions(int job_id, string sub_status = "")
        {
            if (sub_status != string.Empty)
            {
                return (from p in Database.Tracker.PPayments 
                        join d in Database.Tracker.PDealDescriptions on p.id equals d.payment_id
                        where p.job_id == job_id && p.status.Trim() == sub_status && p.status.Trim() != Constants.PAYMENT_STATUS_IDENTIFIED
                        && !Database.Tracker.Schedules.Any(y => y.payment_id == p.id)
                        select new SubscriptionsView
                        {
                                                        Id = p.id,
                                                        Job_Id = p.job_id ?? 0,
                                                        Transaction_Ref = p.transaction_ref_no,
                                                        Value_Date = p.value_date,
                                                        Trans_Details = p.transaction_details,
                                                        Subscription_Value_Date = p.subscription_value_date,
                                                        Subscription_Amount = p.subscription_amount??0,
                                                        Company_Code = p.company_code,
                                                        Company_Name = p.company_name,
                                                        Tier = p.tier,
                                                        Status = p.status,
                                                        Deal_Description = GetPaymentDealDesc(p.id)
                        });
            }
            else
            {
                return (from p in Database.Tracker.PPayments
                        join d in Database.Tracker.PDealDescriptions on p.id equals d.payment_id
                        where p.job_id == job_id && p.status.Trim() != Constants.PAYMENT_STATUS_IDENTIFIED
                        && !Database.Tracker.Schedules.Any(y => y.payment_id == p.id)
                        select new SubscriptionsView
                        {
                            Id = p.id,
                            Job_Id = p.job_id??0,
                            Transaction_Ref = p.transaction_ref_no,
                            Value_Date = p.value_date,
                            Trans_Details = p.transaction_details,
                            Subscription_Value_Date = p.subscription_value_date,
                            Subscription_Amount = p.subscription_amount ?? 0,
                            Company_Code = p.company_code,
                            Company_Name = p.company_name,
                            Tier = p.tier,
                            Status = p.status,
                            Deal_Description = GetPaymentDealDesc(p.id)
                        });
            }
        }

        public static bool read_microgen_data(string doc_source, string job_type, string deal_description, string tier)
        {
            try
            {
                string fullPathToExcel = doc_source;
                string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=yes'", fullPathToExcel);
                DataTable dt = Utils.GetDataTable("SELECT * from [Report$]", connString);

                int jobId = TrackerJobs.Add(job_type, deal_description, tier);
                int ini_inc = 1;
                DateTime valDate,transDate;
                if (jobId > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {



                        //Cast value date 
                        if (dr["Value Date"] is DateTime) { valDate = (DateTime)dr["Value Date"]; }
                        else
                        {
                            string value_date_str = dr["Value Date"].ToString();
                            char[] charSeparators = new char[] { '/', '-' };
                            string[] value_date_res = value_date_str.Split(charSeparators);
                            valDate = new DateTime(int.Parse(value_date_res[2]), int.Parse(value_date_res[1]), int.Parse(value_date_res[0]));
                        }

                        //Cast transaction date 
                        if (dr["Transaction Date"] is DateTime) { transDate = (DateTime)dr["Transaction Date"]; }
                        else
                        {
                            string trans_date_str = dr["Transaction Date"].ToString();
                            char[] charSeparators = new char[] { '/', '-' };
                            string[] trans_date_res = trans_date_str.Split(charSeparators);
                            transDate = new DateTime(int.Parse(trans_date_res[2]), int.Parse(trans_date_res[1]), int.Parse(trans_date_res[0]));
                        }

                        // Insert new payment
                        PPayment objPayment = new PPayment();
                        objPayment.job_id = jobId;
                        objPayment.tier = tier;
                        objPayment.transaction_details = dr["Transaction Details"].ToString();
                        objPayment.transaction_date = transDate;
                        objPayment.value_date = valDate;
                        objPayment.subscription_value_date = valDate;
                        objPayment.transaction_amount = decimal.Parse(dr["Transaction Amount"].ToString());
                        objPayment.subscription_amount = decimal.Parse(dr["Transaction Amount"].ToString());
                        objPayment.transaction_ref_no = get_trans_ref_code(valDate, tier);

                        objPayment.status = (dr["Dr / Cr Indicator"].ToString() == "Credit") ? "Unidentified" : "Returned";
                        objPayment.owner = TrackerUser.GetCurrentUser().id;
                        objPayment.created_at = DateTime.Now;
                        objPayment.updated_at = DateTime.Now;
                        Database.Tracker.PPayments.InsertOnSubmit(objPayment);
                        Database.Tracker.SubmitChanges();
                        ini_inc++;
                    }
                    return true;
                }
                else { MessageBox.Show("Upload description already exists in jobs table."); return false; }
               
            }
            catch (Exception uploadError)
            {
                MessageBox.Show("An error occured while uploading the file.\n" + uploadError.Message+"\n"+uploadError.Source+"\n"+uploadError.StackTrace, "Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool push_payment_to_PTAS(string payment_ref_no)
        {

            try
            {
                var subscription = (from p in Database.Tracker.PPayments
                                    where p.transaction_ref_no == payment_ref_no
                                    select p).Single();

                Payment objPayment = new Payment();
                objPayment.PaymentStatusID = 1;
                objPayment.InsertedDate = DateTime.Now;
                objPayment.ContributionDate = subscription.subscription_value_date;
                objPayment.ValueDate = subscription.value_date;
                objPayment.TransactionAmount = subscription.transaction_amount;
                objPayment.TransactionDetail = subscription.transaction_details;
                objPayment.DRCRFlag = string.Empty;
                objPayment.TransactionReference = subscription.transaction_ref_no;
                objPayment.RETURNED = string.Empty;
                objPayment.CompanyCode = subscription.company_code;
                objPayment.CompanyName = get_company_name(subscription.company_code);
                
                objPayment.ActionUserID = 36; //Declare ActionUserID as constant
                objPayment.Tier = subscription.tier.Replace(" ",string.Empty);
                Database.PTAS.Payments.InsertOnSubmit(objPayment);            
                Database.PTAS.SubmitChanges();
                return true;
            
            }
            catch(Exception)
            {
                MessageBox.Show("An error occured while pushing subscription into PTAS payments.\n");
                return false;
            }
        }

        public static bool update_PTAS_payment(string payment_ref_no)
        {

            try
            {
                var tracker_payment = (from p in Database.Tracker.PPayments
                                    where p.transaction_ref_no == payment_ref_no
                                    select p).Single();

                var PTAS_payment = (from p in Database.PTAS.Payments
                                    where p.TransactionReference == payment_ref_no
                                    select p).Single();

                    //Update PTAS Payment
                    PTAS_payment.ContributionDate = tracker_payment.subscription_value_date;
                    PTAS_payment.ValueDate = tracker_payment.value_date;
                    PTAS_payment.TransactionAmount = tracker_payment.transaction_amount;
                    PTAS_payment.TransactionDetail = tracker_payment.transaction_details;
                    PTAS_payment.CompanyCode = tracker_payment.company_code;
                    PTAS_payment.CompanyName = get_company_name(tracker_payment.company_code);

                    Database.PTAS.SubmitChanges();
                    return true;

            }
            catch (Exception)
            {
                MessageBox.Show("An error occured while updating PTAS payment.\n");
                return false;
            }
        }

        public static bool send_approval_email()
        {
            return true;
        }

        public static int get_company_id_by_code(string companyCode)
        {
            int company_id = 0;

            var comp_id = from et in Database.Microgen.Entities
                          where et.EntityKey == companyCode
                          select new { et.EntityID };

            foreach (var e in comp_id)
            {
                company_id = e.EntityID;
            }

            return company_id;
        }

        public static string [] get_microgen_data(string company_code, string tier)
        {
            string [] res = new string[2];
            string use_comp_code = company_code;
            
            if (company_code != null) { use_comp_code = company_code; } else { use_comp_code = "CO00000776"; }

            var download_data = from HC in
                                    (
                                        (from cm in Database.Microgen.Entities
                                         join spr in Database.Microgen.Associations on new { SourceEntityID = cm.EntityID } equals new { SourceEntityID = spr.SourceEntityID }
                                         join owr in Database.Microgen.Associations on new { SourceEntityID = spr.TargetEntityID } equals new { SourceEntityID = owr.SourceEntityID }
                                         join hc in Database.Microgen.Entities on new { EntityID = spr.TargetEntityID } equals new { EntityID = hc.EntityID }
                                         join ef in Database.Microgen.EntityClients on new { EntityID = owr.TargetEntityID } equals new { EntityID = ef.EntityID }
                                         join prs in Database.Microgen.Associations on new { TargetEntityID = ef.EntityID } equals new { TargetEntityID = prs.TargetEntityID }
                                         join fd in Database.Microgen.Entities on new { EntityID = prs.SourceEntityID } equals new { EntityID = fd.EntityID }
                                         join p in Database.Microgen.Purposes on new { PurposeID = Convert.ToInt32(ef.PurposeID) } equals new { PurposeID = p.PurposeID }
                                         where
                                           hc.EntityTypeID == 1002 &&
                                           spr.RoleTypeID == 1003 &&
                                           prs.RoleTypeID == 1005 &&
                                           owr.RoleTypeID == 2 &&
                                           (new [] { "1012", "1004", "1007" , "1016"}).Contains(ef.PurposeID.ToString())
                                         select new
                                         {
                                             CM = cm.EntityKey,
                                             HC = hc.EntityKey,
                                             FD = fd.EntityKey,
                                             Tier = p.Description.Substring(0, 6)
                                         }))
                                where
                                  HC.CM == use_comp_code  &&
                                  HC.Tier == tier
                                select new
                                {
                                    FundCode = HC.FD,
                                    FundHolderCode = HC.HC,
                                   
                                };

                foreach(var data in download_data)
                {
                    res[0] = data.FundCode;
                    res[1] = data.FundHolderCode;
                }

                return res;
        }

        public static void Approve(PPayment payment)
        {
            payment.status = Constants.PAYMENT_STATUS_IDENTIFIED_APPROVED;
            payment.updated_at = DateTime.Now;
            Database.Tracker.SubmitChanges();
        }

        public static void Reject(PPayment payment)
        {
            payment.status = Constants.PAYMENT_STATUS_REJECTED;
            payment.updated_at = DateTime.Now;
            Database.Tracker.SubmitChanges();
        }

        #endregion

        #region Private Helper Methods


        private static string get_company_name(string company_code)
        {
            string comp_name = string.Empty;
            var companies = (from c in Database.Microgen.cclv_AllEntities
                             where c.EntityKey == company_code
                             select c);

            foreach (cclv_AllEntity ini_comp in companies)
            {

                comp_name = ini_comp.EntityKey;
            }

            return comp_name;
        }

        private static string get_company_email(string company_code)
        {
            string comp_email = string.Empty;
            var companies = (from c in Database.Microgen.EntityContacts
                             where c.EntityID == get_company_id(company_code)
                             select c);

            foreach (EntityContact ini_comp in companies)
            {
                comp_email = ini_comp.Email;
            }

            return comp_email;
        }

        private static int get_company_id(string company_code)
        {
            int comp_id = 0;
            var companies = (from c in Database.Microgen.cclv_AllEntities
                             where c.EntityKey == company_code
                             select c);

            foreach (cclv_AllEntity ini_comp in companies)
            {
                comp_id = ini_comp.EntityID;
            }

            return comp_id;
        }

        private static int get_seqence_no(DateTime valueDate)
        {
            IEnumerable<PPayment> seq = (from p in Database.Tracker.PPayments where p.value_date == valueDate select p );
            return seq.Count();
        }
        
        private static string get_trans_ref_code(DateTime Value_Date,string tier)
        {
            return "TR" + Value_Date.ToString("ddMMyyyy") + tier.ToUpper().Remove(4, 1) + get_seqence_no(Value_Date).ToString("D"+5);
        }

        #endregion
    }
}
