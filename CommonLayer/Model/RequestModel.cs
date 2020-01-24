/*
 *  Purpose: It Contains the Model that user may request 
 * 
 *  Author: Rahul Chaurasia
 *  Date: 17-01-2020
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    /// <summary>
    /// Register Request: To Register the new Uswe
    /// </summary>
    public class RegisterRequest
    {
        [Required]
        public string FirstName { set; get; }

        [Required]
        public string LastName { set; get; }

        [Required]
        [EmailAddress]
        public string EmailId { set; get; }

        [Required]
        public string Password { set; get; }

        [Required]
        public string Type { set; get; }

    }

    /// <summary>
    /// Note Request: When Authenicated User tries to Create or Update a Note
    /// </summary>
    public class NoteRequest
    {
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

        public DateTime Reminder { set; get; }

        public List<NotesLabelRequest> Label { set; get; }
    }

    /// <summary>
    /// Create a New Label
    /// </summary>
    public class LabelRequest
    {
        [Required]
        public string Name { set; get; }
    }

    /// <summary>
    /// Add A label for a Notes
    /// </summary>
    public class NotesLabelRequest
    {
        public int LabelId { set; get; }
    }



}
