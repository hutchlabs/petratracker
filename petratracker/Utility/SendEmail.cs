using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using petratracker.Models;
using System.Net.Configuration;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;


namespace petratracker.Utility
{
    class SendEmail
    {
        #region Private Members

        private static readonly System.Net.Mail.SmtpClient _smtp;
        private static readonly System.Net.Mail.MailAddress _from;
        private static Regex _validEmailRegex = CreateValidEmailRegex();

        #endregion

        #region Constructors

        static SendEmail()
        {
            Setting smpthost = Database.Tracker.Settings.Single(s => s.setting1 == Constants.SETTINGS_EMAIL_SMTP_HOST);
            Setting fromemail = Database.Tracker.Settings.Single(s => s.setting1 == Constants.SETTINGS_EMAIL_FROM);

            _smtp = new System.Net.Mail.SmtpClient(smpthost.value);
            _from = new System.Net.Mail.MailAddress(fromemail.value);
        }

        public SendEmail()
        {
        }

        #endregion

        #region Public Methods

        public static bool IsValidEmail(string addr)
        {
            try
            {
                return _validEmailRegex.IsMatch(addr);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidSMTP(string addr)
        {
            return SmtpHelper.TestConnection(addr, 25);
        }

        public static bool sendNewUserMail(string name, string email, string password)
        {
            Setting tmpl = Database.Tracker.Settings.Single(s => s.setting1 == "tmpl_newuser_email");

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
            Setting tmpl = Database.Tracker.Settings.Single(s => s.setting1 == "tmpl_resetpass_email");

            string adminEmail = TrackerUser.GetAdminEmail();

            string msg = tmpl.value;
            msg.Replace("<username>", email);

            return sendMail(adminEmail, "[Tracker] Reset Password for "+email, msg);
        }

        #endregion

        #region Private Methods

        private static bool sendMail(string to, string subject, string msg)
        {
            return true;
            /*
            bool _isSent = false;

            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.To.Add(to,);
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


        /// <summary>
        /// Taken from http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx
        /// </summary>
        /// <returns></returns>
        private static Regex CreateValidEmailRegex()
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
        }
        
        #endregion
    }

    public static class SmtpHelper
    {
    
        /// <summary>
        /// test the smtp connection by sending a HELO command
        /// </summary>
        /// <param name="smtpServerAddress"></param>
        /// <param name="port"></param>
        public static bool TestConnection(string smtpServerAddress, int port)
        {
            try
            {
                IPAddress[] hostEntry = Dns.GetHostAddresses(smtpServerAddress);
                IPEndPoint endPoint = new IPEndPoint(hostEntry.ElementAt(0), port);
                using (Socket tcpSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
                {
                    //try to connect and test the rsponse for code 220 = success
                    tcpSocket.Connect(endPoint);
                    if (!CheckResponse(tcpSocket, 220))
                    {
                        return false;
                    }

                    // send HELO and test the response for code 250 = proper response
                    SendData(tcpSocket, string.Format("HELO {0}\r\n", Dns.GetHostName()));
                    if (!CheckResponse(tcpSocket, 250))
                    {
                        return false;
                    }

                    // if we got here it's that we can connect to the smtp server
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private static void SendData(Socket socket, string data)
        {
            byte[] dataArray = Encoding.ASCII.GetBytes(data);
            socket.Send(dataArray, 0, dataArray.Length, SocketFlags.None);
        }

        private static bool CheckResponse(Socket socket, int expectedCode)
        {
            while (socket.Available == 0)
            {
                System.Threading.Thread.Sleep(100);
            }
            byte[] responseArray = new byte[1024];
            socket.Receive(responseArray, 0, socket.Available, SocketFlags.None);
            string responseData = Encoding.ASCII.GetString(responseArray);
            int responseCode = Convert.ToInt32(responseData.Substring(0, 3));
            if (responseCode == expectedCode)
            {
                return true;
            }
            return false;
        }
    }

}

