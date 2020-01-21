using FundooCommonLayer.Model;
using FundooCommonLayer.ModelDB;
using System;
using System.Collections.Generic;

namespace FundooRepositoryLayer.Interface
{
    public interface IUserRepository
    {

        UserResponseModel Registration(UserDetails userDetails);

        UserResponseModel Login(LoginRequest login);

        UserResponseModel ForgetPassword(ForgetPasswordRequest forgetPassword);

        bool ResetPassword(ResetPasswordRequest resetPassword);

    }
}
