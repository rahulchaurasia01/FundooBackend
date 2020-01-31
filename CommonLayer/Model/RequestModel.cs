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
        [EmailAddress(ErrorMessage = "Please Input a Proper Email-Id")]
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
        [MaxLength(12, ErrorMessage = "Your FirstName Length Should be Less Than 12.")]
        [MinLength(3, ErrorMessage = "Your FirstName Length Should be more than 3")]
        public string FirstName { set; get; }

        [Required]
        [MaxLength(12, ErrorMessage = "Your LastName Length Should be Less Than 12.")]
        [MinLength(3, ErrorMessage = "Your LastName Length Should be more than 3")]
        public string LastName { set; get; }

        [Required]
        [EmailAddress(ErrorMessage = "Please Input a Proper Email-Id")]
        public string EmailId { set; get; }

        [Required]
        [MinLength(5, ErrorMessage = "Your Password Should be Minimum Length of 5.")]
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

        [RegularExpression("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Enter a Valid Color")]
        public string Color { set; get; }

        public string Image { set; get; }

        [DefaultValue(false)]
        public bool IsPin { set; get; }

        [DefaultValue(false)]
        public bool IsArchived { set; get; }

        [DefaultValue(false)]
        public bool IsDeleted { set; get; }

        public DateTime? Reminder { set; get; }

        public List<NotesLabelRequest> Label { set; get; }

        public List<CollaboratorRequest> Collaborators { set; get; }
    }

    /// <summary>
    /// Pin Request: When user tries to pin there notes.
    /// </summary>
    public class PinnedRequest
    {
        [Required]
        public bool IsPin { set; get; }
    }

    /// <summary>
    /// Archive Request: When User tries to Archive its notes.
    /// </summary>
    public class ArchiveRequest
    {
        public bool IsArchive { set; get; }
    }

    /// <summary>
    /// Color Request: When user Tries to update its color.
    /// </summary>
    public class ColorRequest
    {
        [Required]
        [RegularExpression("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Enter a Valid Color")]
        public string Color { set; get; }
    }

    /// <summary>
    /// Image Request: When User Tries to Add Or Update The Note Images.
    /// </summary>
    public class ImageRequest
    {
        [Required]
        public string Image { set; get; }
    }

    /// <summary>
    /// User Request: To Get the List Of user Register.
    /// </summary>
    public class UserRequest
    {
        [Required]
        public string EmailId { set; get; }
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

    /// <summary>
    /// Add A user to Collaborator the notes with them.
    /// </summary>
    public class CollaboratorRequest
    {
        public int UserId { set; get; }

    }


}
