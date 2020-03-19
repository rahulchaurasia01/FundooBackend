using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FundooBusinessLayer.Interface;
using FundooCommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FundooAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly IAdminBusiness _adminBusiness;
        private readonly IConfiguration _configuration;

        private static readonly string _login = "Login";
        private static readonly string _admin = "Admin";

        private static readonly string _tokenType = "TokenType";
        private static readonly string _userType = "UserType";
        private static readonly string _userId = "UserId";

        public AdminController(IAdminBusiness adminBusiness, IConfiguration configuration)
        {
            _adminBusiness = adminBusiness;
            _configuration = configuration;
        }

        /// <summary>
        /// Api for Admin Registration
        /// </summary>
        /// <param name="registerRequest">Admin Register Data</param>
        /// <returns>If Found, It return 200 or else NotFound Response or Any Execption
        /// occured and Not Proper Input Given it return BadRequest.</returns>
        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> AdminRegistration(AdminRegisterRequest registerRequest)
        {
            try
            {
                AdminResponseModel data = await _adminBusiness.AdminRegistration(registerRequest);
                bool status = false;
                string message;
                string token;
                if (data == null)
                {
                    message = "No Data Provided";
                    return Ok(new { status, message });
                }
                else
                {
                    status = true;
                    message = "Admin Account Created Successfully";
                    token = GenerateToken(data, "Registration");
                    return Ok(new { status, message, data, token });
                }
            }
            catch(Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        /// <summary>
        /// Api For Amdin Login
        /// </summary>
        /// <param name="loginRequest">Login data</param>
        /// <returns>If Found, It return 200 or else NotFound Response or Any Execption
        /// occured and Not Proper Input Given it return BadRequest.</returns>
        [HttpPost]
        [Route("Login")]
        public IActionResult AdminLogin(LoginRequest loginRequest)
        {
            try
            {
                AdminResponseModel data = _adminBusiness.AdminLogin(loginRequest);
                bool status = false;
                string message;
                string token;
                if (data == null)
                {
                    message = "No Admin Account Present with this Email-Id and Password";
                    return Ok(new { status, message });
                }
                else
                {
                    status = true;
                    message = "Admin Successfully Logged In";
                    token = GenerateToken(data, _login);
                    return Ok(new { status, message, data, token });
                }
            }
            catch(Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        /// <summary>
        /// Get the statistics of the Regular Users
        /// </summary>
        /// <returns>If Found, It return 200 or else NotFound Response or Any Execption
        /// occured and Not Proper Input Given it return BadRequest.</returns>
        [HttpGet]
        [Route("Statistics")]
        [Authorize]
        public IActionResult AdminStatistics()
        {
            try
            {
                var user = HttpContext.User;
                bool status = false;
                string message;
                if(user.HasClaim(c => c.Type == _tokenType))
                {
                    if(user.Claims.FirstOrDefault(c => c.Type == _tokenType).Value == _login && 
                        user.Claims.FirstOrDefault(c => c.Type == _userType).Value == _admin)
                    {
                        int userId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == _userId).Value);
                        AdminStatisticsResponseModel data = _adminBusiness.AdminStatistics(userId);
                        if(data != null)
                        {
                            status = true;
                            message = "Here Is the Statistics of Regular User.";
                            return Ok(new { status, message, data });
                        }
                        message = "No Data Present.";
                        return Ok(new { status, message });
                    }
                }
                message = "Invalid Token.";
                return BadRequest(new { status, message });
            }
            catch(Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        /// <summary>
        /// Get the List of All the Regular User with there no. of Notes.
        /// </summary>
        /// <returns>If Found, It return 200 or else NotFound Response or Any Execption
        /// occured and Not Proper Input Given it return BadRequest.</returns>
        [HttpGet]
        [Route("Users")]
        [Authorize]
        public IActionResult AdminUserList(int take, int skip)
        {
            try
            {
                var user = HttpContext.User;
                bool status = false;
                string message;
                if (user.HasClaim(c => c.Type == _tokenType))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == _tokenType).Value == _login &&
                        user.Claims.FirstOrDefault(c => c.Type == _userType).Value == _admin)
                    {
                        int userId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == _userId).Value);
                        List<UserList> data = _adminBusiness.AdminUserLists(userId, take, skip);
                        if (data != null)
                        {
                            status = true;
                            message = "Here Is the List Of all the Users with there No. Of Notes.";
                            return Ok(new { status, message, data });
                        }
                        message = "No Data Present.";
                        return Ok(new { status, message });
                    }
                }
                message = "Invalid Token.";
                return BadRequest(new { status, message });
            }
            catch(Exception e)
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
        private string GenerateToken(AdminResponseModel userToken, string type)
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

    }
}