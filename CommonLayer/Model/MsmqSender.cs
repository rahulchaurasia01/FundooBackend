/*
 *  Purpose: This class will Send the forgetPassword Email token to Msmq
 * 
 *  Author: Rahul Chaurasia
 *  Date: 18-01-2020
 */

using Experimental.System.Messaging;
using System;

namespace FundooCommonLayer.Model
{
    public class MsmqSender
    {
        /// <summary>
        /// It will send the ForgetPassword Email Token to Msmq   
        /// </summary>
        /// <param name="token">Email Token</param>
        public static void SendToMsmq(string email, string token)
        {
            try
            {
                string path = @".\Private$\FundooMsmq";
                MessageQueue messageQueue = null;
                if(!MessageQueue.Exists(path))
                {
                    MessageQueue.Create(path);
                }
                messageQueue = new MessageQueue(path);
                messageQueue.Label = "Fundoo Email Sending";
                string resetPasswordLink = "http://localhost:4200/resetpassword/" + token;
                Message message = new Message(resetPasswordLink);
                message.Formatter = new BinaryMessageFormatter();
                messageQueue.Send(message, email);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
           

        }


    }
}
