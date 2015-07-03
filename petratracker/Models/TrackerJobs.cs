using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using petratracker.Code;

namespace petratracker.Models
{
    class TrackerJobs
    {
        #region Private Members

        private static TrackerDataContext _trackerDB;

        #endregion

        #region Constructors

        static TrackerJobs()
        {
            _trackerDB = (App.Current as App).TrackerDBo;
        }
        
        public TrackerJobs()
        { }

        #endregion

        #region Public Jobs Methods

        public static IEnumerable<Job> GetJobs(string type="")
        {
            if (type == string.Empty)
            {
               return (from j in _trackerDB.Jobs select j);
            }
            else
            {
                return (from j in _trackerDB.Jobs where j.job_type == type select j);
            }
        }

        #endregion
    }
}