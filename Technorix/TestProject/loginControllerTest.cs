using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Technorix.Models;
using Technorix.DTOs;
using Technorix.Controllers.Technorix.Controllers;

namespace TestProject
{
  


        public class loginControllerTest
    {
            private readonly JobsDbContext _context;
            private readonly loginController _controller;

            public loginControllerTest()
            {
                var options = new DbContextOptionsBuilder<JobsDbContext>()
                    .UseInMemoryDatabase(databaseName: "jobsDb")
                    .Options;

                _context = new JobsDbContext(options);

                // Seed data
                _context.Users.Add(new User
                {
                    
                    Username = "testuser",
                    Passwordhash = new Microsoft.AspNetCore.Identity.PasswordHasher<object>()
                        .HashPassword(null, "password123"),
                    Userrole = "User"
                });
                _context.SaveChanges();

                var jwtSettings = Options.Create(new JwtSettings
                {
                    Key = "hdsjfhdjshfjdshfjksdhfjdshfjkdhfkjsdhfjk",
                    Issuer = "TeknorixIssuer",
                    Audience = "TeknorixUsers",
                    ExpiryMinutes = 60
                });

                _controller = new loginController(_context, jwtSettings);
            }

            [Fact]
            public async Task Login_WithValidCredentials_ReturnsToken()
            {
                var loginDto = new loginDTO
                {
                    Username = "testuser",
                    Password = "password123"
                };

                var result = await _controller.Login(loginDto);

                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.NotNull(okResult.Value);
            }

            [Fact]
            public async Task Login_WithInvalidPassword_ReturnsUnauthorized()
            {
                var loginDto = new loginDTO
                {
                    Username = "testuser",
                    Password = "wrongpassword"
                };

                var result = await _controller.Login(loginDto);

                Assert.IsType<UnauthorizedObjectResult>(result);
            }
        }
    }





