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

        /// <summary>
        /// If User Forget their Password
        /// </summary>
        /// <param name="forgetPassword">Forget Password Model</param>
        /// <returns>It return Response Model if Successfull or else null</returns>
        public UserResponseModel ForgetPassword(ForgetPasswordRequest forgetPassword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(forgetPassword.EmailId))
                    return null;
                else
                    return _userRepository.ForgetPassword(forgetPassword);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Login the user
        /// </summary>
        /// <param name="login">Login Model</param>
        /// <returns>It Return Response Model, If Login successfull or else null</returns>
        public UserResponseModel Login(LoginRequest login)
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
        
        /// <summary>
        /// It Register the User.
        /// </summary>
        /// <param name="userDetails"></param>
        /// <returns>Return ResponseModel if Successfull or else Null</returns>
        public UserResponseModel Registration(UserDetails userDetails)
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

        /// <summary>
        /// Reset the User Password
        /// </summary>
        /// <param name="resetPassword">Reset Password Model</param>
        /// <returns>It Return true if reset is successfull or else false.</returns>
        public bool ResetPassword(ResetPasswordRequest resetPassword)
        {
            try
            {
                if (resetPassword.UserId == 0 || resetPassword.Password == null)
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
