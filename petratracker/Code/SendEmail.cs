using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using petratracker.Models;

namespace petratracker.Code
{
    class SendEmail
    {
        #region Private Members
        
        private static readonly string _adminEmail;
        private static readonly System.Net.Mail.SmtpClient _smtp;
        private static readonly System.Net.Mail.MailAddress _from;
        private static readonly TrackerDataContext _trackerDB = (App.Current as App).TrackerDBo;

        #endregion

        #region Constructors

        static SendEmail()
        {
            Setting smpthost = _trackerDB.Settings.Single(s => s.setting1 == "smtp_host");
            Setting fromemail = _trackerDB.Settings.Single(s => s.setting1 == "email_from");
            Setting adminemail = _trackerDB.Settings.Single(s => s.setting1 == "email_admin");

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
            Setting tmpl = _trackerDB.Settings.Single(s => s.setting1 == "tmpl_newuser_email");

            string msg = tmpl.value;
            msg.Replace("<username>", email);
            msg.Replace("<password>", password);
            msg.Replace("<name>", name);

            return sendMail(email, "[Tracker] Welcome to Petra Tracker", msg);
        }

        public static bool sendFixErrorMail(string to, string msg)
        {
            //Setting tmpl = _trackerDB.Settings.Single(s => s.setting1 == "tmpl_newuser_email");

            // TODO: full form template
            return sendMail(to, "[Petra Trust] Reminder to fix Errors", msg);
        }
        
        public static bool sendResetPasswordMail(string email)
        {
            Setting tmpl = _trackerDB.Settings.Single(s => s.setting1 == "tmpl_resetpass_email");

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

