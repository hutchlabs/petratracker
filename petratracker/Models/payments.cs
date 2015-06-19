using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace petratracker.Models
{
    public class payments
    {


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
            catch(Exception errMsg)
            {
                MessageBox.Show(errMsg.Message);
                //log error
            }
            return dt;
        }


        public void read_microgen_data(string doc_source)
        {
            try
            {
                string fullPathToExcel = doc_source;
                string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=yes'", fullPathToExcel);
                DataTable dt = GetDataTable("SELECT * from [Payments Sheet$]", connString);

                foreach (DataRow dr in dt.Rows)
                {

                    if(true)
                    {
                        string insert_cmd = "";
                        dr["Transaction References"].ToString();
                        dr["Transaction Detail"].ToString();
                        dr["Statement Amount"].ToString();
                        dr["Contribution Date"].ToString();
                        dr["Statement Value Date"].ToString();
                        dr["Subscription Amount"].ToString();
                        dr["Subscription Value Date"].ToString();
                        dr["Comments"].ToString();
                    }
                   


                }
            }
            catch(Exception)
            {
                MessageBox.Show("An error occured while uploading the file.","Upload Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                //log error
            }

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
