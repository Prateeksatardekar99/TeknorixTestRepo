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

    public async Task<IEnumerable<Job>> Jobs(listDTO filters)
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

    public async Task<int> JobsCount(listDTO filters)
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
        await _context.Jobs.AddAsync(job);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Job job)
    {
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
