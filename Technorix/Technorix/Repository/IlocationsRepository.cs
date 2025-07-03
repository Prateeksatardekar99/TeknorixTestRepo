using Technorix.Models;

namespace Technorix.Repository
{
    public interface IlocationsRepository
    {

        Task<IEnumerable<Location>> GetAll();
        Task<Location?> GetById(int id);
        Task Create(Location location);
        Task Update(Location location);
       
    }
}
