using System;

namespace MsmqListener
{

    class Program
    {
        static void Main(string[] args)
        {

            string path = @".\Private$\FundooMsmq";
            MsmqListener msmqListener = new MsmqListener(path);
            msmqListener.Start();

        }
    }
}
