/*
 *  Purpose: It Send the User Data as a Response to Output
 * 
 * 
 *  Author: Rahul Chaurasia
 *  Date: 17-01-2020
 */

using FundooCommonLayer.ModelDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FundooCommonLayer.Model
{
    
    public class UserResponseModel
    {

        public int UserId { set; get; }

        public string FirstName { set; get; }

        public string LastName { set; get; }

        [EmailAddress]
        public string EmailId { set; get; }

        public string ProfilePic { set; get; }

        public string Type { set; get; }

        public bool IsActive { set; get; }

        public string UserRole { set; get; }

        public DateTime CreatedAt { set; get; }

        public DateTime ModifiedAt { set; get; }

    }

    public class NoteResponseModel
    {
        public int NoteId { set; get; }

        public string Title { set; get; }

        public string Description { set; get; }

        public string Color { set; get; }

        public string Image { set; get; }

        [DefaultValue(false)]
        public bool IsPin { set; get; }

        [DefaultValue(false)]
        public bool IsArchived { set; get; }

        [DefaultValue(false)]
        public bool IsDeleted { set; get; }

        public DateTime? Reminder { set; get; }

        public DateTime CreatedAt { set; get; }

        public DateTime ModifiedAt { set; get; }

        public List<LabelResponseModel> Labels { set; get; }

        public List<CollaboratorResponseModel> Collaborators { set; get; }

    }

    public class LabelResponseModel
    {
        public int LabelId { set; get; }

        public string Name { set; get; }

        public DateTime CreatedAt { set; get; }

        public DateTime ModifiedAt { set; get; }

    }

    public class UserListResponseModel
    {

        public int UserId { set; get; }

        public string FirstName { set; get; }

        public string LastName { set; get; }

        [EmailAddress]
        public string EmailId { set; get; }

    }

    public class CollaboratorResponseModel
    {
        public int UserId { set; get; }

        public string FirstName { set; get; }

        public string LastName { set; get; }

        public string EmailId { set; get; }
    }

    public class AdminResponseModel
    {
        public int UserId { set; get; }

        public string FirstName { set; get; }

        public string LastName { set; get; }

        [EmailAddress]
        public string EmailId { set; get; }

        public bool IsActive { set; get; }

        public string UserRole { set; get; }

        public DateTime CreatedAt { set; get; }

        public DateTime ModifiedAt { set; get; }
    }

    public class AdminStatisticsResponseModel
    {
        public int Basic { set; get; }

        public int Advanced { set; get; }

    }

    public class AdminUserListResponseModel
    {
        public List<UserList> records { set; get; }

        public string previous { set; get; }

        public string next { set; get; }

        public int count { set; get; }

    }

    public class UserList
    {

        public int UserId { set; get; }

        public string FirstName { set; get; }

        public string LastName { set; get; }

        public string EmailId { set; get; }

        public string Service { set; get; }

        public int Notes { set; get; }

    }

    
    public class ReminderNotificationResponseModel
    {
        public int UserId { set; get; }

        public string Token { set; get; }

        public int NoteId { set; get; }

        public string Title { set; get; }

        public string Desciption { set; get; }

        public DateTime Reminder { set; get; }

    }


}
