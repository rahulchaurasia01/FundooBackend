using FundooCommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FundooRepositoryLayer.Interface
{
    public interface IAdminRepository
    {

        Task<AdminResponseModel> AdminRegistration(AdminRegisterRequest registerRequest);

        AdminResponseModel AdminLogin(LoginRequest loginRequest);

        AdminStatisticsResponseModel AdminStatistics(int userId);

        List<UserList> AdminUserLists(int userId, int take, int skip);

    }
}
