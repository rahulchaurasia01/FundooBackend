using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FundooBusinessLayer.Interface;
using FundooCommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FundooAppBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {

        private static readonly string _login = "Login";

        private readonly ILabelBusiness _labelBusiness;

        public LabelController(ILabelBusiness labelBusiness)
        {
            _labelBusiness = labelBusiness;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLabel([FromBody]LabelRequest label)
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
                        LabelResponseModel data = await _labelBusiness.CreateLabel(label, UserId);
                        if (data != null)
                        {
                            status = true;
                            message = "Your Label Has been Successfully Created.";
                            return Ok(new { status, message, data });
                        }
                        status = false;
                        message = "Unable to Create the Label.";
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
        public async Task<IActionResult> GetAllLabel()
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
                        List<LabelResponseModel> data = await _labelBusiness.GetAllLabel(UserId);
                        if (data != null && data.Count > 0)
                        {
                            status = true;
                            message = "List Of All the Label.";
                            return Ok(new { status, message, data });
                        }
                        status = false;
                        message = "No Label Found.";
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

        [HttpGet("{LabelId}")]
        public async Task<IActionResult> GetNoteByLabelId(int LabelId)
        {
            try
            {
                var user = HttpContext.User;
                bool status = false;
                string message;
                if (user.HasClaim(c => c.Type == "TokenType"))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == "TokenType").Value == _login)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                        List<NoteResponseModel> data = await _labelBusiness.GetNoteByLabelId(LabelId);
                        if (data != null && data.Count > 0)
                        {
                            status = true;
                            message = "Here is the list of notes present by this label.";
                            return Ok(new { status, message, data });
                        }
                        message = "No Notes Present for this label";
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

        [HttpPut("{LabelId}")]
        public async Task<IActionResult> UpdateLabel([FromBody] LabelRequest updateLabel, int LabelId)
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
                        LabelResponseModel data = await _labelBusiness.UpdateLabel(updateLabel, LabelId);
                        if (data != null)
                        {
                            status = true;
                            message = "Label has been Updated Successfully.";
                            return Ok(new { status, message, data });
                        }
                        status = false;
                        message = "Unable to Update the label.";
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

        [HttpDelete("{LabelId}")]
        public async Task<IActionResult> DeleteLabel(int LabelId)
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
                        status = await _labelBusiness.DeleteLabel(LabelId);
                        if (status)
                        {
                            message = "Label has been Deleted Successfully.";
                            return Ok(new { status, message});
                        }
                        message = "Unable to Delete the label.";
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



    }
}