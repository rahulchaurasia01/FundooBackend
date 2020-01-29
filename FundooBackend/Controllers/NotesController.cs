using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FundooBusinessLayer.Interface;
using FundooCommonLayer.Model;
using FundooCommonLayer.ModelDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FundooAppBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {

        private readonly INotesBusiness _notesBusiness;

        private static readonly string _login = "Login";

        public NotesController(INotesBusiness notesBusiness)
        {
            _notesBusiness = notesBusiness;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote(NoteRequest notesDetails)
        {
            try
            {
                var user = HttpContext.User;
                bool status;
                string message;
                if (user.HasClaim(c => c.Type == "TokenType"))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == "TokenType").Value == _login)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                        NoteResponseModel data = await _notesBusiness.CreateNotes(notesDetails, UserId);
                        if (notesDetails != null)
                        {
                            status = true;
                            message = "Your Notes Has Been Successfully Created";
                            return Ok(new { status, message, data });
                        }
                        else
                        {
                            status = false;
                            message = "Unable to Create Notes.";
                            return NotFound(new { status, message });
                        } 
                    }
                }
                status = false;
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch(Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        [HttpGet("{NoteId}")]
        public async Task<IActionResult> GetNote(int NoteId)
        {
            try
            {
                var user = HttpContext.User;
                bool status;
                string message;
                if (user.HasClaim(c => c.Type == "TokenType"))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == "TokenType").Value == _login)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                        NoteResponseModel notesDetails = await _notesBusiness.GetNote(NoteId, UserId);
                        if(notesDetails != null)
                        {
                            status = true;
                            message = "Your Note.";
                            return Ok(new { status, message, notesDetails });
                        }
                        status = false;
                        message = "No Such Notes is Present";
                        return NotFound(new { status, message });
                    }
                }
                status = false;
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch(Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            try
            {
                var user = HttpContext.User;
                bool status;
                string message;
                if (user.HasClaim(c => c.Type == "TokenType"))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == "TokenType").Value == _login)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                        List<NoteResponseModel> data = await _notesBusiness.GetAllNotes(UserId);
                        if (data != null && data.Count > 0)
                        {
                            status = true;
                            message = "Here is the List Of all Notes.";
                            return Ok(new { status, message, data });
                        }
                        status = false;
                        message = "No Notes Currently Present";
                        return NotFound(new { status, message });
                    }
                }
                status = false;
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> GetAllDeletedNotes()
        {
            try
            {
                var user = HttpContext.User;
                bool status;
                string message;
                if (user.HasClaim(c => c.Type == "TokenType"))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == "TokenType").Value == _login)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                        List<NoteResponseModel> data = await _notesBusiness.GetAllDeletedNotes(UserId);
                        if (data != null && data.Count > 0)
                        {
                            status = true;
                            message = "Here is the List Of all Deleted Notes.";
                            return Ok(new { status, message, data });
                        }
                        status = false;
                        message = "No Notes Currently Deleted";
                        return NotFound(new { status, message });
                    }
                }
                status = false;
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        [HttpGet]
        [Route("Archive")]
        public async Task<IActionResult> GetAllArchivedNotes()
        {
            try
            {
                var user = HttpContext.User;
                bool status;
                string message;
                if (user.HasClaim(c => c.Type == "TokenType"))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == "TokenType").Value == _login)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                        List<NoteResponseModel> data = await _notesBusiness.GetAllArchivedNotes(UserId);
                        if (data != null && data.Count > 0)
                        {
                            status = true;
                            message = "Here is the List Of all Archived Notes.";
                            return Ok(new { status, message, data });
                        }
                        status = false;
                        message = "No Notes Currently Archived";
                        return NotFound(new { status, message });
                    }
                }
                status = false;
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        [HttpGet]
        [Route("Pinned")]
        public async Task<IActionResult> GetAllPinnedNotes()
        {
            try
            {
                var user = HttpContext.User;
                bool status;
                string message;
                if (user.HasClaim(c => c.Type == "TokenType"))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == "TokenType").Value == _login)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                        List<NoteResponseModel> data = await _notesBusiness.GetAllPinnedNotes(UserId);
                        if (data != null && data.Count > 0)
                        {
                            status = true;
                            message = "Here is the List Of all Pinned Notes.";
                            return Ok(new { status, message, data });
                        }
                        status = false;
                        message = "No Notes Currently Pinned";
                        return NotFound(new { status, message });
                    }
                }
                status = false;
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        [HttpPut("{NoteId}")]
        public async Task<IActionResult> UpdateNote(int NoteId, NoteRequest updateNotesDetails)
        {
            try
            {
                var user = HttpContext.User;
                bool status;
                string message;
                if (user.HasClaim(c => c.Type == "TokenType"))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == "TokenType").Value == _login)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                        NoteResponseModel data = await _notesBusiness.UpdateNotes(NoteId, UserId, updateNotesDetails);
                        if (data != null)
                        {
                            status = true;
                            message = "Note Updated Successfull";
                            return Ok(new { status, message, data });
                        }
                        status = false;
                        message = "Note Cannot be Updated.";
                        return NotFound(new { status, message });
                    }
                }
                status = false;
                message = "Invalid Token.";
                return BadRequest(new { status, message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        [HttpDelete]
        [Route("Delete/{NoteId}")]
        public async Task<IActionResult> DeleteNote(int NoteId)
        {
            try
            {
                var user = HttpContext.User;
                bool status;
                string message;
                if (user.HasClaim(c => c.Type == "TokenType"))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == "TokenType").Value == _login)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
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
                status = false;
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        [HttpDelete]
        [Route("DeleteForever")]
        public async Task<IActionResult> DeleteNotesPermanently()
        {
            try
            {
                var user = HttpContext.User;
                bool status;
                string message;
                if (user.HasClaim(c => c.Type == "TokenType"))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == "TokenType").Value == _login)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                        status = await _notesBusiness.DeleteNotesPermanently(UserId);
                        if (status)
                        {
                            message = "Your Notes has been Permanently Deleted";
                            return Ok(new { status, message });
                        }
                        message = "No Note Present to Delete. !!";
                        return NotFound(new { status, message });
                    }
                }
                status = false;
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        [HttpPost]
        [Route("Restore/{NoteId}")]
        public async Task<IActionResult> RestoreDeletedNotes(int NoteId)
        {
            try
            {
                var user = HttpContext.User;
                bool status;
                string message;
                if (user.HasClaim(c => c.Type == "TokenType"))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == "TokenType").Value == _login)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                        status = await _notesBusiness.RestoreDeletedNotes(NoteId, UserId);
                        if (status)
                        {
                            message = "Your Notes has Been Successfully Restored";
                            return Ok(new { status, message });
                        }
                        message = "No Note Present to be Restored. !!";
                        return NotFound(new { status, message });
                    }
                }
                status = false;
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        [HttpGet]
        [Route("Reminder")]
        public async Task<IActionResult> SortByReminderNotes()
        {
            try
            {
                var user = HttpContext.User;
                bool status;
                string message;
                if (user.HasClaim(c => c.Type == "TokenType"))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == "TokenType").Value == _login)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                        List<NoteResponseModel> data = await _notesBusiness.SortByReminderNotes(UserId);
                        if (data != null && data.Count > 0)
                        {
                            status = true;
                            message = "Here is the List Of all Notes By Upcoming Reminder.";
                            return Ok(new { status, message, data });
                        }
                        status = false;
                        message = "No Notes Currently on Reminder.";
                        return NotFound(new { status, message });
                    }
                }
                status = false;
                message = "Invalid Token";
                return BadRequest(new { status, message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

    }
}