using FundooCommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FundooBusinessLayer.Interface
{
    public interface IAdminBusiness
    {

        Task<UserResponseModel> AdminRegistration(RegisterRequest registerRequest);

        UserResponseModel AdminLogin(LoginRequest loginRequest);

    }
}
