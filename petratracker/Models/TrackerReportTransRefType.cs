using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petratracker.Models
{
    class TrackerReportTransRefType
    {

        public string transaction_ref_no
        {
            get;
            set;
        }

        public string company_code
        {
            get;
            set;
        }

        public string tier
        {
            get;
            set;
        }

        public decimal transaction_amount
        {
            get;
            set;
        }

        public DateTime transaction_date
        {
            get;
            set;
        }

        public DateTime value_date
        {
            get;
            set;
        }

        public string EntityKey
        {
            get;
            set;
        }

        public string OrderReference
        {
            get;
            set;
        }

        public decimal? NAVPrice
        {
            get;
            set;
        }

        public decimal? Group1Units
        {
            get;
            set;
        }

        public int DealID
        {
            get;
            set;
        }

    }
}
