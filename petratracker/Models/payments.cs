using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;

namespace petratracker.Models
{
    class Payments
    {
        
     
        public void read_excel_office(string fileLocation)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application xlApp;
                Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
                Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(fileLocation, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                MessageBox.Show(xlWorkSheet.get_Range("A1", "A1").Value2.ToString());

                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
            }
            catch(Exception readError)
            {
                MessageBox.Show(readError.Message);
            }
        }

         private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }


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


         public void read_excel_data(string doc_source)
         {
             try
             {
                 string fullPathToExcel = doc_source;
                 string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=yes'", fullPathToExcel);
                 DataTable dt = GetDataTable("SELECT * from [Payments Sheet$]", connString);

                 foreach (DataRow dr in dt.Rows)
                 {

                     if (true)
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
             catch (Exception)
             {
                 MessageBox.Show("An error occured while uploading the file.", "Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 //log error
             }

         }


        



    }


    }

