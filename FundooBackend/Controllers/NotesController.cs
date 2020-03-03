using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FundooBusinessLayer.Interface;
using FundooCommonLayer.Model;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FundooCommonLayer.ModelDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FundooAppBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {

        private readonly INotesBusiness _notesBusiness;
        private readonly IConfiguration _configuration;

        private static readonly string _login = "Login";
        private static readonly string _regularUser = "Regular User";

        private static readonly string _tokenType = "TokenType";
        private static readonly string _userType = "UserType";
        private static readonly string _userId = "UserId";

        public NotesController(INotesBusiness notesBusiness, IConfiguration configuration)
        {
            _notesBusiness = notesBusiness;
            _configuration = configuration;
        }

        /// <summary>
        /// It Create the New Notes
        /// </summary>
        /// <param name="notesDetails">Note data</param>
        /// <returns>If Found, It return 200 or else NotFound Response or Any Execption
        /// occured and Not Proper Input Given it return BadRequest.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateNote(NoteRequest notesDetails)
        {
            try
            {
                var user = HttpContext.User;
                bool status = false;
                string message;
                if (user.HasClaim(c => c.Type == _tokenType))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == _tokenType).Value == _login &&
                        user.Claims.FirstOrDefault(c => c.Type == _userType).Value == _regularUser)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == _userId).Value);

                        NoteResponseModel data = await _notesBusiness.CreateNotes(notesDetails, UserId);
                        if (notesDetails != null)
                        {
                            status = true;
                            message = "Your Notes Has Been Successfully Created";
                            return Ok(new { status, message, data });
                        }
                        else
                        {
                            message = "Unable to Create Notes.";
                            return NotFound(new { status, message });
                        } 
                    }
                }
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch(Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        /// <summary>
        /// It Retrieve the Single Notes
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <returns>If Found, It return 200 or else NotFound Response or Any Execption
        /// occured and Not Proper Input Given it return BadRequest.</returns>
        [HttpGet("{NoteId}")]
        public async Task<IActionResult> GetNote(int NoteId)
        {
            try
            {
                var user = HttpContext.User;
                bool status = false;
                string message;
                if (user.HasClaim(c => c.Type == _tokenType))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == _tokenType).Value == _login &&
                        user.Claims.FirstOrDefault(c => c.Type == _userType).Value == _regularUser)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == _userId).Value);
                        NoteResponseModel notesDetails = await _notesBusiness.GetNote(NoteId, UserId);
                        if(notesDetails != null)
                        {
                            status = true;
                            message = "Your Note.";
                            return Ok(new { status, message, notesDetails });
                        }
                        message = "No Such Notes is Present";
                        return NotFound(new { status, message });
                    }
                }
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch(Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        /// <summary>
        /// It Retrieve the list of all the notes.
        /// </summary>
        /// <returns>If Found, It return 200 or else NotFound Response or Any Execption
        /// occured and Not Proper Input Given it return BadRequest.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllNotes(string search)
        {
            try
            {
                var user = HttpContext.User;
                bool status = false;
                string message;
                if (user.HasClaim(c => c.Type == _tokenType))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == _tokenType).Value == _login &&
                        user.Claims.FirstOrDefault(c => c.Type == _userType).Value == _regularUser)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == _userId).Value);
                        List<NoteResponseModel> data = await _notesBusiness.GetAllNotes(UserId, search);
                        if (data != null && data.Count > 0)
                        {
                            status = true;
                            message = "Here is the List Of all Notes.";
                            return Ok(new { status, message, data });
                        }
                        message = "No Notes Currently Present";
                        return Ok(new { status, message });
                    }
                }
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        /// <summary>
        /// It Retrieve the list of all Deleted Notes Present in the Trash
        /// </summary>
        /// <returns>If Found, It return 200 or else NotFound Response or Any Execption
        /// occured and Not Proper Input Given it return BadRequest.</returns>
        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> GetAllDeletedNotes()
        {
            try
            {
                var user = HttpContext.User;
                bool status = false;
                string message;
                if (user.HasClaim(c => c.Type == _tokenType))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == _tokenType).Value == _login &&
                        user.Claims.FirstOrDefault(c => c.Type == _userType).Value == _regularUser)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == _userId).Value);
                        List<NoteResponseModel> data = await _notesBusiness.GetAllDeletedNotes(UserId);
                        if (data != null && data.Count > 0)
                        {
                            status = true;
                            message = "Here is the List Of all Deleted Notes.";
                            return Ok(new { status, message, data });
                        }
                        message = "No Notes Currently Deleted";
                        return Ok(new { status, message });
                    }
                }
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        /// <summary>
        /// It Retrieve the list of all Archieved Notes
        /// </summary>
        /// <returns>If Found, It return 200 or else NotFound Response or Any Execption
        /// occured and Not Proper Input Given it return BadRequest.</returns>
        [HttpGet]
        [Route("Archive")]
        public async Task<IActionResult> GetAllArchivedNotes()
        {
            try
            {
                var user = HttpContext.User;
                bool status = false;
                string message;
                if (user.HasClaim(c => c.Type == _tokenType))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == _tokenType).Value == _login &&
                        user.Claims.FirstOrDefault(c => c.Type == _userType).Value == _regularUser)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == _userId).Value);
                        List<NoteResponseModel> data = await _notesBusiness.GetAllArchivedNotes(UserId);
                        if (data != null && data.Count > 0)
                        {
                            status = true;
                            message = "Here is the List Of all Archived Notes.";
                            return Ok(new { status, message, data });
                        }
                        message = "No Notes Currently Archived";
                        return Ok(new { status, message });
                    }
                }
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        /// <summary>
        /// It Retreive the List Of All Pinned Notes
        /// </summary>
        /// <returns>If Found, It return 200 or else NotFound Response or Any Execption
        /// occured and Not Proper Input Given it return BadRequest.</returns>
        [HttpGet]
        [Route("PinnedNotes")]
        public async Task<IActionResult> GetAllPinnedNotes()
        {
            try
            {
                var user = HttpContext.User;
                bool status = false;
                string message;
                if (user.HasClaim(c => c.Type == _tokenType))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == _tokenType).Value == _login &&
                        user.Claims.FirstOrDefault(c => c.Type == _userType).Value == _regularUser)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == _userId).Value);
                        List<NoteResponseModel> data = await _notesBusiness.GetAllPinnedNotes(UserId);
                        if (data != null && data.Count > 0)
                        {
                            status = true;
                            message = "Here is the List Of all Pinned Notes.";
                            return Ok(new { status, message, data });
                        }
                        message = "No Notes Currently Pinned";
                        return Ok(new { status, message });
                    }
                }
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        /// <summary>
        /// Api For Getting all the Notes By Reminder.
        /// </summary>
        /// <returns>If Found, It return 200 or else NotFound Response or Any Execption
        /// occured and Not Proper Input Given it return BadRequest.
        /// </returns>
        [HttpGet]
        [Route("Reminder")]
        public async Task<IActionResult> GetAllReminderNotes()
        {
            try
            {
                var user = HttpContext.User;
                bool status = false;
                string message;
                if (user.HasClaim(c => c.Type == _tokenType))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == _tokenType).Value == _login &&
                        user.Claims.FirstOrDefault(c => c.Type == _userType).Value == _regularUser)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == _userId).Value);
                        List<NoteResponseModel> data = await _notesBusiness.GetAllReminderNotes(UserId);
                        if (data != null && data.Count > 0)
                        {
                            status = true;
                            message = "Here is the List Of all Notes By Upcoming Reminder.";
                            return Ok(new { status, message, data });
                        }
                        message = "No Notes Currently on Reminder.";
                        return Ok(new { status, message });
                    }
                }
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        /// <summary>
        /// It Update The Selected Notes
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="updateNotesDetails">Note Data</param>
        /// <returns>If Found, It return 200 or else NotFound Response or Any Execption
        /// occured and Not Proper Input Given it return BadRequest.</returns>
        [HttpPut("{NoteId}")]
        public async Task<IActionResult> UpdateNote(int NoteId, UpdateNoteRequest updateNotesDetails)
        {
            try
            {
                var user = HttpContext.User;
                bool status = false;
                string message;
                if (user.HasClaim(c => c.Type == _tokenType))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == _tokenType).Value == _login &&
                        user.Claims.FirstOrDefault(c => c.Type == _userType).Value == _regularUser)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == _userId).Value);

                        NoteResponseModel data = await _notesBusiness.UpdateNotes(NoteId, UserId, updateNotesDetails);
                        if (data != null)
                        {
                            status = true;
                            message = "Note Updated Successfull";
                            return Ok(new { status, message, data });
                        }
                        message = "Note Cannot be Updated.";
                        return NotFound(new { status, message });
                    }
                }
                message = "Invalid Token.";
                return BadRequest(new { status, message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        /// <summary>
        /// It Delete the Note and Put it in Trash, and If Deleted from trash it remove it Permanentely
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <returns>If Found, It return 200 or else NotFound Response or Any Execption
        /// occured and Not Proper Input Given it return BadRequest.</returns>
        [HttpDelete]
        [Route("{NoteId}")]
        public async Task<IActionResult> DeleteNote(int NoteId)
        {
            try
            {
                var user = HttpContext.User;
                bool status = false;
                string message;
                if (user.HasClaim(c => c.Type == _tokenType))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == _tokenType).Value == _login &&
                        user.Claims.FirstOrDefault(c => c.Type == _userType).Value == _regularUser)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == _userId).Value);
                        status = await _notesBusiness.DeleteNote(NoteId, UserId);
                        if(status)
                        {
                            message = "Note Deleted Successfully";
                            return Ok(new { status, message });
                        }
                        message = "No Such Note Present to Delete. !!";
                        return NotFound(new { status, message });
                    }
                }
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        /// <summary>
        /// It Delete the List of Notes Permanentely
        /// </summary>
        /// <returns>If Found, It return 200 or else NotFound Response or Any Execption
        /// occured and Not Proper Input Given it return BadRequest.</returns>
        [HttpDelete]
        [Route("BulkDelete")]
        public async Task<IActionResult> DeleteNotesPermanently()
        {
            try
            {
                var user = HttpContext.User;
                bool status = false;
                string message;
                if (user.HasClaim(c => c.Type == _tokenType))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == _tokenType).Value == _login &&
                        user.Claims.FirstOrDefault(c => c.Type == _userType).Value == _regularUser)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == _userId).Value);
                        status = await _notesBusiness.DeleteNotesPermanently(UserId);
                        if (status)
                        {
                            message = "Your Notes has been Permanently Deleted";
                            return Ok(new { status, message });
                        }
                        message = "No Note Present to Delete. !!";
                        return Ok(new { status, message });
                    }
                }
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        /// <summary>
        /// It Mark the Notes as Pin
        /// </summary>
        /// <param name="pinnedRequest">Notes Id</param>
        /// <returns>If Found, It return 200 or else NotFound Response or Any Execption
        /// occured and Not Proper Input Given it return BadRequest.</returns>
        [HttpPut]
        [Route("{NoteId}/Pin")]
        public async Task<IActionResult> PinUnpinNotes(int NoteId, PinnedRequest pinnedRequest)
        {
            try
            {
                var user = HttpContext.User;
                bool status = false;
                string message;
                if (user.HasClaim(c => c.Type == _tokenType))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == _tokenType).Value == _login &&
                        user.Claims.FirstOrDefault(c => c.Type == _userType).Value == _regularUser)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == _userId).Value);
                        NoteResponseModel data = await _notesBusiness.PinOrUnPinTheNote(NoteId, pinnedRequest, UserId);
                        if (data != null)
                        {
                            status = true;
                            message = "The Note has been Successfully Pinned.";
                            return Ok(new { status, message, data });
                        }
                        message = "Unable to Pinned the Note";
                        return NotFound(new { status, message });
                    }
                }
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        /// <summary>
        /// It Mark the Notes as Archive.
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="archiveRequest">Archive Value</param>
        /// <returns>If Found, It return 200 or else NotFound Response or Any Execption
        /// occured and Not Proper Input Given it return BadRequest.</returns>
        [HttpPut]
        [Route("{NoteId}/Archive")]
        public async Task<IActionResult> ArchiveUnArchiveNote(int NoteId, ArchiveRequest archiveRequest)
        {
            try
            {
                var user = HttpContext.User;
                bool status = false;
                string message;
                if (user.HasClaim(c => c.Type == _tokenType))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == _tokenType).Value == _login &&
                        user.Claims.FirstOrDefault(c => c.Type == _userType).Value == _regularUser)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == _userId).Value);
                        NoteResponseModel data = await _notesBusiness.ArchiveUnArchiveTheNote(NoteId, archiveRequest, UserId);
                        if (data != null)
                        {
                            status = true;
                            message = "The Note has been Successfully Archived.";
                            return Ok(new { status, message, data });
                        }
                        message = "Unable to Archived the Note";
                        return NotFound(new { status, message });
                    }
                }
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch(Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        /// <summary>
        /// It Mark the color of the Note
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="colorRequest">Color Value</param>
        /// <returns>If Found, It return 200 or else NotFound Response or Any Execption
        /// occured and Not Proper Input Given it return BadRequest.</returns>
        [HttpPut]
        [Route("{NoteId}/Color")]
        public async Task<IActionResult> ColorNote(int NoteId, ColorRequest colorRequest)
        {
            try
            {
                var user = HttpContext.User;
                bool status = false;
                string message;
                if (user.HasClaim(c => c.Type == _tokenType))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == _tokenType).Value == _login &&
                        user.Claims.FirstOrDefault(c => c.Type == _userType).Value == _regularUser)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == _userId).Value);
                        NoteResponseModel data = await _notesBusiness.ColorTheNote(NoteId, colorRequest, UserId);
                        if (data != null)
                        {
                            status = true;
                            message = "The Color Has Been Successfully Added To the Note.";
                            return Ok(new { status, message, data });
                        }
                        message = "Unable to Color the Note.";
                        return NotFound(new { status, message });
                    }
                }
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch(Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        /// <summary>
        /// It Add or Update the Image of the notes.
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="imageRequest">Image Data</param>
        /// <returns>If Found, It return 200 or else NotFound Response or Any Execption
        /// occured and Not Proper Input Given it return BadRequest.</returns>
        [HttpPut]
        [Route("{NoteId}/Image")]
        public async Task<IActionResult> AddUpdateImage(int NoteId, [FromForm] IFormFile file)
        {
            try
            {
                var user = HttpContext.User;
                bool status = false;
                string message;
                if (user.HasClaim(c => c.Type == _tokenType))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == _tokenType).Value == _login &&
                        user.Claims.FirstOrDefault(c => c.Type == _userType).Value == _regularUser)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == _userId).Value);

                        if (file == null || file.Length <= 0)
                        {
                            message = "No Image Provided";
                            return BadRequest(new { status, message });
                        }

                        if (file.FileName.EndsWith(".jpg") || file.FileName.EndsWith(".png"))
                        {

                            ImageRequest imageRequest1 = new ImageRequest
                            {
                                Image = UploadImageToCloudinary(file)
                            };

                            NoteResponseModel data = await _notesBusiness.AddUpdateImage(NoteId, imageRequest1, UserId);
                            if (data != null)
                            {
                                status = true;
                                message = "The Image has Been Successfully Added To the Note.";
                                return Ok(new { status, message, data });
                            }
                            message = "Unable to Add the Image to the Note.";
                            return NotFound(new { status, message });
                        }
                        else
                        {
                            message = "Please Provide the .jpg or .png Images only";
                            return BadRequest(new { status, message });
                        }
                    }
                }
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch(Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        /// <summary>
        /// It Add Or Update the Collaborator to the Notes
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="collaboratorsRequest">Collaborator Data</param>
        /// <returns>If Found, It return 200 or else NotFound Response or Any Execption
        /// occured and Not Proper Input Given it return BadRequest.</returns>
        [HttpPut]
        [Route("{NoteId}/Collaborator")]
        public async Task<IActionResult> AddUpdateCollaborator(int NoteId, CollaboratorsRequest collaboratorsRequest)
        {
            try
            {
                var user = HttpContext.User;
                bool status = false;
                string message;
                if (user.HasClaim(c => c.Type == _tokenType))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == _tokenType).Value == _login &&
                        user.Claims.FirstOrDefault(c => c.Type == _userType).Value == _regularUser)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == _userId).Value);

                        NoteResponseModel data = await _notesBusiness.AddUpdateCollaborator(NoteId, collaboratorsRequest, UserId);
                        if (data != null)
                        {
                            status = true;
                            message = "The Collaborators has Been Successfully Added To the Note.";
                            return Ok(new { status, message, data });
                        }
                        message = "Unable to Add the Collaborators to the Note.";
                        return NotFound(new { status, message });
                    }
                }
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch(Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }


        
        /// <summary>
        /// It Restore the Deleted Notes.
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <returns>If Found, It return 200 or else NotFound Response or Any Execption
        /// occured and Not Proper Input Given it return BadRequest.</returns>
        [HttpPut]
        [Route("Restore/{NoteId}")]
        public async Task<IActionResult> RestoreDeletedNotes(int NoteId)
        {
            try
            {
                var user = HttpContext.User;
                bool status = false;
                string message;
                if (user.HasClaim(c => c.Type == _tokenType))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == _tokenType).Value == _login &&
                        user.Claims.FirstOrDefault(c => c.Type == _userType).Value == _regularUser)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == _userId).Value);
                        status = await _notesBusiness.RestoreDeletedNotes(NoteId, UserId);
                        if (status)
                        {
                            message = "Your Notes has Been Successfully Restored";
                            return Ok(new { status, message });
                        }
                        message = "No Note Present to be Restored. !!";
                        return Ok(new { status, message });
                    }
                }
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }




        /// <summary>
        /// It Upload the Image to Cloudinary and Get the url Of the uploaded Image
        /// </summary>
        /// <param name="imageRequest">Image Data</param>
        private string UploadImageToCloudinary(IFormFile getImageFrom)
        {
            try
            {
                var Account = new Account(_configuration["Cloudinary:Cloud_Name"],
                    _configuration["Cloudinary:Api_Key"], _configuration["Cloudinary:Api_Secret"]);

                Cloudinary cloudinary = new Cloudinary(Account);

                var imageUpload = new ImageUploadParams
                {
                    File = new FileDescription(getImageFrom.FileName, getImageFrom.OpenReadStream()),
                    Folder = "FundooNotes"
                };

                var uploadImage = cloudinary.Upload(@imageUpload);

                return uploadImage.SecureUri.AbsoluteUri;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}