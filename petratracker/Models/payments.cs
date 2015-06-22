using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace petratracker.Models
{
    public class payments
    {


        Models.Payment newPayment = new Payment();

        private DataTable GetDataTable(string sql, string connectionString)
        {
            DataTable dt = new DataTable();
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {

                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                    {
                        using (OleDbDataReader rdr = cmd.ExecuteReader())
                        {
                            dt.Load(rdr);

                        }
                    }
                }
            }
            catch (Exception errMsg)
            {
                MessageBox.Show(errMsg.Message);
                //log error
            }
            return dt;
        }


        public void read_microgen_data(string doc_source, string job_type)
        {
            try
            {
                string fullPathToExcel = doc_source;
                string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=yes'", fullPathToExcel);
                DataTable dt = GetDataTable("SELECT * from [Report$]", connString);


                //Insert payment into jobs
                TrackerDataContext newPaymentObj = (App.Current as App).TrackerDBo;


                //Create new job
                Job objJob = new Job();

                objJob.job_type = job_type;
                objJob.status = "In Progress";
                newPaymentObj.Jobs.InsertOnSubmit(objJob);
                newPaymentObj.SubmitChanges();

                foreach (DataRow dr in dt.Rows)
                {

                    if (true)
                    {

                        string trans_date_str = dr["Transaction Date"].ToString();
                        string value_date_str = dr["Value Date"].ToString();
                        char[] charSeparators = new char[] { '/' };

                        string[] value_date_res = value_date_str.Split(charSeparators);
                        string[] trans_date_res = trans_date_str.Split(charSeparators);

                        //Create new payment
                        Payment objPayment = new Payment();
                        objPayment.transaction_ref_no = get_trans_ref_code(dr["Value Date"].ToString(), dr["Transaction Date"].ToString());
                        //objPayment.job_id = objJob.id;
                        objPayment.transaction_details = dr["Transaction Details"].ToString();

                        DateTime trans_date = new DateTime(int.Parse(trans_date_res[2]), int.Parse(trans_date_res[1]), int.Parse(trans_date_res[0]));
                        DateTime value_date = new DateTime(int.Parse(value_date_res[2]), int.Parse(value_date_res[1]), int.Parse(value_date_res[0]));
                        objPayment.transaction_date = trans_date;
                        objPayment.value_date = value_date;

                        objPayment.transaction_amount = decimal.Parse(dr["Transaction Amount"].ToString());
                        objPayment.status = "Pending";
                        newPaymentObj.Payments.InsertOnSubmit(objPayment);
                        //Submit changes to db
                        newPaymentObj.SubmitChanges();
                    }



                }
            }
            catch (Exception uploadError)
            {
                MessageBox.Show("An error occured while uploading the file.\n" + uploadError.Message, "Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //log error
            }

        }


        private string get_trans_ref_code(string val_date, string trans_date)
        {
            return "TR" + val_date.Replace("/", string.Empty) + trans_date.Replace("/", string.Empty);
        }

        public static bool upload_payment()
        {
            return true;
        }

        public static bool validate_company_code()
        {
            return true;
        }
    }
}
