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
    public class Book:IBook
    {
        public DapperContext _context;
        public readonly JWTSettings _jwtSettings;
        public readonly IDistributedCache _distributedCache;
        public Book(DapperContext dapperContext,IOptions<JWTSettings> jwtSettings,IDistributedCache distributedCache)
        {
            _context = dapperContext;
            _jwtSettings = jwtSettings.Value;
            _distributedCache = distributedCache;
        }
        public async Task<long> Add(BookModel rquest)
        {
            var parameters = new Dictionary<string, object>
                                {
                                    { "@name", rquest.Name },
                                    { "@author", rquest.Author },
                                    { "@price", rquest.Price },
                                     { "@unit", rquest.Unit },
                                };
         return await _context.QuerySingleAsync<long>("Book_Add_Update", parameters, CommandType.StoredProcedure);
        }
        public async Task<long> Update(long id, BookModel rquest)
        {
            var parameters = new Dictionary<string, object>
                                {
                                    { "@id", id },
                                  { "@name", rquest.Name },
                                    { "@author", rquest.Author },
                                    { "@price", rquest.Price },
                                     { "@unit", rquest.Unit },
                                };
            return await _context.QuerySingleAsync<long>("Book_Add_Update", parameters, CommandType.StoredProcedure);
        }
        public async Task<ViewBookModel> GetById(long id)
        {
            var parameters = new Dictionary<string, object>
                                {
                                    { "@id", id }
                                };
            return await _context.QuerySingleAsync<ViewBookModel>("Book_Read_ById", parameters, CommandType.StoredProcedure);
        }
    }
}
