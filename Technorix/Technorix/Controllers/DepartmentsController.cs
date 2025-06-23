using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Technorix.DTOs;
using Technorix.Models;

namespace Technorix.Controllers
{

    [ApiController]
    [Authorize(Roles = "Admin")]


    [Route("api/v1/[controller]")]
    //[Authorize]

    public class DepartmentsController : Controller
    {
        private JobsDbContext context;



        public DepartmentsController(JobsDbContext context)
        {

            this.context = context;
        }



      

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Department obj)
        {
            // Add the Department object directly
            context.Departments.Add(obj);

            // Save changes to the database
            await context.SaveChangesAsync();

            // Return 201 Created with the new department ID
            return Created();
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            var departments = await context.Departments.ToListAsync();
            return Ok(departments);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDepartment(int id, [FromBody] Department obj)
        {
            var department = await context.Departments.FindAsync(id);
            if (department == null)
                return NotFound();

            department.Title = obj.Title;
            await context.SaveChangesAsync();

            return Ok();
        }

    }
}
