using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

using petratracker.Utility;

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

        public static Payment GetSubscription(string company_id, string tier, int month, int year, string ct)
        {
            try
            {
                return (from p in TrackerDB.Tracker.Payments
                        where p.company_code == company_id &&
                              p.deal_description_period == (month.ToString() + " " + year.ToString()) &&
                              p.deal_description == ct
                        select p).Single();
            } 
            catch(Exception)
            {
                throw;
            }
        }

        public static IEnumerable<Payment> GetSubscriptions(int job_id, string sub_status="",bool showAll=false)
        {
            if (TrackerUser.IsCurrentUserOps())
            {
                return GetOpsUserSubscriptions(job_id, sub_status);
            }
            else
            {
                if (sub_status != string.Empty)
                {
                    return (from p in TrackerDB.Tracker.Payments where p.job_id == job_id && p.status.Trim() == sub_status select p);
                }
                else
                {
                    return (from p in TrackerDB.Tracker.Payments where p.job_id == job_id select p);
                }
            }
        }

        public static IEnumerable<Payment> GetOpsUserSubscriptions(int job_id, string sub_status = "")
        {
            if (sub_status != string.Empty)
            {
                return (from p in TrackerDB.Tracker.Payments 
                        where p.job_id == job_id && p.status.Trim() == sub_status && p.status.Trim() != Constants.PAYMENT_STATUS_IDENTIFIED
                        select p);
            }
            else
            {
                return (from p in TrackerDB.Tracker.Payments
                        where p.job_id == job_id && p.status.Trim() != Constants.PAYMENT_STATUS_IDENTIFIED 
                        select p);
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
                foreach (DataRow dr in dt.Rows)
                {
                   

                    // Insert new payment
                    Payment objPayment = new Payment();                 
                    objPayment.job_id = jobId;
                    objPayment.tier = tier;
                    objPayment.transaction_details = dr["Transaction Details"].ToString();
                    objPayment.transaction_date = (DateTime)dr["Transaction Date"];
                    objPayment.value_date = (DateTime)dr["Value Date"];
                    objPayment.subscription_value_date = (DateTime)dr["Transaction Date"];
                    objPayment.transaction_amount = decimal.Parse(dr["Transaction Amount"].ToString());
                    objPayment.subscription_amount = decimal.Parse(dr["Transaction Amount"].ToString());
                    objPayment.transaction_ref_no = get_trans_ref_code((DateTime)dr["Value Date"], tier);
                    objPayment.status = (dr["Dr / Cr Indicator"].ToString() == "Credit") ? "Unidentified" : "Returned";
                    objPayment.owner = TrackerUser.GetCurrentUser().id;
                    objPayment.created_at = DateTime.Now;
                    objPayment.updated_at = DateTime.Now;
                    TrackerDB.Tracker.Payments.InsertOnSubmit(objPayment);
                    TrackerDB.Tracker.SubmitChanges();
                    ini_inc++;
                }

                return true;
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
                var subscription = (from p in TrackerDB.Tracker.Payments
                                    where p.transaction_ref_no == payment_ref_no
                                    select p).Single();

                PTasPayment objPayment = new PTasPayment();
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
                objPayment.PaymentID = 1;
                objPayment.ActionUserID = 36; //Declare ActionUserID as constant
                objPayment.Tier = subscription.tier.Replace(" ",string.Empty);
                TrackerDB.PTAS.Payments.InsertOnSubmit(objPayment);            
                TrackerDB.PTAS.SubmitChanges();
                return true;
            
            }
            catch(Exception)
            {
                MessageBox.Show("An error occured while pushing subscription into PTAS payments.\n");
                return false;
            }
        }

        public static bool send_approval_email()
        {
            return true;
        }

        public static string [] get_microgen_data(string company_code, string tier)
        {
            string [] res = new string[2];
            string use_comp_code = company_code;
            
            if (company_code != null) { use_comp_code = company_code; } else { use_comp_code = "CO00000776"; }

            var download_data = from HC in
                                    (
                                        (from cm in TrackerDB.Microgen.Entities
                                         join spr in TrackerDB.Microgen.Associations on new { SourceEntityID = cm.EntityID } equals new { SourceEntityID = spr.SourceEntityID }
                                         join owr in TrackerDB.Microgen.Associations on new { SourceEntityID = spr.TargetEntityID } equals new { SourceEntityID = owr.SourceEntityID }
                                         join hc in TrackerDB.Microgen.Entities on new { EntityID = spr.TargetEntityID } equals new { EntityID = hc.EntityID }
                                         join ef in TrackerDB.Microgen.EntityClients on new { EntityID = owr.TargetEntityID } equals new { EntityID = ef.EntityID }
                                         join prs in TrackerDB.Microgen.Associations on new { TargetEntityID = ef.EntityID } equals new { TargetEntityID = prs.TargetEntityID }
                                         join fd in TrackerDB.Microgen.Entities on new { EntityID = prs.SourceEntityID } equals new { EntityID = fd.EntityID }
                                         join p in TrackerDB.Microgen.Purposes on new { PurposeID = Convert.ToInt32(ef.PurposeID) } equals new { PurposeID = p.PurposeID }
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
       

        #endregion

        #region Private Helper Methods


        private static string get_company_name(string company_code)
        {
            string comp_name = string.Empty;
            var companies = (from c in TrackerDB.Microgen.cclv_AllEntities
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
            var companies = (from c in TrackerDB.Microgen.EntityContacts
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
            var companies = (from c in TrackerDB.Microgen.cclv_AllEntities
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
            IEnumerable<Payment> seq = (from p in TrackerDB.Tracker.Payments where p.value_date == valueDate select p );
            return seq.Count();
        }
        
        private static string get_trans_ref_code(DateTime Value_Date,string tier)
        {
            return "TR" + Value_Date.ToString("ddMMyyyy") + tier.ToUpper().Remove(4, 1) + get_seqence_no(Value_Date).ToString("D"+5);
        }

        #endregion


    }
}
