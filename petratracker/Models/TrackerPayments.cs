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

        public static Payment GetSubscription(string company_code, string tier, decimal? amount, DateTime dealDate)
        {
            try
            {
                return (from p in TrackerDB.Tracker.Payments
                        where p.company_code == company_code &&
                              p.transaction_date == dealDate &&
                              p.transaction_amount == amount
                        select p).Single();
            } 
            catch(Exception)
            {
                throw;
            }
        }

        public static IEnumerable<Payment> GetSubscriptions(int job_id, string sub_status)
        {
            return (from p in TrackerDB.Tracker.Payments where p.job_id == job_id && p.status == sub_status select p);
        }

        public static bool read_microgen_data(string doc_source, string job_type, string deal_description)
        {
            try
            {
                string fullPathToExcel = doc_source;
                string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=yes'", fullPathToExcel);
                DataTable dt = Utils.GetDataTable("SELECT * from [Report$]", connString);

                int jobId = TrackerJobs.Add(job_type, deal_description);

                foreach (DataRow dr in dt.Rows)
                {
                    string trans_date_str = dr["Transaction Date"].ToString();
                    string value_date_str = dr["Value Date"].ToString();
                    char[] charSeparators = new char[] { '/' };
                    string[] value_date_res = value_date_str.Split(charSeparators);
                    string[] trans_date_res = trans_date_str.Split(charSeparators);

                    // Insert new payment
                    Payment objPayment = new Payment();
                    objPayment.transaction_ref_no = get_trans_ref_code(dr["Value Date"].ToString(), dr["Transaction Date"].ToString());
                    objPayment.job_id = jobId;
                    objPayment.transaction_details = dr["Transaction Details"].ToString();
                    DateTime trans_date = new DateTime(int.Parse(trans_date_res[2]), int.Parse(trans_date_res[1]), int.Parse(trans_date_res[0]));
                    DateTime value_date = new DateTime(int.Parse(value_date_res[2]), int.Parse(value_date_res[1]), int.Parse(value_date_res[0]));
                    objPayment.transaction_date = trans_date;
                    objPayment.value_date = value_date;
                    objPayment.subscription_value_date = value_date;
                    objPayment.transaction_amount = decimal.Parse(dr["Transaction Amount"].ToString());
                    objPayment.subscription_amount = decimal.Parse(dr["Transaction Amount"].ToString());

                    objPayment.status = (dr["Dr / Cr Indicator"].ToString() == "Credit") ? "Unidentified" : "Returned";
                    objPayment.owner = TrackerUser.GetCurrentUser().id;
                    objPayment.created_at = DateTime.Now;
                    objPayment.updated_at = DateTime.Now;
                    TrackerDB.Tracker.Payments.InsertOnSubmit(objPayment);
                    TrackerDB.Tracker.SubmitChanges();
                }

                return true;
            }
            catch (Exception uploadError)
            {
                MessageBox.Show("An error occured while uploading the file.\n" + uploadError.Message, "Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion

        #region Private Helper Methods

        private static string get_trans_ref_code(string val_date, string trans_date)
        {
            return "TR" + val_date.Replace("/", string.Empty) + trans_date.Replace("/", string.Empty);
        }

        #endregion
    }
}
