using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Technorix.DTOs;
using Technorix.Models;


namespace Technorix.Controllers
{


    namespace Technorix.Controllers
    {
        [ApiController]
        [AllowAnonymous]


        [Route("api/[controller]")]

        public class loginController : Controller
        {



            private readonly JwtSettings objjwtSettings;
            private JobsDbContext context;


            public loginController(JobsDbContext dbContext, IOptions<JwtSettings> obj)
            {
                this.objjwtSettings = obj.Value;
                this.context = dbContext;
            }

            [HttpPost("login")]
            public async Task<ActionResult> Login([FromBody] loginDTO login)
            {
                var user = await context.Users
                    .FirstOrDefaultAsync(u => u.Username == login.Username);

                var _hasher = new PasswordHasher<object>();


                var passwordencrytion = _hasher.HashPassword(null, login.Username);


                if (user == null)
                    return Unauthorized("User not found");


                // `login.PasswordHash` should be the plain password from user input (bad name, better to rename it to just `Password`)
                var result = _hasher.VerifyHashedPassword(user, user.Passwordhash, login.Password);

                if (result == PasswordVerificationResult.Failed)
                    return Unauthorized("Invalid password");

                var token = GenerateJwtToken(user.Username,user.Userrole);
                return Ok(new { token });
            }


            
            
        



        private string GenerateJwtToken(string username,string userRole)
            {
                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                                new Claim(ClaimTypes.Role, userRole)


            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(objjwtSettings.Key ?? "DefaultKeyMustBeSecureAnd32CharLong"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: objjwtSettings.Issuer,
                    audience: objjwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(objjwtSettings.ExpiryMinutes),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }



    }

}
