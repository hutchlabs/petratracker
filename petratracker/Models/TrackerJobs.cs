using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using petratracker.Utility;

namespace petratracker.Models
{
    class TrackerJobs
    {
        #region Private Members

        #endregion

        #region Constructors
        
        public TrackerJobs()
        { }

        #endregion

        #region Public Jobs Methods

        public static int Add(string job_type, string deal_description, string tier)
        {
            try
            {
                Job j = new Job();
                j.job_type = job_type;
                j.tier = tier;
                j.job_description = deal_description;
                j.status = Constants.PAYMENT_STATUS_INPROGRESS;
                j.owner = TrackerUser.GetCurrentUser().id;
                j.created_at = DateTime.Now;
                j.updated_at = DateTime.Now;
                Database.Tracker.Jobs.InsertOnSubmit(j);
                Database.Tracker.SubmitChanges();

                return j.id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Job GetJob(int jobid)
        {
            return (from j in Database.Tracker.Jobs where j.id==jobid select j).Single();

        }

        public static IEnumerable<Job> GetJobs(string type="", string status="")
        {
            if (type == string.Empty && status == string.Empty)
            {
                return (from j in Database.Tracker.Jobs orderby j.created_at descending select j);
            }
            else
            {
                if (status == string.Empty)
                {
                    return (from j in Database.Tracker.Jobs where j.job_type == type orderby j.created_at descending select j);
                }
                else
                {
                    return (from j in Database.Tracker.Jobs where j.job_type == type && j.status==status orderby j.created_at descending select j);
                }
            }
        }
  
        public static void Approve(Job job)
        {
            job.status = Constants.PAYMENT_STATUS_APPROVED;
            job.updated_at = DateTime.Now;
            Database.Tracker.SubmitChanges();
        }

        #endregion


    }
}