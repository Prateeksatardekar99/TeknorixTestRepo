using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Technorix.Models;

namespace Technorix.Controllers
{



    [ApiController]

    [Authorize(Roles = "Admin")]

    [Route("api/v1/[controller]")]


    public class LocationController : Controller
    {

        
            private JobsDbContext context;



            public LocationController(JobsDbContext context)
            {

                this.context = context;
            }





            [HttpPost]
            public async Task<ActionResult> CreateLocation([FromBody] Location obj)
            {
                // Add the Department object directly
                context.Locations.Add(obj);

                // Save changes to the database
                await context.SaveChangesAsync();

                // Return 201 Created with the new department ID
                return Created();
            }


            [HttpGet]
            public async Task<ActionResult<IEnumerable<Location>>> GetDepartments()
            {
                var location = await context.Locations.ToListAsync();
                return Ok(location);
            }

            [HttpPut("{id}")]
            public async Task<ActionResult> UpdateLocations(int id, [FromBody] Location obj)
            {
                var location = await context.Locations.FindAsync(id);
                if (location == null)
                    return NotFound();

                location.Title = obj.Title;
                location.State=obj.State;
                location.Country = obj.Country;
                location.City = obj.City;
                location.Zip = obj.Zip;
                await context.SaveChangesAsync();

                return Ok();
            }
        }

    }
