using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petratracker.Code
{
    class SendEmail
    {
        bool isSent = false;

        public bool sendMail(string from,string to,string msg)
        {
            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.To.Add(to);
                message.Subject = "Your user credentails from PetraTracker.";
                message.From = new System.Net.Mail.MailAddress(from);
                message.Body = msg;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("mail.cdhgroup.co");
                smtp.Send(message);
                isSent = true;
            }
            catch(Exception mailError)
            {
                System.Windows.MessageBox.Show(mailError.Message);
            }
            return isSent;
        }

    }
}
