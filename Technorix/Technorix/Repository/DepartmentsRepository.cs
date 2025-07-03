using Technorix.Models;

using Microsoft.EntityFrameworkCore;

namespace Technorix.Repository
{


    public class DepartmentsRepository : IdepartmentsRepository
    {
        private readonly JobsDbContext _context;

        public DepartmentsRepository(JobsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAll()
        {
            return await _context.Departments
                .OrderBy(d => d.Title)
                .ToListAsync();
        }

        public async Task<Department?> GetById(int id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task Create(Department department)
        {
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
        }

       
        public async Task<bool> Update(Department department)
        {
            var existing = await _context.Departments.FindAsync(department.Id);
            if (existing == null)
            {
                throw new InvalidOperationException($"Department with ID {department.Id} does not exist.");
            }

            existing.Title = department.Title;

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
       
    }
}
