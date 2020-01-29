﻿/*
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

        public string Type { set; get; }

        public bool IsActive { set; get; }

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

        [EmailAddress]
        public string EmailId { set; get; }

    }


}
