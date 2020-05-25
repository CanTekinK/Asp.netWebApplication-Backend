using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace WebApplication18.Helpers
{
    public class MailSender
    {
        public void mailSender(string body,string toAdress,string senderAdress,string senderAdressPassword)
        {
            var fromAddress = new MailAddress(senderAdress);
            var toAddress = new MailAddress(toAdress);
            const string subject = "Site Adı | Sitenizden Gelen İletişim Formu";
            using (var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, senderAdressPassword)
            })
            {
                using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body })
                {
                    smtp.Send(message);
                }
            }
        }
    }
}
