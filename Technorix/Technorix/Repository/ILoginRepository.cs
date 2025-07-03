
using Technorix.Models;

namespace Technorix.Repository
{
    public interface ILoginRepository
    {
        Task<User?> GetUserByUsername(string username);
        bool VerifyPassword(string hashedPassword, string plainPassword);
        string GenerateJwtToken(User user);
    }
}
