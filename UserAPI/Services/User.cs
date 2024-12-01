using System.Data;
using UserService.Interfaces;
using UserService.ViewModels;
using UserService.Helper;
using System.Security.Claims;
using System.Text;
using UserService.ViewModels.Common;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using UserService.CustomException;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using UserService.Models;
namespace UserService.Services
{
    public class User:IUser
    {
        public DapperContext _context;
        public readonly JWTSettings _jwtSettings;
        public readonly IDistributedCache _distributedCache;
        public User(DapperContext dapperContext,IOptions<JWTSettings> jwtSettings,IDistributedCache distributedCache)
        {
            _context = dapperContext;
            _jwtSettings = jwtSettings.Value;
            _distributedCache = distributedCache;
        }
        public async Task<long> AddUser(CreateUserModel rquest)
        {
            var parameters = new Dictionary<string, object>
                                {
                                    { "@passwordHash",UtilityHelper.EncryptInput(rquest.PasswordHash) },
                                    { "@emailId", rquest.EmailId },
                                    { "@firstName", rquest.FirstName },
                                    { "@lastName", rquest.LastName },
                                    { "@role", rquest.Role },
                                     { "@phoneNum", rquest.PhoneNum },
                                };
         return await _context.QuerySingleAsync<long>("User_Add", parameters, CommandType.StoredProcedure);
        }
        public async Task<long> UpdateUser(long id,UpdateUserModel rquest)
        {
            var parameters = new Dictionary<string, object>
                                {
                                    { "@id", id },
                                    { "@firstName", rquest.FirstName },
                                    { "@lastName", rquest.LastName },
                                     { "@phoneNum", rquest.PhoneNum },
                                };
            return await _context.QuerySingleAsync<long>("User_Update", parameters, CommandType.StoredProcedure);
        }
        public async Task<ViewUserModel> GetUserById(long id)
        {
            var parameters = new Dictionary<string, object>
                                {
                                    { "@id", id }
                                };
            return await _context.QuerySingleAsync<ViewUserModel>("User_Read_ById", parameters, CommandType.StoredProcedure);
        }
        public async Task<List<ViewUserModel>> GetUserList(FilterRequest<UserRequest> request)
        {
            var parameters = new Dictionary<string, object>
                                {
                                  { "@sortType", request.SortType },
                                  { "@sortBy", request.SortBy },
                                  { "@pageSize", request.PageSize },
                                  { "@pageNum", request.PageNum },
                                  { "@role", request.AdditionalData.Role }
                                };
            var res = await _context.QueryAsync<ViewUserModel>("User_Read_All", parameters, CommandType.StoredProcedure);
            return res.ToList();
        }
    }
}
