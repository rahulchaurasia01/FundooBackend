using System;
using System.Collections.Generic;
using System.Text;

namespace FundooUnitTest
{
    public class UserAccountUnitTest
    {

        public bool Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }
            else if (!email.Contains('@') || !email.Contains('.'))
            {
                return false;
            }
            else if(password.Length <= 3 )
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public bool ForgetPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            else if (!email.Contains('@') || !email.Contains('.'))
                return false;
            else
                return true;

        }

    }
}
