using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using petratracker.Code;
using System.Data;

namespace petratracker.Models
{

    public class SchedulesItemData
    {
        public int id { get; set; }
        public string name { get; set; }
        public SchedulesItem item { get; set; }
    }

    public class TrackerSchedule
    {
        private User currentUser;
        private TrackerDataContext trackerDB = (App.Current as App).TrackerDBo;
        private PTASDataContext ptasDB = (App.Current as App).PTASDBo;

        public TrackerSchedule()
        {
            currentUser = trackerDB.Users.Single(p => p.username == Properties.Settings.Default.username);
        }

        public Schedule GetSchedule(int id)
        {
           return (from j in trackerDB.Schedules where j.id == id select j).Single();
        }

        public IEnumerable<Schedule> GetSchedules()
        {
            return (from j in trackerDB.Schedules
                    orderby j.created_at descending
                    select j);
        }

        public IEnumerable<SchedulesItemData> GetSchedulesItems(int schedule_id)
        {
            return (from j in trackerDB.SchedulesItems
                    join u in trackerDB.Users on j.modified_by equals u.id
                    where j.schedule_id == schedule_id
                    orderby j.created_at descending
                    select new SchedulesItemData { id = j.id, item = j, name = string.Format("{0} {1}", u.first_name, u.last_name) });       
        }

        public IEnumerable<ComboBoxPairs> GetYears()
        {
            int year = DateTime.Now.Year;
            List<ComboBoxPairs> cbp = new List<ComboBoxPairs>();
            cbp.Add(new ComboBoxPairs((year - 1).ToString(), (year - 1).ToString()));
            cbp.Add(new ComboBoxPairs(year.ToString(), year.ToString()));
            cbp.Add(new ComboBoxPairs((year + 1).ToString(), (year + 1).ToString()));

            return cbp.AsEnumerable();
        }

        public IEnumerable<ComboBoxPairs> GetCompanies()
        {
            var companies = Utils.GetCompanies();
            List<ComboBoxPairs> cbpc = new List<ComboBoxPairs>();

            foreach (var c in companies)
            {
                cbpc.Add(new ComboBoxPairs(c.EntityID.ToString(), c.FullName));
            }

            return cbpc.AsEnumerable();
        }

        public IEnumerable<ComboBoxPairs> GetContributionTypes(string company="")
        {
            List<ComboBoxPairs> cbp = new List<ComboBoxPairs>();
            IQueryable<ContributionType> cts = null;

            if (company==string.Empty)
            {
                cts = (from j in ptasDB.ContributionTypes select j);
            } else {
                cts = (from j in ptasDB.ContributionTypes
                       join f in ptasDB.FundDeals on j.ContribTypeID equals f.ContribType_ID 
                      where f.CompanyEntityId == company
                     select j);
            }

            foreach (var ct in cts)
            {
                cbp.Add(new ComboBoxPairs(ct.ContribTypeID.ToString(), ct.Description));
            }

            return cbp.AsEnumerable();
        }
       
        internal void AddSchedule(string company, string companyid, string tier, string month, string year)
        {
            try
            {
                Schedule s = new Schedule();
                s.company = company;
                s.company_id = companyid;
                s.tier = tier;
                s.month = month;
                s.year = int.Parse(year);
                s.modified_by = currentUser.id;
                s.validation_status = "Not Validated";
                s.created_at = DateTime.Now;
                s.updated_at = DateTime.Now;
                trackerDB.Schedules.InsertOnSubmit(s);
                trackerDB.SubmitChanges();
            }
            catch (Exception e)
            {
                throw (e);
            }
        }
    }
}
