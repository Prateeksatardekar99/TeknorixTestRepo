using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Technorix.DTOs;
using Technorix.Models;
using Technorix.Repository;

namespace Technorix.Controllers
{
    /// <summary>
    /// Manages location-related operations.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly IlocationsRepository _locationRepo;

        public LocationController(IlocationsRepository locationRepo)
        {
            _locationRepo = locationRepo;
        }

        /// <summary>
        /// Creates a new location.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateLocation([FromBody] LocationRequestDto obj)
        {
            try
            {
                if (obj == null || string.IsNullOrWhiteSpace(obj.Title))
                    return BadRequest("Invalid location data.");

                var location = new Location
                {
                    Title = obj.Title,
                    City = obj.City,
                    State = obj.State,
                    Country = obj.Country,
                    Zip = obj.Zip
                };

                await _locationRepo.Create(location);
                return Created(string.Empty, null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets a list of all locations.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<LocationResponseDto>>> GetLocations()
        {
            try
            {
                var locations = await _locationRepo.GetAll();

                var locationDtos = locations.Select(l => new LocationResponseDto
                {
                    Id = l.Id,
                    Title = l.Title,
                    City = l.City,
                    State = l.State,
                    Country = l.Country,
                    Zip = l.Zip
                }).ToList();

                return Ok(locationDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing location by ID.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateLocations(int id, [FromBody] LocationRequestDto obj)
        {
            try
            {
                var location = await _locationRepo.GetById(id);
                if (location == null)
                    return NotFound($"Location with ID {id} not found.");

                location.Title = obj.Title;
                location.State = obj.State;
                location.Country = obj.Country;
                location.City = obj.City;
                location.Zip = obj.Zip;

                await _locationRepo.Update(location);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
