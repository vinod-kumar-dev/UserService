using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using UserService.Interfaces;
using UserService.Services;
using UserService.ViewModels;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IToken _token;
       private readonly IDistributedCache _cache;
        public TokenController(IToken token, IDistributedCache cache)
        {
            _token = token;
            _cache = cache;
        }

        [HttpPost("Token")]
        public async Task<IActionResult> Token(LoginModel _login)
        {
            //string cacheKey = $"Token:{_login.Email}";
            //var cachedProduct = await _cache.GetStringAsync(cacheKey);

            //if (!string.IsNullOrEmpty(cachedProduct))
            //{
            //    var product = JsonSerializer.Deserialize<string>(cachedProduct);
            //    return Ok(product);  // Return cached product if found
            //}
            return Ok(await _token.LoginUser(_login));
        }   
    }
}
