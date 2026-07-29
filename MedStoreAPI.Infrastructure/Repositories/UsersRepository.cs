using MedStoreAPI.Common;
using MedStoreAPI.Domain;
using MedStoreAPI.Entities.Repositories;

namespace MedStoreAPI.Infrastructure.Repositories
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Data access implementation for Users, calls stored
    /// procedures via ISqlDataAccess. Implements IUsersRepository.
    /// </summary>
    public class UsersRepository : IUsersRepository
    {
        private readonly ISqlDataAccess _db;

        public UsersRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<User> InsertAsync(User user)
        {
            var parameters = new
            {
                storeID = user.StoreId,
                roleID = user.RoleId,
                fullName = user.FullName,
                username = user.Username,
                passwordHash = user.PasswordHash,
                email = user.Email,
                mobile = user.Mobile
            };

            var userID = await _db.QuerySingleAsync<int>(StoredProcedureNames.User.Insert, parameters);
            user.UserId = userID;
            return user;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            var parameters = new { username };
            return await _db.QuerySingleAsync<User>(StoredProcedureNames.User.GetByUsername, parameters);
        }

        public async Task<User?> GetByIDAsync(int userID)
        {
            var parameters = new { userID };
            return await _db.QuerySingleAsync<User>(StoredProcedureNames.User.GetByID, parameters);
        }

        public async Task<IEnumerable<User>> GetByStoreAsync(int storeID)
        {
            var parameters = new { storeID };
            return await _db.QueryAsync<User>(StoredProcedureNames.User.GetByStore, parameters);
        }

        public async Task UpdatePasswordAsync(int userID, string passwordHash)
        {
            var parameters = new { userID, passwordHash };
            await _db.ExecuteAsync(StoredProcedureNames.User.UpdatePassword, parameters);
        }

        public async Task DeactivateAsync(int userID)
        {
            var parameters = new { userID };
            await _db.ExecuteAsync(StoredProcedureNames.User.Deactivate, parameters);
        }
    }
}
