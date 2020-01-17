using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FundooBusinessLayer.Interface;
using FundooCommonLayer.Model;
using FundooCommonLayer.ModelDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FundooAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserBusiness _userBusiness;
        private IConfiguration _configuration;

        public UserController(IUserBusiness userBusiness, IConfiguration configuration)
        {
            _userBusiness = userBusiness;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration([FromBody] UserDetails userDetails)
        {
            try
            {
                ResponseModel data = _userBusiness.Registration(userDetails);
                bool status;
                string message;
                string token;
                if (data == null)
                {
                    status = false;
                    message = "No Data Provided";
                    return NotFound(new { status, message });
                }
                else
                {
                    status = true;
                    message = "User Account Created Successfully";
                    token = GenerateToken(data);
                    return Ok(new {status, message, data, token });
                }
            }
            catch(Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            try
            {
                ResponseModel data = _userBusiness.Login(login);
                bool status;
                string message;
                string token;
                if (data == null)
                {
                    status = false;
                    message = "No User Present with this Email-Id and Password";
                    return NotFound(new { status, message });
                }
                else
                {
                    status = true;
                    message = "User Successfully Logged In";
                    token = GenerateToken(data);
                    return Ok(new { status, message, data, token });
                }
            }
            catch(Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }


        [HttpPost]
        [Route("ForgetPassword")]
        public IActionResult ForgetPassword([FromBody] ForgetPasswordRequest forgetPassword)
        {
            try
            {
                bool flag = _userBusiness.ForgetPassword(forgetPassword);
                bool status;
                string message;
                if(!flag)
                {
                    status = false;
                    message = "No User Found with this Email-Id: " + forgetPassword.EmailId;
                    return NotFound(new { status, message });
                }
                else
                {
                    status = true;
                    message = "An Password Reset Link has been Send to the above Email";
                    return Ok(new { status, message });
                }
                
            }
            catch(Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }


        [HttpPost]
        [Route("ResetPassword")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest resetPassword)
        {
            try
            {
                bool flag = _userBusiness.ResetPassword(resetPassword);
                bool status;
                string message;
                if (!flag)
                {
                    status = false;
                    message = "No User Found with this Token: " + resetPassword.ResetToken;
                    return NotFound(new { status, message });
                }
                else
                {
                    status = true;
                    message = "Your Password Has been Successfully Changed";
                    return Ok(new { status, message });
                }
            }
            catch(Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }




        private string GenerateToken(ResponseModel userToken)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim("UserId", userToken.UserId.ToString()),
                    new Claim("EmailId", userToken.EmailId.ToString())
                };

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Issuer"],
                    claims, expires: DateTime.Now.AddMinutes(10), signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}