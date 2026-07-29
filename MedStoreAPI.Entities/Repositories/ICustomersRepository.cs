using MedStoreAPI.Domain;

namespace MedStoreAPI.Entities.Repositories
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Contract for Customers data access. Works with Domain models
    /// (raw DB shape). Implemented by MedStoreAPI.Infrastructure.Repositories.CustomersRepository.
    /// </summary>
    public interface ICustomersRepository
    {
        Task<Customer> InsertAsync(Customer customer);
        Task<Customer?> GetByMobileAsync(int storeID, string mobile);
        Task<IEnumerable<Customer>> GetAllAsync(int storeID);
    }
}
