using System.Data;
using UserAPI.Interfaces;
using UserAPI.ViewModels;
using UserAPI.Helper;
using System.Security.Claims;
using System.Text;
using UserAPI.ViewModels.Common;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using UserAPI.CustomException;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using UserAPI.Models;
namespace UserAPI.Services
{
    public class Token:IToken
    {
        public DapperContext _context;
        public readonly JWTSettings _jwtSettings;
        public readonly IDistributedCache _distributedCache;
        public Token(DapperContext dapperContext,IOptions<JWTSettings> jwtSettings,IDistributedCache distributedCache)
        {
            _context = dapperContext;
            _jwtSettings = jwtSettings.Value;
            _distributedCache = distributedCache;
        }
        public async Task<string> LoginUser(LoginModel loginModel)
        {
            var parameters = new Dictionary<string, object>
                                {
                                    { "@password",UtilityHelper.EncryptInput(loginModel.Password) },
                                    { "@email", loginModel.Email },
                                };
            int res = await _context.QuerySingleAsync<int>("User_Login", parameters, CommandType.StoredProcedure);
            if (res > 0)
            {
                var parametersUsr = new Dictionary<string, object>
                                {
                                    { "@email", loginModel.Email },
                };
                ViewUserModel userDt = await _context.QuerySingleAsync<ViewUserModel>("User_Read_ByEmail", parametersUsr, CommandType.StoredProcedure);

                string token = await GenerateToken(loginModel.Email, userDt.Id.ToString(), userDt.Role);
                //var options = new DistributedCacheEntryOptions
                //{
                //    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                //};
                //await _distributedCache.SetStringAsync($"Token:{loginModel.Email}", JsonSerializer.Serialize(token), options);
                return token;
            }
            else
            {
                throw new UnAuthorizedException("Don't have permission");
            }
            return string.Empty;
        }
        private async Task<string> GenerateToken(string userName, string userId, string role)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,userName),
                new Claim(JwtRegisteredClaimNames.Jti,userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Name,userName),
                new Claim(ClaimTypes.Role,role),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_jwtSettings.ExprationInMin)),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token); ;
        }
    }
}
