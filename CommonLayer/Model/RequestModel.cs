/*
 *  Purpose: It Contains the Model that user may request 
 * 
 *  Author: Rahul Chaurasia
 *  Date: 17-01-2020
 */

using Microsoft.AspNetCore.Http;
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
    /// It Update The Notes
    /// </summary>
    public class UpdateNoteRequest
    {
        public string Title { set; get; }

        public string Description { set; get; }

    }

    /// <summary>
    /// It Add Or Remove the Reminder
    /// </summary>
    public class ReminderRequest
    {
        public DateTime? Reminder { set; get; }
    }

    /// <summary>
    /// It Delete the list of notes with there NoteId
    /// </summary>
    public class ListOfDeleteNotes
    {
        public List<DeleteIdRequest> DeleteNotes { set; get; }
    }

    /// <summary>
    /// Noteid Of the Note to be deleted
    /// </summary>
    public class DeleteIdRequest
    {
        public int NoteId { set; get;}
    }

    /// <summary>
    /// It Update the pin value of the list of Notes
    /// </summary>
    public class ListOfPinnedNotes
    {
        public List<PinnedRequest> PinnedNotes { set; get; }
    }

    /// <summary>
    /// Pin Request: When user tries to pin there notes.
    /// </summary>
    public class PinnedRequest
    {

        public int NoteId { set; get; }

        [Required]
        public bool IsPin { set; get; }
    }

    /// <summary>
    /// It Update the archive value of the list of Notes.
    /// </summary>
    public class ListOfArchiveNotes
    {
        public List<ArchiveRequest> ArchiveNotes { set; get; }
    }

    /// <summary>
    /// Archive Request: When User tries to Archive its notes.
    /// </summary>
    public class ArchiveRequest
    {
        public int NoteId { set; get; }

        public bool IsArchive { set; get; }
    }

    /// <summary>
    /// It Update the color of the list of notes.
    /// </summary>
    public class ListOfColorNotes
    {
        public List<ColorRequest> ColorNotes { set; get; }
    }

    /// <summary>
    /// Color Request: When user Tries to update its color.
    /// </summary>
    public class ColorRequest
    {
        public int NoteId { set; get; }

        [Required]
        [RegularExpression("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Enter a Valid Color")]
        public string Color { set; get; }
    }

    /// <summary>
    /// Image Request: When User Tries to Add Or Update The Note Images.
    /// </summary>
    public class ImageRequest
    {
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
    /// This Model Is used for Adding Label to notes.
    /// </summary>
    public class AddLabelNoteRequest
    {
        public List<NotesLabelRequest> Label { set; get; }
    }

    /// <summary>
    /// Add A label for a Notes
    /// </summary>
    public class NotesLabelRequest
    {
        public int LabelId { set; get; }
    }

    /// <summary>
    /// Add a List Of Collaborator to the Notes
    /// </summary>
    public class CollaboratorsRequest
    {
        public List<CollaboratorRequest> Collaborators { set; get; }
    }

    /// <summary>
    /// Add A user to Collaborator the notes with them.
    /// </summary>
    public class CollaboratorRequest
    {
        public int UserId { set; get; }

    }

    /// <summary>
    /// Register a New Admin Model
    /// </summary>
    public class AdminRegisterRequest
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

    }

    /// <summary>
    /// Notification Token
    /// </summary>
    public class NotificationRequest
    {
        public string Token { set; get; }
    }


}
