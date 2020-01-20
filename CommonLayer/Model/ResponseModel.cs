/*
 *  Purpose: It Send the User Data as a Response to Output
 * 
 * 
 *  Author: Rahul Chaurasia
 *  Date: 17-01-2020
 */



using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FundooCommonLayer.Model
{
    
    public class ResponseModel
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

}
