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

        private static readonly string _admin = "Admin";
        private static readonly string _regularUser = "Regular User";

        private static readonly string _basicUser = "Basic";
        private static readonly string _advanceUser = "Advanced";

        public AdminRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        /// <summary>
        /// It Register the New Admin
        /// </summary>
        /// <param name="registerRequest">Admin Data</param>
        /// <returns>User Response Model</returns>
        public async Task<AdminResponseModel> AdminRegistration(AdminRegisterRequest registerRequest)
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
                    IsActive = true,
                    UserRole = _admin,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                };

                _applicationContext.UserDetails.Add(userData);
                await _applicationContext.SaveChangesAsync();

                var data = new AdminResponseModel()
                {
                    UserId = userData.UserId,
                    FirstName = userData.FirstName,
                    LastName = userData.LastName,
                    EmailId = userData.EmailId,
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

        /// <summary>
        /// It Login the Admin
        /// </summary>
        /// <param name="loginRequest">Login Data</param>
        /// <returns>User Response Model</returns>
        public AdminResponseModel AdminLogin(LoginRequest loginRequest)
        {
            try
            {
                loginRequest.Password = EncodeDecode.EncodePasswordToBase64(loginRequest.Password);
                var data = _applicationContext.UserDetails.
                    FirstOrDefault(user => (user.EmailId == loginRequest.EmailId) &&
                        (user.Password == loginRequest.Password) && user.UserRole == _admin);

                if (data != null)
                {
                    var userData = new AdminResponseModel()
                    {
                        UserId = data.UserId,
                        FirstName = data.FirstName,
                        LastName = data.LastName,
                        EmailId = data.EmailId,
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

        /// <summary>
        /// It Gives the Statistics Of Basic and Advanced User.
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Admin Statistics Model</returns>
        public AdminStatisticsResponseModel AdminStatistics(int userId)
        {
            try
            {
                UserDetails user = _applicationContext.UserDetails.
                    FirstOrDefault(usr => usr.UserId == userId && usr.UserRole == _admin);

                if(user != null)
                {
                    AdminStatisticsResponseModel adminStatistics = new AdminStatisticsResponseModel
                    {
                        Basic = _applicationContext.UserDetails.
                        Where(usr => usr.UserRole == _regularUser && usr.Type == _basicUser).Count(),

                        Advanced = _applicationContext.UserDetails.
                        Where(usr => usr.UserRole == _regularUser && usr.Type == _advanceUser).Count()
                    };

                    return adminStatistics;

                }

                return null;

            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It Gives the list of all the Users With the No. of Notes Each User Has.
        /// </summary>
        /// <param name="userId">Admin User Id</param>
        /// <returns>List Of Users With there No. Of Notes.</returns>
        public AdminUserListResponseModel AdminUserLists(int userId, int take, int skip)
        {
            try
            {
                UserDetails userDetails = _applicationContext.UserDetails.
                    FirstOrDefault(user => user.UserId == userId && user.UserRole == _admin);

                if (userDetails != null)
                {
                    List<UserList> adminUserLists = _applicationContext.UserDetails.
                        Where(user => user.UserRole == _regularUser).
                        Select(user => new UserList
                        {
                            UserId = user.UserId,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            EmailId = user.EmailId,
                            Service = user.Type
                        }).
                        ToList();

                    if (take > adminUserLists.Count)
                        return null;

                    foreach(UserList adminUserList in adminUserLists)
                    {
                        adminUserList.Notes = _applicationContext.NotesDetails.
                            Where(note => note.UserId == adminUserList.UserId).Count();
                    }

                    if (skip == 0)
                        skip = 10;

                    int end=take+skip;

                    if(end >= adminUserLists.Count)
                        end = adminUserLists.Count - take;

                    AdminUserListResponseModel userListResponseModel = new AdminUserListResponseModel();

                    userListResponseModel.records = adminUserLists.GetRange(take, end);

                    return userListResponseModel;

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
