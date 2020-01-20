/*
 *  Purpose: It Contains the Model that user may request 
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
    /// <summary>
    /// ForgetPassword Request: When User ForgetPassword their Password.
    /// </summary>
    public class ForgetPasswordRequest
    {

        [Required]
        [EmailAddress]
        public string EmailId { set; get; }

    }

    /// <summary>
    /// ResetPassword Request: When User tries to reset the Password
    /// </summary>
    public class ResetPasswordRequest
    {
        public int UserId { set; get; }

        [Required]
        public string Password { set; get; }

    }

    /// <summary>
    /// Login Request: When User tries to login.
    /// </summary>
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string EmailId { set; get; }

        [Required]
        public string Password { set; get; }

    }
}
