using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petratracker.Models
{
    public partial class Schedule
    {
        public string period { get { return string.Format("{0}{1}", this.year, this.month); } set { } }
        public string ownername { get { return string.Format("{0} {1}", this.User.first_name, this.User.last_name); } set { } } 
    }
}
