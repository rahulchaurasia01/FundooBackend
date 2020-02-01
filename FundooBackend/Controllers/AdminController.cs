using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FundooBusinessLayer.Interface;
using FundooCommonLayer.Model;
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

        public AdminController(IAdminBusiness adminBusiness, IConfiguration configuration)
        {
            _adminBusiness = adminBusiness;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> AdminRegistration(RegisterRequest registerRequest)
        {
            try
            {
                UserResponseModel data = await _adminBusiness.AdminRegistration(registerRequest);
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

        [HttpPost]
        [Route("Login")]
        public IActionResult AdminLogin(LoginRequest loginRequest)
        {
            try
            {
                UserResponseModel data = _adminBusiness.AdminLogin(loginRequest);
                bool status;
                string message;
                string token;
                if (data == null)
                {
                    status = false;
                    message = "No Admin Account Present with this Email-Id and Password";
                    return NotFound(new { status, message });
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

    }
}