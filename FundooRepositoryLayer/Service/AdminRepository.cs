using FundooCommonLayer.Model;
using FundooCommonLayer.ModelDB;
using FundooRepositoryLayer.Interface;
using FundooRepositoryLayer.ModelContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooRepositoryLayer.Service
{
    public class AdminRepository : IAdminRepository
    {

        private readonly ApplicationContext _applicationContext;

        public AdminRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<UserResponseModel> AdminRegistration(RegisterRequest registerRequest)
        {
            try
            {
                registerRequest.Password = EncodeDecode.EncodePasswordToBase64(registerRequest.Password);


                var userData = new UserDetails
                {
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName,
                    EmailId = registerRequest.EmailId,
                    Password = registerRequest.Password,
                    Type = registerRequest.Type,
                    IsActive = true,
                    UserRole = "Admin",
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                };

                _applicationContext.UserDetails.Add(userData);
                await _applicationContext.SaveChangesAsync();

                var data = new UserResponseModel()
                {
                    UserId = userData.UserId,
                    FirstName = userData.FirstName,
                    LastName = userData.LastName,
                    EmailId = userData.EmailId,
                    Type = userData.Type,
                    IsActive = userData.IsActive,
                    UserRole = userData.UserRole,
                    CreatedAt = userData.CreatedAt,
                    ModifiedAt = userData.ModifiedAt
                };

                return data;

            }
            catch(Exception e)
            {
                if (e.InnerException != null)
                    throw new Exception(e.InnerException.Message);
                else
                    throw new Exception(e.Message);
            }
        }

        public UserResponseModel AdminLogin(LoginRequest loginRequest)
        {
            try
            {
                loginRequest.Password = EncodeDecode.EncodePasswordToBase64(loginRequest.Password);
                var data = _applicationContext.UserDetails.
                    FirstOrDefault(user => (user.EmailId == loginRequest.EmailId) && (user.Password == loginRequest.Password));

                if (data != null)
                {
                    var userData = new UserResponseModel()
                    {
                        UserId = data.UserId,
                        FirstName = data.FirstName,
                        LastName = data.LastName,
                        EmailId = data.EmailId,
                        Type = data.Type,
                        IsActive = data.IsActive,
                        UserRole = data.UserRole,
                        CreatedAt = data.CreatedAt,
                        ModifiedAt = data.ModifiedAt
                    };
                    return userData;
                }
                return null;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
