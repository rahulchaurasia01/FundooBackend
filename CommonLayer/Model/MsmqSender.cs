﻿using Experimental.System.Messaging;
using System;

namespace FundooCommonLayer.Model
{
    public class MsmqSender
    {

        public static void SendToMsmq(string token)
        {
            try
            {
                string path = @".\Private$\FundooMsmq";
                string Desciption = "User ForgetPassword Token";
                MessageQueue messageQueue = null;
                if(!MessageQueue.Exists(path))
                {
                    MessageQueue.Create(path);
                }
                messageQueue = new MessageQueue(path);
                messageQueue.Label = Desciption;
                Message message = new Message(token);
                message.Formatter = new BinaryMessageFormatter();
                messageQueue.Send(message);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
           

        }


    }
}
