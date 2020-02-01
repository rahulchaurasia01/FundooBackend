using FundooBusinessLayer.Interface;
using FundooCommonLayer.Model;
using FundooRepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FundooBusinessLayer.Service
{
    public class AdminBusiness : IAdminBusiness
    {

        private readonly IAdminRepository _adminRepository;

        public AdminBusiness(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<UserResponseModel> AdminRegistration(RegisterRequest registerRequest)
        {
            if (registerRequest == null)
                return null;
            else
                return await _adminRepository.AdminRegistration(registerRequest);
        }

        public UserResponseModel AdminLogin(LoginRequest loginRequest)
        {
            if (loginRequest == null)
                return null;
            else
                return _adminRepository.AdminLogin(loginRequest);
        }

    }
}
