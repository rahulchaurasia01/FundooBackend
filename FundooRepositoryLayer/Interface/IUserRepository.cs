using FundooCommonLayer.Model;
using FundooCommonLayer.ModelDB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FundooRepositoryLayer.Interface
{
    public interface IUserRepository
    {

        Task<UserResponseModel> Registration(RegisterRequest userDetails);

        UserResponseModel Login(LoginRequest login);

        UserResponseModel ForgetPassword(ForgetPasswordRequest forgetPassword);

        Task<bool> ResetPassword(ResetPasswordRequest resetPassword, int userId);

    }
}
