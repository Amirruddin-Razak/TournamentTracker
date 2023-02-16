using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace TrackerLibrary
{
    public static class EmailLogic
    {
        public static void SendEmail(string to, string subject, string body)
        {
            SendEmail(new List<string>{ to }, new List<string>(), subject, body);
        }

        public static void SendEmail(List<string> to, List<string> bcc, string subject, string body) 
        {
            if (to.Count == 0 && bcc.Count == 0)
            {
                return;
            }

            MailAddress fromMailAddress = 
                new MailAddress(GlobalConfig.AppKeyLookup("senderEmail"), GlobalConfig.AppKeyLookup("senderDisplayName"));

            MailMessage mail = new MailMessage();
            to.ForEach(x => mail.To.Add(x));
            bcc.ForEach(x => mail.Bcc.Add(x));

            mail.From = fromMailAddress;
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Port = 25;
            client.EnableSsl = false;
            client.Host = "127.0.0.1";
            client.UseDefaultCredentials = true;
            client.Send(mail);
        }
    }
}
