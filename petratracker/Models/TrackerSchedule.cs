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

        internal void AddScheduleFromFileUpload(string filename)
        {
            try
            {
                string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=yes'", filename);
                DataTable dt = Utils.GetDataTable("SELECT * from [Report$]", connString);

                Schedule s = new Schedule();
                s.company = "";
                s.tier = "";

                s.modified_by = currentUser.id;
                s.created_at = DateTime.Now;
                s.updated_at = DateTime.Now;
                trackerDB.Schedules.InsertOnSubmit(s);
                trackerDB.SubmitChanges();

                foreach (DataRow dr in dt.Rows)
                {
                    /*string trans_date_str = dr["Transaction Date"].ToString();
                    string value_date_str = dr["Value Date"].ToString();
                    char[] charSeparators = new char[] { '/' };
                    string[] value_date_res = value_date_str.Split(charSeparators);
                    string[] trans_date_res = trans_date_str.Split(charSeparators);

                    //Insert new payment
                    Payment objPayment = new Payment();
                    objPayment.transaction_ref_no = get_trans_ref_code(dr["Value Date"].ToString(), dr["Transaction Date"].ToString());
                    objPayment.job_id = objJob.id;
                    objPayment.transaction_details = dr["Transaction Details"].ToString();
                    DateTime trans_date = new DateTime(int.Parse(trans_date_res[2]), int.Parse(trans_date_res[1]), int.Parse(trans_date_res[0]));
                    DateTime value_date = new DateTime(int.Parse(value_date_res[2]), int.Parse(value_date_res[1]), int.Parse(value_date_res[0]));
                    objPayment.transaction_date = trans_date;
                    objPayment.value_date = value_date;
                    objPayment.transaction_amount = decimal.Parse(dr["Transaction Amount"].ToString());

                    if (dr["Dr / Cr Indicator"].ToString() == "Credit")
                    {
                        objPayment.status = "Pending";
                    }
                    else if (dr["Dr / Cr Indicator"].ToString() == "Debit")
                    {
                        objPayment.status = "Returned";
                    }

                    objPayment.owner = ini_user.id;
                    objPayment.created_at = DateTime.Now;
                    trackerDB.Payments.InsertOnSubmit(objPayment);

                    //Submit changes to db
                    trackerDB.SubmitChanges();
                     */
                }
            }
            catch (Exception uploadError)
            {
                throw (uploadError);
            }
        }

        internal void AddSchedule(string company, string tier, DateTime month)
        {
            try
            {

                Schedule s = new Schedule();
                s.company = company;
                s.tier = tier;
                s.month = month;
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
