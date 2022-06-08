using StoredProcedureApi.Models;

namespace StoredProcedureApi.Repository
{
    public interface IUserRepo
    {
        Task<int> CreateUserAsync(UserProfile model);
        Task <List<UserProfile>> GetUsersAsync(string email, string passwordHash);
    }
}