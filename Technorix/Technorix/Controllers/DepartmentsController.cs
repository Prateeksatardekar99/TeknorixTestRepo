using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Technorix.DTOs;
using Technorix.Models;
using Technorix.Repository;

namespace Technorix.Controllers
{
    /// <summary>
    /// Manages department-related operations.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IdepartmentsRepository _departmentRepo;

        public DepartmentsController(IdepartmentsRepository departmentRepo)
        {
            _departmentRepo = departmentRepo;
        }

        /// <summary>
        /// Creates a new department.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Create([FromBody] DepartmentCreateDto obj)
        {
            try
            {
                if (obj == null || string.IsNullOrWhiteSpace(obj.Title))
                    return BadRequest("Invalid department data.");

                var department = new Department
                {
                    Title = obj.Title
                };

                 await _departmentRepo.Create(department);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets a list of all departments.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DepartmentResponseDto>>> GetDepartments()
        {
            try
            {
                var departments = await _departmentRepo.GetAll();

                var result = departments.Select(d => new DepartmentResponseDto
                {
                    Id = d.Id,
                    Title = d.Title
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing department.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateDepartment(int id, [FromBody] DepartmentCreateDto obj)
        {
            try
            {
                if (obj == null || id < 0 || string.IsNullOrWhiteSpace(obj.Title))
                    return BadRequest("Invalid department data.");

                var data = new Department
                {
                   Id=id,
                    Title = obj.Title
                };

                bool result = await _departmentRepo.Update(data);
                if (!result)
                    return NotFound($"Department with ID {id} not found.");

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
