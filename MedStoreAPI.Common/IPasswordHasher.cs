using BCrypt.Net;

namespace MedStoreAPI.Common
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Wraps password hashing so Service layer never deals with
    /// a specific hashing library directly - only this interface. Uses BCrypt
    /// (industry standard, includes built-in salting).
    /// </summary>
    public interface IPasswordHasher
    {
        string Hash(string plainTextPassword);
        bool Verify(string plainTextPassword, string passwordHash);
    }

    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string plainTextPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainTextPassword);
        }

        public bool Verify(string plainTextPassword, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(plainTextPassword, passwordHash);
        }
    }
}
