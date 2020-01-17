using FundooBusinessLayer.Interface;
using FundooCommonLayer.Model;
using FundooCommonLayer.ModelDB;
using FundooRepositoryLayer.Interface;
using System;

namespace FundooBusinessLayer.Service
{
    public class UserBusiness : IUserBusiness
    {

        private readonly IUserRepository _userRepository;

        public UserBusiness(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool ForgetPassword(ForgetPasswordRequest forgetPassword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(forgetPassword.EmailId))
                    return false;
                else
                    return _userRepository.ForgetPassword(forgetPassword);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public ResponseModel Login(LoginRequest login)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(login.EmailId) || string.IsNullOrWhiteSpace(login.Password))
                    return null;
                else
                    return _userRepository.Login(login);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public ResponseModel Registration(UserDetails userDetails)
        {
            try
            {
                if (userDetails == null)
                    return null;
                else
                    return _userRepository.Registration(userDetails);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool ResetPassword(ResetPasswordRequest resetPassword)
        {
            try
            {
                if (resetPassword.ResetToken == null || resetPassword.Password == null)
                    return false;
                else
                    return _userRepository.ResetPassword(resetPassword);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
