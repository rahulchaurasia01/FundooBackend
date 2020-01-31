/*
 *  Purpose: Business Layer for User Model
 * 
 *  Author: Rahul Chaurasia
 *  Date: 16/1/2020
 */

using FundooBusinessLayer.Interface;
using FundooCommonLayer.Model;
using FundooCommonLayer.ModelDB;
using FundooRepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        /// Get All the Register User.
        /// </summary>
        /// <returns>List Of All the User</returns>
        public async Task<List<UserListResponseModel>> GetAllUsers(UserRequest userRequest, int userId)
        {
            if (userRequest == null || userId <= 0)
                return null;
            return await _userRepository.GetAllUsers(userRequest, userId);
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
        public async Task<UserResponseModel> Registration(RegisterRequest userDetails)
        {
            try
            {
                if (userDetails == null)
                    return null;
                else
                    return await _userRepository.Registration(userDetails);
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
        public async Task<bool> ResetPassword(ResetPasswordRequest resetPassword, int userId)
        {
            try
            {
                if (userId <= 0 || resetPassword.Password == null)
                    return false;
                else
                    return await _userRepository.ResetPassword(resetPassword, userId);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
