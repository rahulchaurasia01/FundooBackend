using FundooCommonLayer.Model;
using FundooCommonLayer.ModelDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace FundooBusinessLayer.Interface
{
    public interface IUserBusiness
    {

        ResponseModel Registration(UserDetails userDetails);

        ResponseModel Login(LoginRequest login);

        bool ForgetPassword(ForgetPasswordRequest forgetPassword);

        bool ResetPassword(ResetPasswordRequest resetPassword);

    }
}
