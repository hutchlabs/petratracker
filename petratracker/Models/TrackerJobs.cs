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

        private static TrackerDataContext _trackerDB  = (App.Current as App).TrackerDBo;

        #endregion

        #region Constructors
        
        public TrackerJobs()
        { }

        #endregion

        #region Public Jobs Methods

        public static int Add(string job_type, string deal_description)
        {
            try
            {
                Job j = new Job();
                j.job_type = job_type;
                j.job_description = deal_description;
                j.status = "In Progress";
                j.owner = TrackerUser.GetCurrentUser().id;
                j.created_at = DateTime.Now;
                j.updated_at = DateTime.Now;
                _trackerDB.Jobs.InsertOnSubmit(j);
                _trackerDB.SubmitChanges();

                return j.id;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

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