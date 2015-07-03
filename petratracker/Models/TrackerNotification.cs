using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace petratracker.Models
{
    public class TrackerNotification
    {
        private static readonly TrackerDataContext _trackerDB = (App.Current as App).TrackerDBo;

        public TrackerNotification()
        {
        }

        public static IEnumerable<Notification> GetNotifications()
        {
            return (from n in _trackerDB.Notifications
                    where (n.status != "Expired") && (n.to_role_id == TrackerUser.GetCurrentUser().role_id)
                    orderby n.times_sent descending, n.updated_at descending, n.status descending 
                    select n);
        }

        public static Notification GetNotificationByJob(string notification_type, string job_type, int job_id)
        {
            try
            {
                return (from n in _trackerDB.Notifications
                        where (n.status != "Expired") && (n.notification_type == notification_type) &&
                        (n.job_id == job_id) && (n.job_type==job_type)
                        select n).Single();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void ExpireByJob(string notification_type, string job_type, int job_id)
        {
            try
            {
                var nf =  (from n in _trackerDB.Notifications
                          where (n.status != "Expired") && (n.notification_type == notification_type) &&
                           (n.job_id == job_id) && (n.job_type == job_type)
                           select n).Single();
                nf.status = "Expired";
                Save(nf);
            }
            catch (Exception e)
            {
                throw e;
            }    
        }
     
        public static string GetNotificationStatus()
        {
            var total_notifications = (from n in _trackerDB.Notifications
                                       where (n.status != "Expired") && (n.to_role_id == TrackerUser.GetCurrentUser().role_id)
                                       select n).Count();
            var new_notifications = (from n in _trackerDB.Notifications
                                     where (n.to_role_id == TrackerUser.GetCurrentUser().role_id) && (n.status == "New")
                                     select n
                                    ).Count();

            String status = "";
            if (total_notifications == 0)
            {
                status = "Notifications";
            }
            else
            {
                status = (new_notifications == 0) 
                       ? String.Format("{0} Notifications",  total_notifications.ToString())
                       : String.Format("{0} / {1} Notifications", new_notifications.ToString(), total_notifications.ToString()); 
            }
            return  status;
        }

        public static object GetNotificationToolTip()
        {
            var total_notifications = (from n in _trackerDB.Notifications
                                       where (n.status != "Expired") && (n.to_role_id == TrackerUser.GetCurrentUser().role_id)
                                       select n).Count();
            var new_notifications = (from n in _trackerDB.Notifications
                                     where (n.to_role_id == TrackerUser.GetCurrentUser().role_id) && (n.status == "New")
                                     select n
                                    ).Count();

            String tip = "";
            if (total_notifications == 0)
            {
                tip = " 0 Notifications";
            }
            else
            {
                tip = String.Format("{0} New Notifications\n{1} Total Notifications", new_notifications.ToString(), total_notifications.ToString());
            }
            return tip;
        }

        public static void Save(Notification nf)
        {
            try
            {
                nf.modified_by = TrackerUser.GetCurrentUser().id;
                nf.updated_at = DateTime.Now;
                _trackerDB.SubmitChanges();
            } catch(Exception e)
            {
                throw e;
            }
        }

        public static void Add(int role_id, string notification_type, string job_type, int jobid)
        {
            try 
            {
                Notification n = new Notification();
                n.to_role_id = role_id;
                n.from_user_id = TrackerUser.GetCurrentUser().id;
                n.notification_type = notification_type;
                n.job_type = job_type;
                n.job_id = jobid;
                n.times_sent = 1;
                n.last_sent = DateTime.Now;
                n.status = "New";
                n.modified_by = TrackerUser.GetCurrentUser().id;
                n.created_at = DateTime.Now;
                n.updated_at = DateTime.Now;
                _trackerDB.Notifications.InsertOnSubmit(n);
                _trackerDB.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

    
    }
}
