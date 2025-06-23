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
using FluentAssertions;
using Technorix.Controllers;


namespace TestProject
{
    

        public class jobsControllerTests
        {
            private readonly Mock<IJobRepository> _mockRepo;
            private readonly JobsController _controller;

            public jobsControllerTests()
            {
                _mockRepo = new Mock<IJobRepository>();
                _controller = new JobsController(_mockRepo.Object);
            }

            [Fact]
            public async Task Create_ValidJob_ReturnsCreated()
            {
                var dto = new JobDto
                {
                    Title = "new job",
                    Description = "Test data",
                    LocationId = 1,
                    DepartmentId = 2,
                    ClosingDate = DateTime.UtcNow.AddDays(10)
                };

                _mockRepo.Setup(r => r.Create(It.IsAny<Job>())).Returns(Task.CompletedTask);

                var result = await _controller.Create(dto);

                var created = result as CreatedResult;
                created.Should().NotBeNull();
                created.StatusCode.Should().Be(201);
            }

            [Fact]
            public async Task Jobs_ReturnsJobList()
            {
                _mockRepo.Setup(r => r.Jobs(It.IsAny<listDTO>())).ReturnsAsync(new List<Job>
            {
                new Job
                {
                    Id = 1,
                    Title = "Job A",
                    Department = new Department { Title = "Dept A" },
                    Location = new Location { Title = "Loc A" },
                    Posteddate = DateTime.UtcNow,
                    Closingdate = DateTime.UtcNow.AddDays(5),
                    Code = "JOB-1"
                }
            });

                _mockRepo.Setup(r => r.JobsCount(It.IsAny<listDTO>())).ReturnsAsync(1);

                var result = await _controller.Jobs(new listDTO());

                var okResult = result.Result as OkObjectResult;
                okResult.Should().NotBeNull();

                var response = okResult.Value as JobListResponseDto;
                response.Should().NotBeNull();
                response.Total.Should().Be(1);
                response.Data.Should().HaveCount(1);
            }

            [Fact]
            public async Task Details_ValidId_ReturnsJob()
            {
                _mockRepo.Setup(r => r.Details(1)).ReturnsAsync(new Job
                {
                    Id = 1,
                    Title = "Test Job",
                    Description = "Job Desc",
                    Code = "JOB-123",
                    Location = new Location { Id = 1, Title = "goa", City = "panjim", State = "ga", Country = "ind", Zip = 403511 },
                    Department = new Department { Id = 1, Title = "Engineering" },
                    Posteddate = DateTime.UtcNow,
                    Closingdate = DateTime.UtcNow.AddDays(5)
                });

                var result = await _controller.Details(1);

                var okResult = result as OkObjectResult;
                okResult.Should().NotBeNull();
            }

            [Fact]
            public async Task Details_InvalidId_ReturnsNotFound()
            {
                _mockRepo.Setup(r => r.Details(99)).ReturnsAsync((Job)null!);

                var result = await _controller.Details(99);

                result.Should().BeOfType<NotFoundResult>();
            }

            [Fact]
            public async Task Update_ValidId_UpdatesJob()
            {
                var existingJob = new Job { Id = 1, Title = "checking existig Job" };

                _mockRepo.Setup(r => r.Details(1)).ReturnsAsync(existingJob);
                _mockRepo.Setup(r => r.Update(It.IsAny<Job>())).Returns(Task.CompletedTask);

                var dto = new JobDto
                {
                    Title = "Updated Title",
                    Description = "Updated data",
                    DepartmentId = 1,
                    LocationId = 1,
                    ClosingDate = DateTime.UtcNow.AddDays(5)
                };

                var result = await _controller.Update(1, dto);

                result.Should().BeOfType<OkResult>();
            }

            [Fact]
            public async Task Update_InvalidId_ReturnsNotFound()
            {
                _mockRepo.Setup(r => r.Details(99)).ReturnsAsync((Job)null!);

                var result = await _controller.Update(99, new JobDto());

                result.Should().BeOfType<NotFoundResult>();
            }

            [Fact]
            public async Task Delete_ValidId_ReturnsNoContent()
            {
                _mockRepo.Setup(r => r.Details(1)).ReturnsAsync(new Job { Id = 1 });
                _mockRepo.Setup(r => r.Delete(1)).Returns(Task.CompletedTask);

                var result = await _controller.Delete(1);

                result.Should().BeOfType<NoContentResult>();
            }

            [Fact]
            public async Task Delete_InvalidId_ReturnsNotFound()
            {
                _mockRepo.Setup(r => r.Details(999)).ReturnsAsync((Job)null!);

                var result = await _controller.Delete(999);

                result.Should().BeOfType<NotFoundResult>();
            }
        }
    }




