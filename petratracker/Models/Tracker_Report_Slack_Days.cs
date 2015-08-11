﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petratracker.Models
{
    class Tracker_Report_Slack_Days
    {

        public string Company
        {
            get;
            set;
        }

        public string Tier
        {
            get;
            set;
        }

        public string Contribution_Month
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public TimeSpan Slack_Days
        {
            get;
            set;
        }


        public String Comments
        {
            get;
            set;
        }
    }
}
