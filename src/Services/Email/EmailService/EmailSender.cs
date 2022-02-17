using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
   public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailSender(EmailConfiguration emailConfig)
        {
            this._emailConfig = emailConfig;
        }

        public void sendEmail(Message message)
        {
            using(MailMessage mail = new MailMessage())
            {
                Console.WriteLine("WWW " + _emailConfig.From);

                mail.From = new MailAddress(_emailConfig.From);
                mail.To.Add(message.To);
                mail.Subject = message.Subject;
                mail.IsBodyHtml = true;
                mail.Body = message.Content;

                using (SmtpClient smtp = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.Port))
                {
                    smtp.Credentials = new NetworkCredential(_emailConfig.Username, _emailConfig.Password);//Real email and password
                    smtp.EnableSsl = true;
                    smtp.Send(mail);

                }
            }
        }
    }
}
