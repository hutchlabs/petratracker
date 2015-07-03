﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using petratracker.Code;

namespace petratracker.Models
{
    public class TrackerEmail
    {
        #region Private Members
        
        private static readonly TrackerDataContext _trackerDB = (App.Current as App).TrackerDBo;
        
        #endregion

        #region Constructor
        public TrackerEmail()
        {
        }
        #endregion

        #region Public Email Methods
       
        public static IEnumerable<Email> GetEmails()
        {
            return (from n in _trackerDB.Emails
                    orderby n.created_at descending, n.updated_at descending
                    select n);
        }

        public static IEnumerable<Email> GetEmailsByJob(string email_type, string job_type, int job_id)
        {
            try
            {
                return (from n in _trackerDB.Emails
                        where  (n.email_type == email_type) &&
                        (n.job_id == job_id) && (n.job_type == job_type)
                        orderby n.created_at ascending
                        select n);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void Add(string addr, string uid, string msg, string email_type, string job_type, int job_id)
        {
            try
            {
                Email em = new Email();
                em.sent_to = addr;
                em.sent_to_id = uid;
                em.email_text = string.Format("Dear Sir/Madam\nPlease fix this problem: {0}", msg);
                em.email_type = email_type;
                em.job_type = job_type;
                em.job_id = job_id;
                em.modified_by = TrackerUser.GetCurrentUser().id;
                em.created_at = DateTime.Now;
                em.updated_at = DateTime.Now;
                _trackerDB.Emails.InsertOnSubmit(em);
                _trackerDB.SubmitChanges();

                SendEmail.sendFixErrorMail(addr, em.email_text);
            } 
            catch(Exception e) 
            {
                throw e;
            }
        }
        #endregion


    }

}
