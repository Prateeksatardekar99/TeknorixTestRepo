using Technorix.Models;
using Microsoft.EntityFrameworkCore;

namespace Technorix.Repository
{
   

        public class locationsRepository : IlocationsRepository
        {
            private readonly JobsDbContext _context;

            public locationsRepository(JobsDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Location>> GetAll()
            {
                return await _context.Locations.OrderBy(l => l.Title).ToListAsync();
            }

            public async Task<Location?> GetById(int id)
            {
                return await _context.Locations.FindAsync(id);
            }

            public async Task Create(Location location)
            {



                await _context.Locations.AddAsync(location);
                await _context.SaveChangesAsync();
            }

            public async Task Update(Location location)
            {
                _context.Locations.Update(location);
                await _context.SaveChangesAsync();
            }
        }
    }

