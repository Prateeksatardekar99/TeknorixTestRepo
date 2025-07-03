using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Technorix.Controllers;
using Technorix.DTOs;
using Technorix.Models;
using Technorix.Repository;
using Xunit;

namespace TestProject
{

    public class LoginControllerTest
    {
        private readonly Mock<ILoginRepository> _mockRepo;
        private readonly LoginController _controller;

        public LoginControllerTest()
        {
            _mockRepo = new Mock<ILoginRepository>();

            _controller = new LoginController(_mockRepo.Object);
        }

        [Fact]
        
        public async Task Login_WithValidCredentials_ReturnsToken()
        {
            var loginDto = new loginRequestDTO
            {
                Username = "testuser",
                Password = "password123"
            };

            var user = new User
            {
                Username = "testuser",
                PasswordHash = new PasswordHasher<object>().HashPassword(null, "password123"),
                UserRole = "User"
            };

            _mockRepo.Setup(r => r.GetUserByUsername("testuser")).ReturnsAsync(user);
            _mockRepo.Setup(r => r.VerifyPassword(It.IsAny<string>(), "password123")).Returns(true);
            _mockRepo.Setup(r => r.GenerateJwtToken(user)).Returns("mocked-jwt-token");

            var result = await _controller.Login(loginDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var tokenObj = Assert.IsType<TokenResponseDto>(okResult.Value);

            Assert.NotNull(tokenObj);
            Assert.Equal("mocked-jwt-token", tokenObj.Token);
        }



        [Fact]
        public async Task Login_WithInvalidPassword_ReturnsUnauthorized()
        {
            var loginDto = new loginRequestDTO
            {
                Username = "testuser",
                Password = "wrongpassword"
            };

            var user = new User
            {
                Username = "testuser",
                PasswordHash = "hashed-password"
            };

            _mockRepo.Setup(r => r.GetUserByUsername("testuser"))
                          .ReturnsAsync(user);

            _mockRepo.Setup(r => r.VerifyPassword("hashed-password", "wrongpassword"))
                          .Returns(false);

            var result = await _controller.Login(loginDto);

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid password", unauthorizedResult.Value);
        }
    }
}