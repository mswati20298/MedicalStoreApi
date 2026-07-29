using MedStoreAPI.Domain;

namespace MedStoreAPI.Entities.Repositories
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Contract for Suppliers data access. Works with Domain models.
    /// Implemented by MedStoreAPI.Infrastructure.Repositories.SuppliersRepository.
    /// </summary>
    public interface ISuppliersRepository
    {
        Task<Supplier> InsertAsync(Supplier supplier);
        Task UpdateAsync(Supplier supplier);
        Task<Supplier?> GetByIDAsync(int supplierID);
        Task<IEnumerable<Supplier>> GetAllAsync(int storeID);
        Task DeleteAsync(int supplierID);
    }
}
