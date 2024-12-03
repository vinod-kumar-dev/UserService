using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using UserService.Interfaces;
using UserService.Services;
using UserService.ViewModels;
using System.Text.Json;
using UserService.Models;
using UserService.Helper;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _user;
        public UserController(IUser user)
        {
            _user = user;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(CreateUserModel request)
        {
            return Ok(await _user.AddUser(request));
        }
        [HttpPost("User/List")]
        public async Task<IActionResult> UserList(FilterRequest<UserRequest> request)
        {

            return Ok(await _user.GetUserList(request));
        }
        [HttpPut("User /{id}")]
        public async Task<IActionResult> UpdateUser(long id,UpdateUserModel request)
        {

            return Ok(await _user.UpdateUser(id,request));
        }
        [HttpGet("User /{id}")]
        public async Task<IActionResult> GetUserById(long id)
        {

            return Ok(await _user.GetUserById(id));
        }
        [HttpGet("Produced")]
        public async Task<IActionResult> ProduceRabbitMQMessage()
        {
            //RabbitMQHelper helper = new RabbitMQHelper();
            //await helper.ProduceMessageAsync();
            return Ok();
        }
    }
}
