using FundooCommonLayer.Model;
using FundooCommonLayer.ModelDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace FundooBusinessLayer.Interface
{
    public interface IUserBusiness
    {

        UserResponseModel Registration(RegisterRequest userDetails);

        UserResponseModel Login(LoginRequest login);

        UserResponseModel ForgetPassword(ForgetPasswordRequest forgetPassword);

        bool ResetPassword(ResetPasswordRequest resetPassword, int userId);

    }
}
