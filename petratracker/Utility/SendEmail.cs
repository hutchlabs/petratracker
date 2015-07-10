using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using petratracker.Models;

namespace petratracker.Utility
{
    class SendEmail
    {
        #region Private Members
        
        private static readonly string _adminEmail;
        private static readonly System.Net.Mail.SmtpClient _smtp;
        private static readonly System.Net.Mail.MailAddress _from;

        #endregion

        #region Constructors

        static SendEmail()
        {
            Setting smpthost = TrackerDB.Tracker.Settings.Single(s => s.setting1 == "email_smtp_host");
            Setting fromemail = TrackerDB.Tracker.Settings.Single(s => s.setting1 == "email_from");
            Setting adminemail = TrackerDB.Tracker.Settings.Single(s => s.setting1 == "email_admin");

            _smtp = new System.Net.Mail.SmtpClient(smpthost.value);
            _from = new System.Net.Mail.MailAddress(fromemail.value);
            _adminEmail = adminemail.value;
        }

        public SendEmail()
        {
        }

        #endregion

        #region Public Methods

        public static bool sendNewUserMail(string name, string email, string password)
        {
            Setting tmpl = TrackerDB.Tracker.Settings.Single(s => s.setting1 == "tmpl_newuser_email");

            string msg = tmpl.value;
            msg.Replace("<username>", email);
            msg.Replace("<password>", password);
            msg.Replace("<name>", name);

            return sendMail(email, "[Tracker] Welcome to Petra Tracker", msg);
        }

        public static bool sendFixErrorMail(string to, string msg)
        {
            //Setting tmpl = TrackerDB.Tracker.Settings.Single(s => s.setting1 == "tmpl_newuser_email");

            // TODO: full form template
            return sendMail(to, "[Petra Trust] Reminder to fix Errors", msg);
        }
        
        public static bool sendResetPasswordMail(string email)
        {
            Setting tmpl = TrackerDB.Tracker.Settings.Single(s => s.setting1 == "tmpl_resetpass_email");

            string msg = tmpl.value;
            msg.Replace("<username>", email);

            return sendMail(_adminEmail, "[Tracker] Reset Password for "+email, msg);
        }

        #endregion

        #region Private Methods

        private static bool sendMail(string to, string subject, string msg)
        {
            return true;

            /*try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.To.Add(to);
                message.From = _from;
                message.Subject = subject;
                message.Body = msg;
                _smtp.Send(message);
                _isSent = true;
            }
            catch (Exception mailError)
            {
                System.Windows.MessageBox.Show(mailError.Message);
            }

            return _isSent;
             * */
        }

        #endregion
    }
}

