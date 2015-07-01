using petratracker.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
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

        public static IEnumerable<cclv_AllEntity> GetCompanies()
        {
            MicrogenDataContext microgenDB = (App.Current as App).MicrogenDBo;

            return (from c in microgenDB.cclv_AllEntities
                     where c.EntityTypeDesc == "Company"
                     orderby c.FullName 
                     select c);
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
