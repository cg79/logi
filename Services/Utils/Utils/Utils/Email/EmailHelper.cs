using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;


namespace Utils
{
    public class EmailHelper
    {
       

        public static void SendEmail(string to, string subject, string body)
        {

            try
            {
                SmtpClient client = new SmtpClient();
                MailMessage mail = new MailMessage();

                client.Host = "smtp.gmail.com";
                client.Port = 587;

                string user = "claudiu9379@gmail.com";
                string password = "kqdduzoy9379!";
                client.Credentials = new System.Net.NetworkCredential(user, password);

                client.EnableSsl = true;

                MailAddress fromAddress = new MailAddress("noreply@musicweaver.com", "MusicWeaver");

                mail.BodyEncoding = System.Text.Encoding.UTF8;

                mail.To.Add(to);
                mail.Bcc.Add("claudiu9379@gmail.com");
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.From = fromAddress;
                try
                {
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch
            { }
        }
       
    }
}
