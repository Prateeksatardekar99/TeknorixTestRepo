using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Technorix.DTOs;
using Technorix.Models;

namespace Technorix.Controllers
{
    /// <summary>
    /// Handles job creation, listing, updating, and deletion.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly IJobRepository _jobRepo;

        public JobsController(IJobRepository jobRepo)
        {
            _jobRepo = jobRepo;
        }

        /// <summary>
        /// Creates a new job entry.
        /// </summary>
        /// <param name="obj">Job details to create.</param>
        /// <returns>Returns 201 Created on success.</returns>
        /// <response code="201">Job created successfully.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Create([FromBody] JobRequestDto obj)
        {
            try
            {
                var job = new Job
                {
                    Title = obj.Title,
                    Description = obj.Description,
                    Locationid = obj.LocationId,
                    Departmentid = obj.DepartmentId,
                    Closingdate = obj.ClosingDate,
                    Posteddate = DateTime.UtcNow,
                    Code = "JOB-" + DateTime.UtcNow.ToString("yyyyMMddHHmmss")
                };

                await _jobRepo.Create(job);
                return Created($"http://localhost/api/v1/jobs/{job.Id}", null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves paginated list of jobs.
        /// </summary>
        /// <param name="obj">Filter and pagination criteria.</param>
        /// <returns>Returns list of jobs with pagination.</returns>
        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JobListResponseDto>> Jobs([FromQuery] JoblistDTO obj)
        {
            try
            {
                var jobs = await _jobRepo.Jobs(obj);
                var total = await _jobRepo.JobsCount(obj);

                var data = jobs.Select(j => new JobListItemResponseDto
                {
                    Id = j.Id,
                    Code = j.Code,
                    Title = j.Title,
                    Location = j.Location?.Title,
                    Department = j.Department?.Title,
                    PostedDate = j.Posteddate,
                    ClosingDate = j.Closingdate
                }).ToList();

                var response = new JobListResponseDto
                {
                    Total = total,
                    Data = data
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets job details by ID.
        /// </summary>
        /// <param name="id">Job ID.</param>
        /// <returns>Job details or 404 if not found.</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var job = await _jobRepo.Details(id);
                if (job == null) return NotFound();

                //return Ok(new
                //{
                //    job.Id,
                //    job.Code,
                //    job.Title,
                //    job.Description,
                //    Location = new
                //    {
                //        job.Location?.Id,
                //        job.Location?.Title,
                //        job.Location?.City,
                //        job.Location?.State,
                //        job.Location?.Country,
                //        job.Location?.Zip
                //    },
                //    Department = new
                //    {
                //        job.Department?.Id,
                //        job.Department?.Title
                //    },
                //    job.Posteddate,
                //    job.Closingdate
                //});


                return Ok(new JobDetailsResponseDto
                {
                    Id = job.Id,
                    Code = job.Code,
                    Title = job.Title,
                    Description = job.Description,
                    Location = new LocationResponseDto
                    {
                        Id = job.Location?.Id ?? 0,
                        Title = job.Location?.Title,
                        City = job.Location?.City,
                        State = job.Location?.State,
                        Country = job.Location?.Country,
                        Zip = job.Location?.Zip ?? 0
                    },
                    Department = new DepartmentResponseDto
                    {
                        Id = job.Department?.Id ?? 0,
                        Title = job.Department?.Title
                    },
                    Posteddate = job.Posteddate,
                    Closingdate = job.Closingdate
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing job by ID.
        /// </summary>
        /// <param name="id">Job ID.</param>
        /// <param name="obj">Updated job data.</param>
        /// <returns>200 OK or 404 Not Found.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update(int id, [FromBody] JobRequestDto obj)
        {
            try
            {
                var job = await _jobRepo.Details(id);
                if (job == null) return NotFound();

                job.Title = obj.Title;
                job.Description = obj.Description;
                job.Locationid = obj.LocationId;
                job.Departmentid = obj.DepartmentId;
                job.Closingdate = obj.ClosingDate;

                await _jobRepo.Update(job);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a job by ID.
        /// </summary>
        /// <param name="id">Job ID.</param>
        /// <returns>200 sucess or 404 Not Found.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var job = await _jobRepo.Details(id);
                if (job == null) return NotFound();

                await _jobRepo.Delete(id);
                return Ok(new { message = "Deleted job " }); // Return 200 with message

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
