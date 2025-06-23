//using Azure.Core;
//using Humanizer;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
//using Microsoft.EntityFrameworkCore;
//using Technorix.DTOs;
//using Technorix.Models;

//namespace Technorix.Controllers
//{


//    [ApiController]

//    [Authorize]

//    [Route("api/v1/[controller]")]

//    public class jobsController : Controller
//    {
//        private JobsDbContext context;




//        public jobsController(JobsDbContext context)
//        {

//            this.context = context;
//        }




//        [HttpPost]
//        public async Task<ActionResult> Create([FromBody] JobDto obj)
//        {
//            var job = new Job
//            {
//                Title = obj.Title,
//                Description = obj.Description,
//                Locationid = obj.LocationId,
//                Departmentid = obj.DepartmentId,
//                Closingdate = obj.ClosingDate,
//                Posteddate = DateTime.UtcNow,
//                Code = "JOB-" + (await context.Jobs.CountAsync() + 1).ToString()
//            };

//            context.Jobs.Add(job);
//            await context.SaveChangesAsync();

//            return Created($"http://localhost/api/v1/jobs/{job.Id}", null);
//        }



//        [Authorize(Roles = "User")]

//        [HttpGet]
//        public async Task<ActionResult<JobListResponseDto>> jobs([FromQuery] listDTO obj)
//        {
//            var query = context.Jobs
//                .Include(j => j.Location)
//                .Include(j => j.Department)
//                .AsQueryable();

//            // Search string
//            if (!string.IsNullOrWhiteSpace(obj.SearchText))
//            {
//                query = query.Where(j => j.Title.Contains(obj.SearchText) || j.Description.Contains(obj.SearchText));
//            }

//            // Optional filters
//            if (obj.locationId != 0)
//            {
//                query = query.Where(j => j.Locationid == obj.locationId);
//            }

//            if (obj.departmentId != 0)
//            {
//                query = query.Where(j => j.Departmentid == obj.departmentId);
//            }

//            var total = await query.CountAsync();

//            var jobs = await query
//                .OrderByDescending(j => j.Posteddate)
//                .Skip((obj.pageNo - 1) * obj.pageSize)
//                .Take(obj.pageSize)
//                .Select(j => new JobListItemDto
//                {
//                    Id = j.Id,
//                    Code = j.Code,
//                    Title = j.Title,
//                    Location = j.Location.Title,
//                    Department = j.Department.Title,
//                    PostedDate = j.Posteddate,
//                    ClosingDate = j.Closingdate
//                })
//                .ToListAsync(); // <--- You missed this

//            var response = new JobListResponseDto
//            {
//                Total = total,
//                Data = jobs
//            };

//            return Ok(response);
//        }








//        [Authorize(Roles = "User")]


//        [HttpGet("{id}")]
//        public async Task<ActionResult> Details(int id)
//        {

//            var job = await context.Jobs
//                .Include(j => j.Location)
//                .Include(j => j.Department)
//                .FirstOrDefaultAsync(j => j.Id == id);




//            if (job == null) return NotFound();

//            return Ok(new
//            {
//                job.Id,
//                job.Code,
//                job.Title,
//                job.Description,

//                Location = new
//                {
//                    job.Location.Id,
//                    job.Location.Title,
//                    job.Location.City,
//                    job.Location.State,
//                    job.Location.Country,
//                    job.Location.Zip
//                },
//                Department = new
//                {
//                    job.Department.Id,
//                    job.Department.Title
//                },


//                job.Posteddate,
//                job.Closingdate
//            });
//        }



//        [HttpPut("{id}")]

//            public async Task<ActionResult> Update(int id, [FromBody] JobDto obj)


//        {

//            var job = await context.Jobs.FindAsync(id);
//            if (job == null) return NotFound();

//            job.Title = obj.Title;
//            job.Description = obj.Description;
//            job.Locationid = obj.LocationId;
//            job.Departmentid = obj.DepartmentId;
//            job.Closingdate = obj.ClosingDate;

//            await context.SaveChangesAsync();
//            return Ok();

//        }
//    }
//}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Technorix.DTOs;
using Technorix.Models;

namespace Technorix.Controllers
{
    [ApiController]
  //  [Authorize]
    [Route("api/v1/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly IJobRepository _jobRepo;

        public JobsController(IJobRepository jobRepo)
        {
            _jobRepo = jobRepo;
        }


        /// <summary>
        /// create a new job.
        /// </summary>

        /// <param name="obj">The job data to be created, including title, description, department, location, and closing date.</param>
        /// <returns>
        /// Returns a 201 Created response with the newly created job's details if successful.
        /// Returns 400 Bad Request if the input data is invalid.
        /// </returns>


        [Authorize(Roles = "Admin")]

        // POST: api/v1/jobs
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] JobDto obj)
        {
            var job = new Job
            {
                Title = obj.Title,
                Description = obj.Description,
                Locationid = obj.LocationId,
                Departmentid = obj.DepartmentId,
                Closingdate = obj.ClosingDate,
                Posteddate = DateTime.UtcNow,
                Code = "JOB-" + DateTime.UtcNow.Ticks.ToString()
            };

            await _jobRepo.Create(job);

        
                 return Created($"http://localhost/api/v1/jobs/{job.Id}", null);

        }

        /// <summary>
        /// Retrieves a list of jobs with pagination and filters.
        /// </summary>
        /// <param name="obj">Filter criteria such as search keyword, page number, and page size.</param>
        /// <returns>
        /// A paginated list of jobs with basic details like title, location, department, and dates.
        /// </returns>
        /// <response code="200">Returns the list of jobs</response>
        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<JobListResponseDto>> Jobs([FromQuery] listDTO obj)
        {
            var jobs = await _jobRepo.Jobs(obj);
            var total = await _jobRepo.JobsCount(obj);

            var data = jobs.Select(j => new JobListItemDto
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

        

        /// <summary>
        /// Retrieves detailed information about a specific job by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the job.</param>
        /// <returns>
        /// The job details including title, description, department, location, posted date, and closing date.
        /// </returns>
        /// <response code="200">Returns the job details</response>
        /// <response code="404">If the job is not found</response>
       

        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult> Details(int id)
        {
            var job = await _jobRepo.Details(id);
            if (job == null) return NotFound();

            return Ok(new
            {
                job.Id,
                job.Code,
                job.Title,
                job.Description,
                Location = new
                {
                    job.Location?.Id,
                    job.Location?.Title,
                    job.Location?.City,
                    job.Location?.State,
                    job.Location?.Country,
                    job.Location?.Zip
                },
                Department = new
                {
                    job.Department?.Id,
                    job.Department?.Title
                },
                job.Posteddate,
                job.Closingdate
            });
        }


        /// <summary>
        /// Updates an existing job.
        /// </summary>
        /// <param name="id">The unique identifier of the job to be updated.</param>
        /// <param name="obj">The updated job data.</param>
        /// <returns>
        /// Returns 200 OK if the job is updated successfully.
        /// Returns 404 Not Found if the job does not exist.
        /// </returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult> Update(int id, [FromBody] JobDto obj)
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

        /// <summary>
        /// Deletes an existing job.
        /// </summary>
        /// <param name="id">The ID of the job to delete.</param>
        /// <returns>
        /// Returns 204 No Content if deletion is successful.
        /// Returns 404 Not Found if the job does not exist.
        /// </returns>
        

        [Authorize(Roles = "Admin")]

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var job = await _jobRepo.Details(id);
            if (job == null) return NotFound();

            await _jobRepo.Delete(id);
            return NoContent();
        }
    }
}
