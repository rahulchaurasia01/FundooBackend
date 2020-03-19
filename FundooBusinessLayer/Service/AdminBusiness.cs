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

        public async Task<AdminResponseModel> AdminRegistration(AdminRegisterRequest registerRequest)
        {
            if (registerRequest == null)
                return null;
            else
                return await _adminRepository.AdminRegistration(registerRequest);
        }

        public AdminResponseModel AdminLogin(LoginRequest loginRequest)
        {
            if (loginRequest == null)
                return null;
            else
                return _adminRepository.AdminLogin(loginRequest);
        }

        public AdminStatisticsResponseModel AdminStatistics(int userId)
        {
            try
            {
                if (userId <= 0)
                    return null;
                else
                    return _adminRepository.AdminStatistics(userId);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<UserList> AdminUserLists(int userId, int take, int skip)
        {
            try
            {
                if (userId <= 0 || take < 0)
                    return null;
                else
                    return _adminRepository.AdminUserLists(userId, take, skip);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }


    }
}
