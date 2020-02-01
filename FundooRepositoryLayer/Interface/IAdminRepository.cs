using FundooCommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FundooRepositoryLayer.Interface
{
    public interface IAdminRepository
    {

        Task<UserResponseModel> AdminRegistration(RegisterRequest registerRequest);

        UserResponseModel AdminLogin(LoginRequest loginRequest);

    }
}
