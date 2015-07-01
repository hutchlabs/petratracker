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
        private bool isSent = false;

        private string adminEmail;
        private System.Net.Mail.SmtpClient smtp;
        private System.Net.Mail.MailAddress from;
        private TrackerDataContext trackerDB = (App.Current as App).TrackerDBo;

        public SendEmail()
        {
            Setting smpthost = trackerDB.Settings.Single(s => s.setting1 == "smtp_host");
            Setting fromemail = trackerDB.Settings.Single(s => s.setting1 == "email_from");
            Setting adminemail = trackerDB.Settings.Single(s => s.setting1 == "email_admin");

            smtp = new System.Net.Mail.SmtpClient(smpthost.value);
            from = new System.Net.Mail.MailAddress(fromemail.value);

            adminEmail = adminemail.value;
        }

        public bool sendNewUserMail(string name, string email, string password)
        {
            Setting tmpl = trackerDB.Settings.Single(s => s.setting1 == "tmpl_newuser_email");

            string msg = tmpl.value;
            msg.Replace("<username>", email);
            msg.Replace("<password>", password);
            msg.Replace("<name>", name);

            return this.sendMail(email, "[Tracker] Welcome to Petra Tracker", msg);
        }
        
        public bool sendResetPasswordMail(string email)
        {
            Setting tmpl = trackerDB.Settings.Single(s => s.setting1 == "tmpl_resetpass_email");

            string msg = tmpl.value;
            msg.Replace("<username>", email);

            return this.sendMail(adminEmail, "[Tracker] Reset Password for "+email, msg);
        }

        private bool sendMail(string to, string subject, string msg)
        {
            return true;

            /*try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.To.Add(to);
                message.From = this.from;
                message.Subject = subject;
                message.Body = msg;
                smtp.Send(message);
                isSent = true;
            }
            catch (Exception mailError)
            {
                System.Windows.MessageBox.Show(mailError.Message);
            }

            return isSent;
             * */
        }

    }
}

