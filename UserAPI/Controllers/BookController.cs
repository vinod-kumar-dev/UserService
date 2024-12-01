using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using UserAPI.Interfaces;
using UserAPI.Services;
using UserAPI.ViewModels;
using System.Text.Json;
using UserAPI.Models;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBook _book;
        public BookController(IBook book)
        {
            _book = book;
        }

        [HttpPost]
        public async Task<IActionResult> Add(BookModel request)
        {

            return Ok(await _book.Add(request));
        }
        [HttpPut("User /{id}")]
        public async Task<IActionResult> Update(long id, BookModel request)
        {

            return Ok(await _book.Update(id,request));
        }
        [HttpGet("User /{id}")]
        public async Task<IActionResult> GetById(long id)
        {

            return Ok(await _book.GetById(id));
        }
    }
}
