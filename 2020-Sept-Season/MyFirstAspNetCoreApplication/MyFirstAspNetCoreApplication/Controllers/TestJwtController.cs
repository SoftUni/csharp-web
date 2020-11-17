using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyFirstAspNetCoreApplication.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstAspNetCoreApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestJwtController : ControllerBase
    {
        private readonly IOptions<JwtSettings> jwtSettings;

        public TestJwtController(IOptions<JwtSettings> jwtSettings)
        {
            this.jwtSettings = jwtSettings;
        }

        [Authorize]
        public ActionResult<string> WhoAmI()
        {
            return this.User.Identity.Name;
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public ActionResult<UserModel> Post(LoginInputModel input)
        {
            // Authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.jwtSettings.Value.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, input.Username.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                                 new SymmetricSecurityKey(key),
                                 SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenAsString = tokenHandler.WriteToken(token);
            return new UserModel { Token = tokenAsString };
        }

    }

    public class UserModel
    {
        public string Token { get; set; }
    }

    public class LoginInputModel
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
