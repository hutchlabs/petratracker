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
    class MicroGenExportDataTable : DataTable
    {
        public MicroGenExportDataTable()
        {

            Columns.Add(new DataColumn("FundCode", typeof(string))); Columns["FundCode"].DefaultValue = string.Empty;
            Columns.Add(new DataColumn("FundHolderCode", typeof(string))); Columns["FundHolderCode"].DefaultValue = string.Empty;           
            Columns.Add(new DataColumn("HolderAccDesig", typeof(string))); Columns["HolderAccDesig"].DefaultValue = string.Empty;            
            Columns.Add(new DataColumn("TransTypeDesc", typeof(string))); Columns["TransTypeDesc"].DefaultValue = "Issue";           
            Columns.Add(new DataColumn("TransDirection", typeof(string))); Columns["TransDirection"].DefaultValue = "In";           
            Columns.Add(new DataColumn("ProductCode", typeof(string))); Columns["ProductCode"].DefaultValue = "Long Term Savings";           
            Columns.Add(new DataColumn("WrapperCode", typeof(string))); Columns["WrapperCode"].DefaultValue = string.Empty;
            Columns.Add(new DataColumn("TransReference", typeof(string))); Columns["TransReference"].DefaultValue = string.Empty;
            Columns.Add(new DataColumn("TransUnitsGrp1", typeof(string))); Columns["TransUnitsGrp1"].DefaultValue = string.Empty;
            Columns.Add(new DataColumn("TransUnitsGrp2", typeof(string))); Columns["TransUnitsGrp2"].DefaultValue = string.Empty;
            Columns.Add(new DataColumn("NAVPrice", typeof(string))); Columns["NAVPrice"].DefaultValue = "1";
            Columns.Add(new DataColumn("QuotedPrice", typeof(string))); Columns["QuotedPrice"].DefaultValue = "1";
            Columns.Add(new DataColumn("DealingPrice", typeof(string))); Columns["DealingPrice"].DefaultValue = "1";
            Columns.Add(new DataColumn("DealCcyCode", typeof(string))); Columns["DealCcyCode"].DefaultValue = "GHS";
            Columns.Add(new DataColumn("DealCcyPayAmnt", typeof(string))); Columns["DealCcyPayAmnt"].DefaultValue = string.Empty;
            Columns.Add(new DataColumn("DealCcyDealAmnt", typeof(string))); Columns["DealCcyDealAmnt"].DefaultValue = string.Empty;
            Columns.Add(new DataColumn("PayCcyCode", typeof(string))); Columns["PayCcyCode"].DefaultValue = "GHS";
            Columns.Add(new DataColumn("PayCcyPayAmnt", typeof(string))); Columns["PayCcyPayAmnt"].DefaultValue = string.Empty;
            Columns.Add(new DataColumn("PayCcyDealAmnt", typeof(string))); Columns["PayCcyDealAmnt"].DefaultValue = string.Empty;
            Columns.Add(new DataColumn("ExchangeRate", typeof(string))); Columns["ExchangeRate"].DefaultValue = "1";
            Columns.Add(new DataColumn("DealDate", typeof(string))); Columns["DealDate"].DefaultValue = string.Empty;
            Columns.Add(new DataColumn("ValueDate", typeof(string))); Columns["ValueDate"].DefaultValue = string.Empty;
            Columns.Add(new DataColumn("BookDate", typeof(string))); Columns["BookDate"].DefaultValue = string.Empty;
            Columns.Add(new DataColumn("PriceDate", typeof(string))); Columns["PriceDate"].DefaultValue = string.Empty;
            Columns.Add(new DataColumn("FEFRate", typeof(string))); Columns["FEFRate"].DefaultValue = "0";
            Columns.Add(new DataColumn("FEFDealCcy", typeof(string))); Columns["FEFDealCcy"].DefaultValue = "0";
            Columns.Add(new DataColumn("FEFPayCcy", typeof(string))); Columns["FEFPayCcy"].DefaultValue = "0";
            Columns.Add(new DataColumn("DiscRate", typeof(string))); Columns["DiscRate"].DefaultValue = "0";
            Columns.Add(new DataColumn("DiscPayCcy", typeof(string))); Columns["DiscPayCcy"].DefaultValue = "0";
            Columns.Add(new DataColumn("DiscDealCcy", typeof(string))); Columns["DiscDealCcy"].DefaultValue = "0";
            Columns.Add(new DataColumn("ExitFeeRate", typeof(string))); Columns["ExitFeeRate"].DefaultValue = "0";
            Columns.Add(new DataColumn("ExitFeeDealCcy", typeof(string))); Columns["ExitFeeDealCcy"].DefaultValue = "0";
            Columns.Add(new DataColumn("ExitFeePayCcy", typeof(string))); Columns["ExitFeePayCcy"].DefaultValue = "0";
            Columns.Add(new DataColumn("SettlementBasis", typeof(string))); Columns["SettlementBasis"].DefaultValue = "N";
            Columns.Add(new DataColumn("DealBasis", typeof(string))); Columns["DealBasis"].DefaultValue = "A";
           
        }
    }
}
