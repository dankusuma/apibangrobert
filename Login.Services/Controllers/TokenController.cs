using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using Login.Services.Models;

namespace Auth.Services.Controllers
{
    [EnableCors("SiteCorsPolicy")]
    [Route("api/[controller]")]
    public class TokenController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly UserContext _context;

        public TokenController(IConfiguration configuration, UserContext context)
        {

            _configuration = configuration;
            _context = context;

        }

        // GET: api/token
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody]LoginModelClass login)
        {

            if (ModelState.IsValid && login != null)
            {
                var user = _context.Users.FirstOrDefault(m => m.username == login.username);
                if (login.password != user.password)
                {
                    return Unauthorized();
                }
                else
                {
                    var claims = new[]
                    {
                            new Claim(JwtRegisteredClaimNames.Sub, login.username),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                    var token = new JwtSecurityToken
                    (
                        issuer: _configuration["Token:Issuer"],
                        audience: _configuration["Token:Audience"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddDays(30),
                        notBefore: DateTime.UtcNow,
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"])),
                                SecurityAlgorithms.HmacSha256)
                    );
                    return Ok(new {token = new JwtSecurityTokenHandler().WriteToken(token) });
                }

            }
            return BadRequest();

        }
    }
}
