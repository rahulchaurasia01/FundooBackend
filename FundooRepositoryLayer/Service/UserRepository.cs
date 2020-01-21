/*
 *  Purpose: This layer interact with the Database.
 * 
 *  Author: Rahul Chaurasia
 *  Date: 16/1/2020
 */

using FundooCommonLayer.Model;
using FundooCommonLayer.ModelDB;
using FundooRepositoryLayer.Interface;
using FundooRepositoryLayer.ModelContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FundooRepositoryLayer.Service
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext context;

        public UserRepository(ApplicationContext applicationContext)
        {
            context = applicationContext;
        }

        /// <summary>
        /// It will check whether the Provided Email id is present in the database or not.
        /// </summary>
        /// <param name="forgetPassword">Email-Id</param>
        /// <returns>It return the Response Model to be Send as Response</returns>
        public UserResponseModel ForgetPassword(ForgetPasswordRequest forgetPassword)
        {
            try
            {
                var data = context.UserDetails.FirstOrDefault(user => user.EmailId == forgetPassword.EmailId);

                if (data != null)
                {
                    var userData = new UserResponseModel()
                    {
                        UserId = data.UserId,
                        FirstName = data.FirstName,
                        LastName = data.LastName,
                        EmailId = data.EmailId,
                        Type = data.Type,
                        IsActive = data.IsActive,
                        CreatedAt = data.CreatedAt,
                        ModifiedAt = data.ModifiedAt
                    };
                    return userData;
                }
                return null;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It Check whether the login data send by the user is correct or not.
        /// </summary>
        /// <param name="login">login parameter like email-id or password</param>
        /// <returns>It return the Response Model to be Send as Response</returns>
        public UserResponseModel Login(LoginRequest login)
        {
            try
            {
                login.Password = EncodeDecode.EncodePasswordToBase64(login.Password);
                var data = context.UserDetails.FirstOrDefault(user => (user.EmailId == login.EmailId) && (user.Password == login.Password));

                if (data != null)
                {
                    var userData = new UserResponseModel()
                    {
                        UserId = data.UserId,
                        FirstName = data.FirstName,
                        LastName = data.LastName,
                        EmailId = data.EmailId,
                        Type = data.Type,
                        IsActive = data.IsActive,
                        CreatedAt = data.CreatedAt,
                        ModifiedAt = data.ModifiedAt
                    };
                    return userData;
                }
                return null;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It Register the new User.
        /// </summary>
        /// <param name="userDetails">User Data</param>
        /// <returns>It return the Response Model to be Send as Response</returns>
        public UserResponseModel Registration(UserDetails userDetails)
        {
            try
            {
                userDetails.Password = EncodeDecode.EncodePasswordToBase64(userDetails.Password);
                userDetails.CreatedAt = DateTime.Now;
                userDetails.ModifiedAt = DateTime.Now;
                context.UserDetails.Add(userDetails);
                context.SaveChanges();

                var data = new UserResponseModel()
                {
                    UserId = userDetails.UserId,
                    FirstName = userDetails.FirstName,
                    LastName = userDetails.LastName,
                    EmailId = userDetails.EmailId,
                    Type = userDetails.Type,
                    IsActive = userDetails.IsActive,
                    CreatedAt = userDetails.CreatedAt,
                    ModifiedAt = userDetails.ModifiedAt
                };

                return data;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It Reset the User Password From the Database.
        /// </summary>
        /// <param name="resetPassword">ResetPassword Model</param>
        /// <returns>Return True if reset is successfull or else false</returns>
        public bool ResetPassword(ResetPasswordRequest resetPassword)
        {
            try
            {
                UserDetails userDetails = context.UserDetails.FirstOrDefault(usr => usr.UserId == resetPassword.UserId);

                if (userDetails != null)
                {
                    resetPassword.Password = EncodeDecode.EncodePasswordToBase64(resetPassword.Password);
                    userDetails.Password = resetPassword.Password;
                    userDetails.ModifiedAt = DateTime.Now;

                    var user = context.UserDetails.Attach(userDetails);
                    user.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    
    }
}
