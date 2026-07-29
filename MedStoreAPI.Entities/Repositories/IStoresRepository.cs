using MedStoreAPI.Domain;

namespace MedStoreAPI.Entities.Repositories
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Contract for Stores data access. Implemented by
    /// MedStoreAPI.Infrastructure.Repositories.StoresRepository.
    /// </summary>
    public interface IStoresRepository
    {
        Task<Store> InsertAsync(Store store);
        Task<Store?> GetByIDAsync(int storeID);
        Task<IEnumerable<Store>> GetAllAsync();
        Task UpdateAsync(Store store);
        Task UpdateLogoAsync(int storeID, string logoUrl);
    }
}
