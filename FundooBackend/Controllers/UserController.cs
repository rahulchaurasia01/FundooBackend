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
        /// List Of User.
        /// </summary>
        /// <param name="userRequest">User Request data</param>
        /// <returns>If Found, It return 200 or else NotFound Response or Any Execption
        /// occured and Not Proper Input Given it return BadRequest.</returns>
        [HttpPost]
        [Authorize]
        [Route("Users")]
        public async Task<IActionResult> GetAllUsers(UserRequest userRequest)
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
                        int userId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                        List<UserListResponseModel> data = await _userBusiness.GetAllUsers(userRequest, userId);
                        if (data != null && data.Count > 0)
                        {
                            status = true;
                            message = "Here is the List Of all User.";
                            return Ok(new { status, message, data });
                        }
                        status = false;
                        message = "No Such User is Present";
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

        /// <summary>
        /// Api for Registration
        /// </summary>
        /// <param name="userDetails">User Detials Model</param>
        /// <returns>It return 200 and Data If registration is Successfull or else 404</returns>
        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Registration([FromBody] RegisterRequest userDetails)
        {
            try
            {
                if(!ValidateRegisterRequest(userDetails))
                    return BadRequest(new { Message = "Enter Proper Data" });

                UserResponseModel data = await _userBusiness.Registration(userDetails);
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
                if (!ValidateLoginRequest(login))
                    return BadRequest(new { Message= "Enter Proper Input Value." });

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
                if (!ValidateForgetPasswordRequest(forgetPassword))
                    return BadRequest(new { Message = "Please input the Data Properly." });

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
                    MsmqSender.SendToMsmq(forgetPassword.EmailId, token);
                    status = true;
                    message = "An Password Reset Link has been Send to the above Email";
                    return Ok(new { status, message, token });
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
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPassword)
        {
            try
            {
                if (!ValidateResetPasswordRequest(resetPassword))
                    return BadRequest(new { Message = "Input the Data Properly" });

                var user = HttpContext.User;
                bool status;
                string message;
                if (user.HasClaim(c => c.Type == "TokenType"))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == "TokenType").Value == _forgetPassword)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                        status = await _userBusiness.ResetPassword(resetPassword, UserId);
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
                    new Claim("TokenType", type),
                    new Claim("UserType", userToken.UserRole.ToString())
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

        /// <summary>
        /// It Validate the RegisterRequest Model Data Before passing on to Business Layer
        /// </summary>
        /// <param name="userDetails">New User Data</param>
        /// <returns>Return true if validation successfull or else false</returns>
        private bool ValidateRegisterRequest(RegisterRequest userDetails)
        {
            if (userDetails == null || string.IsNullOrWhiteSpace(userDetails.FirstName) ||
                    string.IsNullOrWhiteSpace(userDetails.LastName) || string.IsNullOrWhiteSpace(userDetails.EmailId) ||
                    string.IsNullOrWhiteSpace(userDetails.Password) || string.IsNullOrWhiteSpace(userDetails.Type) ||
                    (userDetails.FirstName.Length < 3 || userDetails.FirstName.Length > 12) ||
                    (userDetails.LastName.Length < 3 || userDetails.LastName.Length > 12) || !userDetails.EmailId.Contains('@') ||
                    !userDetails.EmailId.Contains('.') || userDetails.Password.Length < 5 || (userDetails.Type != "Basic" &&
                    userDetails.Type != "Advanced"))
                return false;
            else
                return true;
        }

        /// <summary>
        /// It Validate The LoginRequest Model Data Before Passing it to Business Layer.
        /// </summary>
        /// <param name="loginRequest">Login Data</param>
        /// <returns>Return True If validation is successfull or else false</returns>
        private bool ValidateLoginRequest(LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.EmailId) ||
                string.IsNullOrWhiteSpace(loginRequest.Password) || !loginRequest.EmailId.Contains('@') ||
                !loginRequest.EmailId.Contains('.') || loginRequest.Password.Length < 5)
                return false;

            return true;
        }

        /// <summary>
        /// It Validate the ForgetPasswordRequest Model Before Sending it to Business Layer
        /// </summary>
        /// <param name="forgetPassword">ForgetPassword Data</param>
        /// <returns>Return True if Validation is successfull.</returns>
        private bool ValidateForgetPasswordRequest(ForgetPasswordRequest forgetPassword)
        {
            if (forgetPassword == null || string.IsNullOrWhiteSpace(forgetPassword.EmailId) ||
                !forgetPassword.EmailId.Contains('@') || !forgetPassword.EmailId.Contains('.'))
                return false;

            return true;
        }

        /// <summary>
        /// It Validate the ResetPasswordRequest Model Before Sending it to a Business Layer.
        /// </summary>
        /// <param name="resetPassword">Reset Password Data</param>
        /// <returns>Return True if Validation is Successfull or else return false</returns>
        private bool ValidateResetPasswordRequest(ResetPasswordRequest resetPassword)
        {
            if (resetPassword == null || string.IsNullOrWhiteSpace(resetPassword.Password) ||
                resetPassword.Password.Length < 5)
                return false;

            return true;
        }


    }
}