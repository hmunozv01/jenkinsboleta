using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace AppFormBoletaGarantia.Class
{
    public class EmailHelper
    {
        public static async Task<bool> Send(string from, string to, string subject, string body)
        {
            try
            {
                MailMessage message = new MailMessage();

                message.To.Add(new MailAddress(to));
                message.From = new MailAddress(from);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    //NetworkCredential credential = new NetworkCredential
                    //{
                    //    UserName = "user",  // replace with valid value
                    //    Password = "pass"  // replace with valid value
                    //};
                    //smtp.Credentials = credential;
                    smtp.Host = ConfigHelper.AppEmailHost;
                    smtp.Port = ConfigHelper.AppEmailPort;
                    smtp.EnableSsl = false;
                    await smtp.SendMailAsync(message);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}