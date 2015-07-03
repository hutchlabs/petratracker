using petratracker.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace petratracker.Code
{
    public class ComboBoxPairs
    {
        public string _Key { get; set; }
        public string _Value { get; set; }

        public ComboBoxPairs(string _key, string _value)
        {
            _Key = _key;
            _Value = _value;
        }
    }

    public class Utils
    {
        public static async Task DoPeriodicWorkAsync(Delegate todoTask, TimeSpan dueTime, TimeSpan interval, CancellationToken token)
        {
            // Initial wait time before we begin the periodic loop.
            if (dueTime > TimeSpan.Zero)
                await Task.Delay(dueTime, token);

            // Repeat this loop until cancelled.
            while (!token.IsCancellationRequested)
            {
                // Update Task
                todoTask.DynamicInvoke();

                // Wait to repeat again.
                if (interval > TimeSpan.Zero)
                    await Task.Delay(interval, token);
            }
        }


        public static string GetCompanyEmail(string company_id)
        {
            try
            {
                MicrogenDataContext microgenDB = (App.Current as App).MicrogenDBo;

                /*var c = (from c in microgenDB.cclv_AllEntities
                         where c.EntityID == int.Parse(company_id)
                         select c).Single();*/
                return "no-reply@petratrust.com";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static IEnumerable<cclv_AllEntity> GetCompanies()
        {
            try
            {
                MicrogenDataContext microgenDB = (App.Current as App).MicrogenDBo;

                return (from c in microgenDB.cclv_AllEntities
                        where c.EntityTypeDesc == "Company"  && c.FullName.ToLower() != "available" &&
                              c.FullName != "Available Company"
                        orderby c.FullName
                        select c);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static DataTable GetDataTable(string sql, string connectionString)
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
                throw(errMsg);
            }

            return dt;
        }
    }
}
