using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace ESB.Utils.Email
{
    public static class EmailHelper
    {
        public static bool IsValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }
        public static SendEmailResponse SendEmail(this EmailSettings emailSettings, EmailMessage emailMessage)
        {
            SendEmailResponse response = new SendEmailResponse();
            try
            {

                SmtpClient client = new SmtpClient();
                MailMessage mail = new MailMessage();

                client.Host = emailSettings.Host;
                client.Port = emailSettings.Port;

                string user = emailSettings.User;
                string password = emailSettings.Password;
                client.Credentials = new System.Net.NetworkCredential(user, password);

                client.EnableSsl = emailSettings.EnableSsl;

                if (string.IsNullOrEmpty(emailSettings.FromEmailAddress))
                {
                    throw new Exception("Please specify the from email address");
                }
                MailAddress fromAddress = new MailAddress(emailSettings.FromEmailAddress, emailSettings.FromEmailName);
                mail.From = fromAddress;

                mail.BodyEncoding = emailMessage.BodyEncoding;

                if (string.IsNullOrEmpty(emailMessage.To))
                {
                    throw new Exception("To address is empty");
                }
                mail.To.Add(emailMessage.To);

                if (!string.IsNullOrEmpty(emailMessage.Cc))
                {
                    mail.CC.Add(emailMessage.Cc);
                }
                if (!string.IsNullOrEmpty(emailMessage.Bcc))
                {
                    mail.Bcc.Add(emailMessage.Bcc);
                }
                mail.Subject = emailMessage.Subject;
                mail.Body = emailMessage.Body;
                mail.IsBodyHtml = emailMessage.IsBodyHtml;
                
                try
                {
                    client.Send(mail);
                    response.EmailSent = true;
                }
                catch (Exception ex)
                {
                    response.Response = ex.Message;
                }
            }
            catch(Exception ex)
            { 
            }

            return response;
        }
    }
}
