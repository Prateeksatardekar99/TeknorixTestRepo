
using Technorix.Models;
using Technorix.DTOs;

public interface IJobRepository
{
    Task<Job?> Details(int id);                        // Matches [HttpGet("{id}")]
    Task<IEnumerable<Job>> Jobs(JoblistDTO filters);     // Matches [HttpGet] with filters
    Task<int> JobsCount(JoblistDTO filters);             // Matches the total count logic
    Task Create(Job job);                             // Matches [HttpPost]
    Task Update(Job job);                             // Matches [HttpPut("{id}")]
    Task Delete(int id);                              // Optional for [HttpDelete]
}
