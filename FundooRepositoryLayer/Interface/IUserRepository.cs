using FundooCommonLayer.Model;
using FundooCommonLayer.ModelDB;
using System;
using System.Collections.Generic;

namespace FundooRepositoryLayer.Interface
{
    public interface IUserRepository
    {

        ResponseModel Registration(UserDetails userDetails);

        ResponseModel Login(LoginRequest login);

        ResponseModel ForgetPassword(ForgetPasswordRequest forgetPassword);

        bool ResetPassword(ResetPasswordRequest resetPassword);

    }
}
