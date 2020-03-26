using FundooAppBackend.Controllers;
using FundooBusinessLayer.Interface;
using FundooBusinessLayer.Service;
using FundooCommonLayer.Model;
using FundooRepositoryLayer.Interface;
using FundooRepositoryLayer.ModelContext;
using FundooRepositoryLayer.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using Xunit;

namespace FundooUnitTest
{
    public class UserAccountUnitTest
    {

        private readonly IUserBusiness _userBusiness;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public static DbContextOptions<ApplicationContext> DbContext { get;  }

        public static string sqlConnection = "server=.; database=FundooDB; Integrated Security=true";


        static UserAccountUnitTest()
        {
            DbContext = new DbContextOptionsBuilder<ApplicationContext>().UseSqlServer(sqlConnection).Options;
        }

        public UserAccountUnitTest(IHostingEnvironment hostingEnvironment)
        {
            var context = new ApplicationContext(DbContext);
            _userRepository = new UserRepository(context);
            _userBusiness = new UserBusiness(_userRepository);
            _hostingEnvironment = hostingEnvironment;

            IConfigurationBuilder configuration = new ConfigurationBuilder();

            configuration.AddJsonFile("appsettings.json");
            _configuration = configuration.Build();

        }

        #region Login User

        [Fact]
        public void Task_LoginUser_ValidLoginData_Return_OkResult()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var Logindata = new LoginRequest
            {
                EmailId = "abc4508@gmail.com",
                Password = "123456789"
            };

            var data = controller.Login(Logindata);

            Assert.IsType<OkObjectResult>(data);

        }

        [Fact]
        public void Task_LoginUser_InvalidLoginData_Return_NotFoundResult()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var Logindata = new LoginRequest
            {
                EmailId = "Lucy@hotmail.com",
                Password = "123456789"
            };

            var data = controller.Login(Logindata);

