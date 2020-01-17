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
        /// <returns></returns>
        public bool ForgetPassword(ForgetPasswordRequest forgetPassword)
        {
            try
            {
                var data = context.UserDetails.FirstOrDefault(user => user.EmailId == forgetPassword.EmailId);

                if (data == null)
                    return false;
                else
                    return true;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It Return True if the login is successfull.
        /// </summary>
        /// <param name="login">login parameter like email-id or password</param>
        /// <returns></returns>
        public ResponseModel Login(LoginRequest login)
        {
            try
            {
                login.Password = EncodeDecode.EncodePasswordToBase64(login.Password);
                var data = context.UserDetails.FirstOrDefault(user => (user.EmailId == login.EmailId) && (user.Password == login.Password));

                if (data != null)
                {
                    var userData = new ResponseModel()
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
        /// <returns></returns>
        public ResponseModel Registration(UserDetails userDetails)
        {
            try
            {
                userDetails.Password = EncodeDecode.EncodePasswordToBase64(userDetails.Password);
                userDetails.CreatedAt = DateTime.Now;
                userDetails.ModifiedAt = DateTime.Now;
                context.UserDetails.Add(userDetails);
                context.SaveChanges();

                var data = new ResponseModel()
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

        public bool ResetPassword(ResetPasswordRequest resetPassword)
        {
            try
            {
                return false;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
