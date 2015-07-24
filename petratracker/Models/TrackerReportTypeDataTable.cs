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

namespace petratracker.Models
{
    class TrackerReportTypeDataTable : DataTable
    {

        public TrackerReportTypeDataTable()
        {
            Columns.Add(new DataColumn("Company", typeof(string))); Columns["Company"].DefaultValue = string.Empty;
            Columns.Add(new DataColumn("Tier", typeof(string))); Columns["Tier"].DefaultValue = string.Empty;
            Columns.Add(new DataColumn("Contribution_Month", typeof(string))); Columns["Contribution_Month"].DefaultValue = string.Empty;
            Columns.Add(new DataColumn("Status", typeof(string))); Columns["Status"].DefaultValue = string.Empty;                      
        }

    }
}
