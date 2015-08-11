using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petratracker.Models
{
    public  class TrackerPaymentDealDescriptions
    {
        public int id
        {
            get;
            set;
        }

        public string month
        {
            get;
            set;
        }

        public string year
        {
            get;
            set;
        }

        public string contribution_type
        {
            get;
            set;
        }

    }
}
