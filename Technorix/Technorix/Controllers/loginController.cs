using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Technorix.DTOs;
using Technorix.Models;
using Technorix.Repository;

namespace Technorix.Controllers
{
    /// <summary>
    /// Handles user authentication and token generation.
    /// </summary>
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository _loginRepo;

        public LoginController(ILoginRepository loginRepo)
        {
            _loginRepo = loginRepo;
        }
        /// <summary>
        /// Authenticates the user and returns a JWT token.
        /// </summary>


        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]


        public async Task<ActionResult> Login([FromBody] loginRequestDTO login)
        {
            try
            {

                if (login == null || string.IsNullOrWhiteSpace(login.Username) || string.IsNullOrWhiteSpace(login.Password))
                    return BadRequest("Username and password must be provided.");

                var user = await _loginRepo.GetUserByUsername(login.Username);
                if (user == null)
                    return Unauthorized("User not found");

                if (!_loginRepo.VerifyPassword(user.PasswordHash, login.Password))
                    return Unauthorized("Invalid password");

                var token = _loginRepo.GenerateJwtToken(user);
                return Ok(new TokenResponseDto { Token = token });


            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}