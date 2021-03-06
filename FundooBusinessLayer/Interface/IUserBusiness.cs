﻿using FundooCommonLayer.Model;
using FundooCommonLayer.ModelDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FundooBusinessLayer.Interface
{
    public interface IUserBusiness
    {

        Task<bool> AddNotification(NotificationRequest notificationRequest, int userId);

        Task<List<UserListResponseModel>> GetAllUsers(UserRequest userRequest, int userId);

        List<ReminderNotificationResponseModel> ReminderNotification(DateTime currentTime, DateTime endTime);

        Task<UserResponseModel> AddUpdateProfilePic(ImageRequest imageRequest, int userId);

        Task<UserResponseModel> Registration(RegisterRequest userDetails);

        UserResponseModel Login(LoginRequest login);

        UserResponseModel ForgetPassword(ForgetPasswordRequest forgetPassword);

        Task<bool> ResetPassword(ResetPasswordRequest resetPassword, int userId);

    }
}
