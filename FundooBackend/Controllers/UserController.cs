using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FundooBusinessLayer.Interface;
using FundooCommonLayer.Model;
using FundooCommonLayer.ModelDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace FundooAppBackend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserBusiness _userBusiness;
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _distributed;

        private static readonly string _forgetPassword = "ForgetPassword";
        private static readonly string _login = "Login";

        private static readonly string _regularUser = "Regular User";

        private static readonly string _tokenType = "TokenType";
        private static readonly string _userType = "UserType";
        private static readonly string _userId = "UserId";

        public UserController(IUserBusiness userBusiness, IConfiguration configuration)
        {
            _userBusiness = userBusiness;
            _configuration = configuration;
            //_distributed = distributedCache;
        }

        /// <summary>
        /// Notification Token
        /// </summary>
        /// <param name="notificationRequest">Notification data</param>
        /// <returns>If Found, It return 200 or else NotFound Response or Any Execption
        /// occured and Not Proper Input Given it return BadRequest.</returns>
        [HttpPost]
        [Authorize]
        [Route("Notification")]
        public async Task<IActionResult> AddNotification(NotificationRequest notificationRequest)
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
                        int userId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                        status = await _userBusiness.AddNotification(notificationRequest, userId);
                        if (status)
                        {
                            message = "Notification Token Added Successfully.";
                            return Ok(new { status, message });
                        }
                        message = "Unable to Add Notification Token";
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
                bool status = false;
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
                        message = "No Such User is Present";
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
        /// Profile Pic Of User.
        /// </summary>
        /// <param name="userRequest">Profile Pic Data</param>
        /// <returns>If Found, It return 200 or else NotFound Response or Any Execption
        /// occured and Not Proper Input Given it return BadRequest.</returns>
        [HttpPut]
        [Authorize]
        [Route("ProfilePic")]
        public async Task<IActionResult> AddUpdateProfilePic([FromForm] IFormFile file)
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

                        if (file.Length <= 0)
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

                            UserResponseModel data = await _userBusiness.AddUpdateProfilePic(imageRequest1, UserId);
                            if (data != null)
                            {
                                status = true;
                                message = "The Image has Been Successfully Added To the Profile Pic.";
                                return Ok(new { status, message, data });
                            }
                            message = "Unable to Add the Image to the Profile Pic.";
                            return Ok(new { status, message });

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
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        [HttpPost]
        [Route("ReminderNotification")]
        public async Task<IActionResult> CheckForReminderNotificationAsync()
        {
            try
            {
                bool status = false;
                string message;
                DateTime currentTime = DateTime.Now;
                DateTime EndTime = currentTime.AddHours(1);

                List<ReminderNotificationResponseModel> data = _userBusiness.ReminderNotification(currentTime, EndTime);

                if (data != null && data.Count > 0)
                    await SendNotification(data);
                message = "No Notes present to Send The Notification";
                return Ok(new { status, message });
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
                if (!ValidateRegisterRequest(userDetails))
                    return BadRequest(new { Message = "Enter Proper Data" });

                UserResponseModel data = await _userBusiness.Registration(userDetails);
                bool status = false;
                string message;
                string token;
                if (data == null)
                {
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
                    return BadRequest(new { Message = "Enter Proper Input Value." });

                UserResponseModel data = _userBusiness.Login(login);
                bool status = false;
                string message;
                string token;
                if (data == null)
                {
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
                bool status = false;
                string message;
                string token;
                if (data == null)
                {
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
                bool status = false;
                string message;
                if (user.HasClaim(c => c.Type == "TokenType"))
                {
                    if (user.Claims.FirstOrDefault(c => c.Type == "TokenType").Value == _forgetPassword)
                    {
                        int UserId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                        status = await _userBusiness.ResetPassword(resetPassword, UserId);
                        if (status)
                        {
                            status = true;
                            message = "Your Password Has been Successfully Changed";
                            return Ok(new { status, message });
                        }
                        message = "Unable to Change the Password.";
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
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        private async Task<IActionResult> SendNotification(List<ReminderNotificationResponseModel> reminders)
        {
            int count = 0;
            bool status = false;
            string message;
            string serverKey = _configuration["Firebase:Server_Key"];
            string senderId = _configuration["Firebase:SenderId"];
            string fcmLink = "https://fcm.googleapis.com/fcm/send";


            foreach (ReminderNotificationResponseModel reminder in reminders)
            {
                var notificationData = new NotificationModel()
                {
                    Notification = new NoteModel()
                    {
                        Title = reminder.Title,
                        Body = reminder.Desciption
                    },
                    To = reminder.Token
                };


                //var httpWebRequest = (HttpWebRequest)WebRequest.Create(fcmLink);
                //httpWebRequest.ContentType = "application/json";
                //httpWebRequest.Headers.Add(string.Format("Authorization: Key={0}", serverKey));
                //httpWebRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                //httpWebRequest.Method = "POST";


                //var json = JsonConvert.SerializeObject(notificationData);
                //var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                //var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                string jsonMessage = JsonConvert.SerializeObject(notificationData);
                var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, fcmLink);

                request.Headers.TryAddWithoutValidation("Authorization", "key=" + serverKey);
                request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                HttpResponseMessage result;

                using(var client = new HttpClient())
                {
                    result = await client.SendAsync(request);
                }

                //using (var client = new HttpClient())
                //{

                //    client.BaseAddress = new Uri("https://fcm.googleapis.com/");
                //    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"key={serverKey}");
                //    client.DefaultRequestHeaders.TryAddWithoutValidation("Sender", $"id={senderId}");

                //    var json = JsonConvert.SerializeObject(notificationData);
                //    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");


                //    //var request = new HttpRequestMessage
                //    //{
                //    //    RequestUri = new Uri("https://fcm.googleapis.com/fcm/send"),
                //    //    Method = System.Net.Http.HttpMethod.Post,
                //    //    //Headers =
                //    //    //{
                //    //    //    { HttpRequestHeader.Authorization.ToString(), _configuration["Firebase:Server_Key"] },
                //    //    //    { HttpRequestHeader.ContentType.ToString(), "application/json" }
                //    //    //},
                //    //    Content = new StringContent(JsonConvert.SerializeObject(notificationData))
                //    //};

                //    //request.Headers.Add("Content-Type", "application/json");
                //    //request.Headers.TryAddWithoutValidation("Authorization", _configuration["Firebase:Server_Key"]);

                //    HttpResponseMessage httpResponse = await client.PostAsync("fcm/send", httpContent);

                //    if (httpResponse.IsSuccessStatusCode)
                //        count++;
                //}

            }

            if (count == reminders.Count)
            {
                status = true;
                message = "Reminder Notification Send Successfully";
                return Ok(new { status, message });
            }
            else
            {
                message = "Unable to Send Notification";
                return Ok(new { status, message });
            }

        }
    }
}