using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FundooCommonLayer.Model
{
    public class ForgetPasswordRequest
    {

        [Required]
        [EmailAddress]
        public string EmailId { set; get; }

    }


    public class ResetPasswordRequest
    {
        public int UserId { set; get; }

        [Required]
        public string Password { set; get; }

    }


    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string EmailId { set; get; }

        [Required]
        public string Password { set; get; }

    }
}
