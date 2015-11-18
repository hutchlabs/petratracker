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
    class MicroGenExportData 
    {

            public MicroGenExportData()
            {
            }
            //Craete DataTable columns

            public string FundCode
            {
                get;
                set;
            }

            public string FundHolderCode
            {
                get;
                set;
            } 

            public string HolderAccDesig
            {
                get;
                set;
            }

            public string TransTypeDesc
            {
                get;
                set;
            }

            public string TransDirection
            {
                get;
                set;
            }

            public string ProductCode
            {
                get;
                set;
            }

            public string WrapperCode
            {
                get;
                set;
            }
            
            public string TransReference
            {
                get;
                set;
            }

            public string TransUnitsGrp1
            {
                get;
                set;
            }
            
            public string TransUnitsGrp2
            {
                get;
                set;
            }
           
            public string NAVPrice
            {
                get;
                set;
            }
           
            public string QuotedPrice
            {
                get;
                set;
            }

            public string DealingPrice
            {
                get;
                set;
            }
           
            public string DealCcyCode
            {
                get;
                set;
            }

            public string DealCcyPayAmnt
            {
                get;
                set;
            }

            public string DealCcyDealAmnt
            {
                get;
                set;
            }
           
            public string PayCcyCode
            {
                get;
                set;
            }
         
            public string PayCcyPayAmnt
            {
                get;
                set;
            }
            
            public string PayCcyDealAmnt
            {
                get;
                set;
            }
       
            public string ExchangeRate
            {
                get;
                set;
            }
         
            public string DealDate
            {
                get;
                set;
            }

            public string ValueDate
            {
                get;
                set;
            }

            public string BookDate
            {
                get;
                set;
            }

            public string PriceDate
            {
                get;
                set;
            }

            public string FEFRate
            {
                get;
                set;
            }
           
            public string FEFDealCcy
            {
                get;
                set;
            }

            public string FEFPayCcy
            {
                get;
                set;
            }
            
            public string DiscRate
            {
                get;
                set;
            }

            public string DiscDealCcy
            {
                get;
                set;
            }

            public string DiscPayCcy
            {
                get;
                set;
            }
         
            public string ExitFeeRate
            {
                get;
                set;
            }
                
            public string ExitFeeDealCcy
            {
                get;
                set;
            }
            
            public string ExitFeePayCcy
            {
                get;
                set;
            }
            
            public string SettlementBasis
            {
                get;
                set;
            }
            
            public string DealBasis
            {
                get;
                set;
            }

        
    }

    public class MicroGenFundDealExportData
    {
        public string Transaction_Ref
        {
            get;
            set;
        }
    }


    public class PaymentsView
    {
        public string Transaction_Ref
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public string Trans_Details
        {
            get;
            set;
        }

        public string Subscription_Value_Date
        {
            get;
            set;
        }

        public string Subscription_Amount
        {
            get;
            set;
        }

        public string Company_Name
        {
            get;
            set;
        }

        public string Company_Code
        {
            get;
            set;
        }

        public string Tier
        {
            get;
            set;
        }

        public string Deal_Description
        {
            get;
            set;
        }

        public string Approved_By
        {
            get;
            set;
        }
    }
}

