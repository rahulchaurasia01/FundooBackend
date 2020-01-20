/*
 *  Purpose: This class will Receive the forgetPassword Email token from Msmq and
 *           Send the Email to the request User.
 * 
 *  Author: Rahul Chaurasia
 *  Date: 18-01-2020
 */

using Experimental.System.Messaging;
using System;
using System.Net;
using System.Net.Mail;

namespace FundooCommonLayer.Model
{
    public class MsmqReceiver
    {

        /// <summary>
        /// It will Receive the forgetPassword Email token from Msmq and
        /// Send the Email to the request User.
        /// </summary>
        /// <param name="forgetPassword">ForgetPassword user Email-Id</param>
        /// <returns>Return True if Email Send Successfully or else False</returns>
        public static bool ReceiveMsmq(ForgetPasswordRequest forgetPassword)
        {
            try
            {
                string path = @".\Private$\FundooMsmq";
                if(MessageQueue.Exists(path))
                {
                    MessageQueue messageQueue = new MessageQueue(path);
                    Message message = messageQueue.Receive();
                    message.Formatter = new BinaryMessageFormatter();
                    string token = message.Body.ToString();

                    MailMessage mailMessage = new MailMessage("fundooapplication@gmail.com", forgetPassword.EmailId);

                    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                    mailMessage.Subject = "Forget Password Link";
                    mailMessage.Body = "Click on the link to Reset your Password <br> " + token;
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
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }


    }
}
