using Technorix.Models;

namespace Technorix.Repository
{
    public interface IdepartmentsRepository
    {

        Task<IEnumerable<Department>> GetAll();
        
        Task Create(Department department);

        Task<bool> Update(Department department); 
    }
}

