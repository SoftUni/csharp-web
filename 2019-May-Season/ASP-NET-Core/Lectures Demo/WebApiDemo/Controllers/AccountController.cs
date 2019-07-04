using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiDemo.Configuration;

namespace WebApiDemo.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IOptions<JwtSettings> options;

        public AccountController(IOptions<JwtSettings> options)
        {
            this.options = options;
        }

        [HttpGet("[action]")]
        public ActionResult<string> WhoAmI()
        {
            return "Username: " + this.User.Identity.Name;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public ActionResult<string> AdminArea()
        {
            return "welcome admin!";
        }

        [HttpGet("[action]")]
        public ActionResult<string> Login(string username, string password)
        {
            if (username == "admin" && password == "admin")
            {
                var secret = this.options.Value.Secret;
                var key = Encoding.UTF8.GetBytes(secret);

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Expires = DateTime.UtcNow.AddDays(7),
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Role, "Admin"),
                        new Claim(ClaimTypes.Name, "admin@nikolay.it"),
                        new Claim(ClaimTypes.NameIdentifier, "2734hr45-345-345")
                    }),
                    SigningCredentials = new SigningCredentials(
                                 new SymmetricSecurityKey(key),
                                 SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwt = tokenHandler.WriteToken(token);
                return jwt;
            }

            return this.Forbid();
        }
    }
}
