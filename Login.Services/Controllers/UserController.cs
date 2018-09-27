using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Login.Services.Models;
using Microsoft.AspNetCore.Cors;
using System.Net;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Login.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserContext _context;

        public UserController(IConfiguration configuration, UserContext context)
        {

            _configuration = configuration;
            _context = context;

        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUser([FromRoute] string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var user = await _context.Users.FirstOrDefaultAsync(a => a.username == username);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public IActionResult AddSchedule([FromBody] UserClass user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            return Ok();
        }

        [HttpPut]
        public IActionResult EditSchedule([FromBody] UserClass user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _context.Users.Update(user);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            return Ok();
        }

        [HttpDelete("{id_user}")]
        public IActionResult DeleteUser(string id_user)
        {
            try
            {
                var temp = _context.Users.Find(id_user);
                _context.Users.Remove(temp);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            return Ok();

        }
    }
}