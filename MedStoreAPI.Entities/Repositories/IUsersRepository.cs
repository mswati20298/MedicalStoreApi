using MedStoreAPI.Domain;

namespace MedStoreAPI.Entities.Repositories
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Contract for Users data access. Works with Domain model (User).
    /// Implemented by MedStoreAPI.Infrastructure.Repositories.UsersRepository.
    /// </summary>
    public interface IUsersRepository
    {
        Task<User> InsertAsync(User user);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByIDAsync(int userID);
        Task<IEnumerable<User>> GetByStoreAsync(int storeID);
        Task UpdatePasswordAsync(int userID, string passwordHash);
        Task DeactivateAsync(int userID);
    }
}
