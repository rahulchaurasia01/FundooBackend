using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace MsmqListener
{
    public class Mail
    {

        public static bool SendTokenToMail(string resetPasswordLink, string email)
        {

            if (!string.IsNullOrWhiteSpace(resetPasswordLink) && !string.IsNullOrWhiteSpace(email))
            {

                MailMessage mailMessage = new MailMessage("fundooapplication@gmail.com", email);

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                mailMessage.Subject = "Forget Password Link";
                mailMessage.Body = "Click on the link to Reset your Password <br> " +
                    "<a href="+resetPasswordLink+">"+resetPasswordLink+"</a> ";
                mailMessage.IsBodyHtml = true;

                smtpClient.UseDefaultCredentials = true;
                smtpClient.EnableSsl = true;

                smtpClient.Credentials = new NetworkCredential("fundooapplication@gmail.com", "Fundoo@123");

                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Send(mailMessage);

                return true;
            }
            return false;

        }
    }
}
