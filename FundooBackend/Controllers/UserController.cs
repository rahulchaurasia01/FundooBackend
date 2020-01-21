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
using Microsoft.AspNetCore.Authorization;
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
        private readonly IConfiguration _configuration;

        private static readonly string _forgetPassword = "ForgetPassword";
        private static readonly string _login = "Login";


        public UserController(IUserBusiness userBusiness, IConfiguration configuration)
        {
            _userBusiness = userBusiness;
            _configuration = configuration;
        }

        /// <summary>
        /// Api for Registration
        /// </summary>
        /// <param name="userDetails">User Detials Model</param>
        /// <returns>It return 200 and Data If registration is Successfull or else 404</returns>
        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration([FromBody] UserDetails userDetails)
        {
            try
            {
                UserResponseModel data = _userBusiness.Registration(userDetails);
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
                    token = GenerateToken(data, "Registration");
                    return Ok(new { status, message, data, token });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        /// <summary>
        /// Api For Login
        /// </summary>
        /// <param name="login">Login Model</param>
        /// <returns>It return 200 and Data If Login is Successfull or else 404</returns>
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            try
            {
                UserResponseModel data = _userBusiness.Login(login);
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
                    token = GenerateToken(data, _login);
                    return Ok(new { status, message, data, token });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        /// <summary>
        /// Api for ForgetPassword
        /// </summary>
        /// <param name="forgetPassword">Forget Password Model</param>
        /// <returns>It Return 200 Reponse if sending mail to the user is successfull or else 404 not Found</returns>
        [HttpPost]
        [Route("ForgetPassword")]
        public IActionResult ForgetPassword([FromBody] ForgetPasswordRequest forgetPassword)
        {
            try
            {
                UserResponseModel data = _userBusiness.ForgetPassword(forgetPassword);
                bool status;
                string message;
                string token;
                if (data == null)
                {
                    status = false;
                    message = "No User Found with this Email-Id: " + forgetPassword.EmailId;
                    return NotFound(new { status, message });
                }
                else
                {
                    token = GenerateToken(data, _forgetPassword);
                    MsmqSender.SendToMsmq(token);
                    status = MsmqReceiver.ReceiveMsmq(forgetPassword);
                    if (status)
                    {
                        message = "An Password Reset Link has been Send to the above Email";
                        return Ok(new { status, message, token });
                    }
                    else
                    {
                        message = "Unable to Send the Email.";
                        return NotFound(new { status, message });
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        /// <summary>
        /// Api for Reseting the Password
        /// </summary>
        /// <param name="resetPassword">Reset Password Model</param>
        /// <returns>It return 200 Response, If Reset is Successfull or else 404 Not Found</returns>
        [HttpPost]
        [Authorize]
        [Route("ResetPassword")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest resetPassword)
        {
            try
            {
                var user = HttpContext.User;
                bool status;
                string message;
                if (user.HasClaim(c => c.Type == "TokenType"))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == "TokenType").Value == _forgetPassword)
                    {
                        resetPassword.UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                        status = _userBusiness.ResetPassword(resetPassword);
                        if(status)
                        {
                            status = true;
                            message = "Your Password Has been Successfully Changed";
                            return Ok(new { status, message });
                        }
                    }
                }
                status = false;
                message = "Invalid Token.";
                return NotFound(new { status, message });
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }


        /// <summary>
        /// It Generate the token.
        /// </summary>
        /// <param name="userToken">Response Model</param>
        /// <param name="type">Token Type</param>
        /// <returns>it return Token</returns>
        private string GenerateToken(UserResponseModel userToken, string type)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim("UserId", userToken.UserId.ToString()),
                    new Claim("EmailId", userToken.EmailId.ToString()),
                    new Claim("TokenType", type)
                };

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Issuer"],
                    claims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    
    }
}