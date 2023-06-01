using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net;
using System.Net.Mail;

namespace GuestTrack
{
    public class EmailSender
    {
        private string smtpServer;
        private int smtpPort;
        private string smtpUsername;
        private string smtpPassword;

        public EmailSender(string server, int port, string username, string password)
        {
            smtpServer = server;
            smtpPort = port;
            smtpUsername = username;
            smtpPassword = password;
        }

        public void SendEmail(string from, string to, string subject, string body)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                    using (MailMessage mailMessage = new MailMessage(from, to))
                    {
                        mailMessage.Subject = subject;
                        mailMessage.Body = body;
                        mailMessage.IsBodyHtml = true;

                        smtpClient.Send(mailMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions or log the error
                Console.WriteLine("Error sending email: " + ex.Message);
            }
        }
    }

}