            Assert.IsType<NotFoundObjectResult>(data);
        }

        [Fact]
        public void Task_LoginUser_PasswordEmpty_Return_BadRequest()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var Logindata = new LoginRequest
            {
                EmailId = "Lucy@hotmail.com",
                Password = ""
            };

            var data = controller.Login(Logindata);

            Assert.IsType<BadRequestObjectResult>(data);
        }

        [Fact]
        public void Task_LoginUser_EmailEmpty_Return_BadRequest()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var Logindata = new LoginRequest
            {
                EmailId = "",
                Password = "123456789"
            };

            var data = controller.Login(Logindata);

            Assert.IsType<BadRequestObjectResult>(data);
        }

        [Fact]
        public void Task_LoginUser_Email_At_Return_BadRequest()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var Logindata = new LoginRequest
            {
                EmailId = "ashckahotmail.com",
                Password = "123456789"
            };

            var data = controller.Login(Logindata);

            Assert.IsType<BadRequestObjectResult>(data);
        }

        [Fact]
        public void Task_LoginUser_Email_Dot_Return_BadRequest()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var Logindata = new LoginRequest
            {
                EmailId = "abscuka@hotmailcom",
                Password = "123456789"
            };

            var data = controller.Login(Logindata);

            Assert.IsType<BadRequestObjectResult>(data);
        }

        [Fact]
        public void Task_LoginUser_Password_L5_Return_BadRequest()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var Logindata = new LoginRequest
            {
                EmailId = "rasvja92@hotmail.com",
                Password = "1239"
            };

            var data = controller.Login(Logindata);

            Assert.IsType<BadRequestObjectResult>(data);
        }

        [Fact]
        public void Task_LoginUser_Null_Return_BadRequest()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            LoginRequest Logindata = null;

            var data = controller.Login(Logindata);

            Assert.IsType<BadRequestObjectResult>(data);
        }

        #endregion

        #region Register User

        [Fact]
        public async void Task_RegisterUser_Return_OkResult()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var newUserData = new RegisterRequest
            {
                FirstName = "Lucy",
                LastName = "Nelson",
                EmailId = "lucynelson2014@gmail.com",
                Password = "789456123",
                Type = "Basic"
            };

            var data = await controller.Registration(newUserData);

            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_RegisterUser_Null_Return_BadRequest()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            RegisterRequest newUserData = null;

            var data = await controller.Registration(newUserData);

            Assert.IsType<BadRequestObjectResult>(data);
        }

        [Fact]
        public async void Task_RegisterUser_AllEmptyField_Return_BadRequest()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var newUserData = new RegisterRequest
            {
                FirstName = "",
                LastName = "",
                EmailId = "",
                Password = "",
                Type = ""
            };

            var data = await controller.Registration(newUserData);

            Assert.IsType<BadRequestObjectResult>(data);
        }

        [Fact]
        public async void Task_RegisterUser_FN_L3_Return_BadRequest()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var newUserData = new RegisterRequest
            {
                FirstName = "sa",
                LastName = "nelson",
                EmailId = "lucynelson12@gmail.com",
                Password = "789456123",
                Type = "Basic"
            };

            var data = await controller.Registration(newUserData);

            Assert.IsType<BadRequestObjectResult>(data);
        }

        [Fact]
        public async void Task_RegisterUser_FN_G12_Return_BadRequest()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var newUserData = new RegisterRequest
            {
                FirstName = "LucySonu",
                LastName = "Nelson",
                EmailId = "lucynelson12@gmail.com",
                Password = "789456123",
                Type = "Basic"
            };

            var data = await controller.Registration(newUserData);

            Assert.IsType<BadRequestObjectResult>(data);
        }

        [Fact]
        public async void Task_RegisterUser_LN_L3_Return_BadRequest()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var newUserData = new RegisterRequest
            {
                FirstName = "Lucy",
                LastName = "ne",
                EmailId = "lucynelson12@gmail.com",
                Password = "789456123",
                Type = "Basic"
            };

            var data = await controller.Registration(newUserData);

            Assert.IsType<BadRequestObjectResult>(data);
        }

        [Fact]
        public async void Task_RegisterUser_LN_G12_Return_BadRequest()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var newUserData = new RegisterRequest
            {
                FirstName = "Lucy",
                LastName = "hilavuytygh",
                EmailId = "lucynelson12@gmail.com",
                Password = "789456123",
                Type = "Basic"
            };

            var data = await controller.Registration(newUserData);

            Assert.IsType<BadRequestObjectResult>(data);
        }

        [Fact]
        public async void Task_RegisterUser_Email_At_Return_BadRequest()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var newUserData = new RegisterRequest
            {
                FirstName = "Lucy",
                LastName = "Nelson",
                EmailId = "lucynelson12gmail.com",
                Password = "789456123",
                Type = "Basic"
            };

            var data = await controller.Registration(newUserData);

            Assert.IsType<BadRequestObjectResult>(data);
        }

        [Fact]
        public async void Task_RegisterUser_Email_Dot_Return_BadRequest()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var newUserData = new RegisterRequest
            {
                FirstName = "Lucy",
                LastName = "Nelson",
                EmailId = "lucynelson12@gmailcom",
                Password = "789456123",
                Type = "Basic"
            };

            var data = await controller.Registration(newUserData);

            Assert.IsType<BadRequestObjectResult>(data);
        }

        [Fact]
        public async void Task_RegisterUser_EmailRepeated_Return_BadRequest()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var newUserData = new RegisterRequest
            {
                FirstName = "Lucy",
                LastName = "Nelson",
                EmailId = "lucynelson12@gmail.com",
                Password = "789456123",
                Type = "Basic"
            };

            var data = await controller.Registration(newUserData);

            Assert.IsType<BadRequestObjectResult>(data);
        }

        [Fact]
        public async void Task_RegisterUser_Password_L5_Return_BadRequest()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var newUserData = new RegisterRequest
            {
                FirstName = "Lucy",
                LastName = "Nelson",
                EmailId = "lucynelson12@gmail.com",
                Password = "7894",
                Type = "Basic"
            };

            var data = await controller.Registration(newUserData);

            Assert.IsType<BadRequestObjectResult>(data);
        }

        [Fact]
        public async void Task_RegisterUser_NotDefinedType__Return_BadRequest()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var newUserData = new RegisterRequest
            {
                FirstName = "Lucy",
                LastName = "Nelson",
                EmailId = "lucynelson12@gmail.com",
                Password = "789456123",
                Type = "asdas"
            };

            var data = await controller.Registration(newUserData);

            Assert.IsType<BadRequestObjectResult>(data);
        }

        #endregion

        #region ForgetPassword

        [Fact]
        public void Task_ForgetPassword_ValidEmailData_Return_OkResult()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var forgetPassword = new ForgetPasswordRequest
            {
                EmailId = "asdjcjna2@hotmail.com"
            };

            var data = controller.ForgetPassword(forgetPassword);

            Assert.IsType<OkObjectResult>(data);

        }

        [Fact]
        public void Task_ForgetPassword_InValidEmailData_Return_NotFoundResult()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var forgetPassword = new ForgetPasswordRequest
            {
                EmailId = "holaAdios@hotmail.com"
            };

            var data = controller.ForgetPassword(forgetPassword);

            Assert.IsType<NotFoundObjectResult>(data);

        }

        [Fact]
        public void Task_ForgetPassword_Null_Return_BadRequest()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            ForgetPasswordRequest forgetPassword = null;

            var data = controller.ForgetPassword(forgetPassword);

            Assert.IsType<BadRequestObjectResult>(data);

        }

        [Fact]
        public void Task_ForgetPassword_EmailEmpty_Return_BadRequest()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var forgetPassword = new ForgetPasswordRequest
            {
                EmailId = ""
            };

            var data = controller.ForgetPassword(forgetPassword);

            Assert.IsType<BadRequestObjectResult>(data);

        }

        [Fact]
        public void Task_ForgetPassword_Email_At_Return_BadRequest()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var forgetPassword = new ForgetPasswordRequest
            {
                EmailId = "asdnjasdk@gmail.com"
            };

            var data = controller.ForgetPassword(forgetPassword);

            Assert.IsType<BadRequestObjectResult>(data);

        }

        [Fact]
        public void Task_ForgetPassword_Email_Dot_Return_BadRequest()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var forgetPassword = new ForgetPasswordRequest
            {
                EmailId = "asdjnkmvn@gmailcom"
            };

            var data = controller.ForgetPassword(forgetPassword);

            Assert.IsType<BadRequestObjectResult>(data);

        }

        #endregion

        #region ResetPassword

        [Fact]
        public async void Task_ResetPassword_ValidPassword_Return_OkResult()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var resetPassword = new ResetPasswordRequest
            {
                Password = "789456123"
            };

            var data = await controller.ResetPassword(resetPassword);

            Assert.IsType<OkObjectResult>(data);

        }

        [Fact]
        public async void Task_ResetPassword_Password_L5_Return_BadRequest()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var resetPassword = new ResetPasswordRequest
            {
                Password = "7885"
            };

            var data = await controller.ResetPassword(resetPassword);

            Assert.IsType<BadRequestObjectResult>(data);

        }

        [Fact]
        public async void Task_ResetPassword_Null_Return_BadRequest()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            ResetPasswordRequest resetPassword = null;

            var data = await controller.ResetPassword(resetPassword);

            Assert.IsType<BadRequestObjectResult>(data);

        }

        [Fact]
        public async void Task_ResetPassword_PasswordEmpty_Return_BadRequest()
        {
            var controller = new UserController(_userBusiness, _configuration, _hostingEnvironment);
            var resetPassword = new ResetPasswordRequest
            {
                Password = ""
            };

            var data = await controller.ResetPassword(resetPassword);

            Assert.IsType<BadRequestObjectResult>(data);

        }


        #endregion

    }
}
