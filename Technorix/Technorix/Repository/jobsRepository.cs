using Microsoft.EntityFrameworkCore;
using Technorix.DTOs;
using Technorix.Models;

public class JobRepository : IJobRepository
{
    private readonly JobsDbContext _context;

    public JobRepository(JobsDbContext context)
    {
        _context = context;
    }

    public async Task<Job?> Details(int id)
    {
        return await _context.Jobs
            .Include(j => j.Location)
            .Include(j => j.Department)
            .FirstOrDefaultAsync(j => j.Id == id);
    }

    public async Task<IEnumerable<Job>> Jobs(JoblistDTO filters)
    {
        var query = _context.Jobs
            .Include(j => j.Location)
            .Include(j => j.Department)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filters.SearchText))
        {
            query = query.Where(j =>
                j.Title.Contains(filters.SearchText) ||
                j.Description.Contains(filters.SearchText));
        }

        if (filters.locationId != 0)
        {
            query = query.Where(j => j.Locationid == filters.locationId);
        }

        if (filters.departmentId != 0)
        {
            query = query.Where(j => j.Departmentid == filters.departmentId);
        }

        return await query
            .OrderByDescending(j => j.Posteddate)
            .Skip((filters.pageNo - 1) * filters.pageSize)
            .Take(filters.pageSize)
            .ToListAsync();
    }

    public async Task<int> JobsCount(JoblistDTO filters)
    {
        var query = _context.Jobs.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filters.SearchText))
        {
            query = query.Where(j =>
                j.Title.Contains(filters.SearchText) ||
                j.Description.Contains(filters.SearchText));
        }

        if (filters.locationId != 0)
        {
            query = query.Where(j => j.Locationid == filters.locationId);
        }

        if (filters.departmentId != 0)
        {
            query = query.Where(j => j.Departmentid == filters.departmentId);
        }

        return await query.CountAsync();
    }

    public async Task Create(Job job)
    {
        var locationExists = await _context.Locations.AnyAsync(l => l.Id == job.Locationid);
        if (!locationExists)
            throw new InvalidOperationException($"Location with ID {job.Locationid} does not exist.");

        var departmentExists = await _context.Departments.AnyAsync(d => d.Id == job.Departmentid);
        if (!departmentExists)
            throw new InvalidOperationException($"Department with ID {job.Departmentid} does not exist.");

        // If both checks pass, then only proceed to save data
        await _context.Jobs.AddAsync(job);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Job job)
    {
        var locationExists = await _context.Locations.AnyAsync(l => l.Id == job.Locationid);
        if (!locationExists)
        {
            throw new InvalidOperationException($"Location with ID {job.Locationid} does not exist.");
        }

        // Check if Department exists
        var departmentExists = await _context.Departments.AnyAsync(d => d.Id == job.Departmentid);
        if (!departmentExists)
        {
            throw new InvalidOperationException($"Department with ID {job.Departmentid} does not exist.");
        }

        _context.Jobs.Update(job);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var job = await _context.Jobs.FindAsync(id);
        if (job != null)
        {
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
        }
    }
}
