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
        private User currentUser;
        private TrackerDataContext trackerDB = (App.Current as App).TrackerDBo;

        public TrackerNotification()
        {
            currentUser = trackerDB.Users.Single(p => p.username == Properties.Settings.Default.username);
        }

        public IEnumerable<Notification> GetNotifications()
        {
            return (from n in trackerDB.Notifications
                    where (n.status != "Expired") && (n.to_role_id == this.currentUser.role_id)
                    orderby n.status descending 
                    select n);
        }

        public string GetNotificationStatus()
        {
            var total_notifications = (from n in trackerDB.Notifications
                                       where (n.status != "Expired") && (n.to_role_id == this.currentUser.role_id)
                                       select n).Count();
            var new_notifications = (from n in trackerDB.Notifications
                                     where (n.to_role_id == this.currentUser.role_id) && (n.status == "New")
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

        public void Add(int role_id, string notification_type, string job_type, int jobid)
        {
            try 
            {
                Notification n = new Notification();
                n.to_role_id = role_id;
                n.from_user_id = currentUser.id;
                n.notification_type = notification_type;
                n.job_type = job_type;
                n.job_id = jobid;
                n.status = "New";
                n.modified_by = currentUser.id;
                n.created_at = new DateTime();
                n.updated_at = new DateTime();
                trackerDB.Notifications.InsertOnSubmit(n);
                trackerDB.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        internal object GetNotificationToolTip()
        {
            var total_notifications = (from n in trackerDB.Notifications
                                       where (n.status != "Expired") && (n.to_role_id == this.currentUser.role_id)
                                       select n).Count();
            var new_notifications = (from n in trackerDB.Notifications
                                     where (n.to_role_id == this.currentUser.role_id) && (n.status == "New")
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
    }
}
