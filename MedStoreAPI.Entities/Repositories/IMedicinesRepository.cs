using MedStoreAPI.Domain;

namespace MedStoreAPI.Entities.Repositories
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Contract for Medicines data access. Works with Domain models.
    /// Implemented by MedStoreAPI.Infrastructure.Repositories.MedicinesRepository.
    /// </summary>
    public interface IMedicinesRepository
    {
        Task<Medicine> InsertAsync(Medicine medicine);
        Task UpdateAsync(Medicine medicine);
        Task<Medicine?> GetByIDAsync(int medicineID);
        Task<IEnumerable<Medicine>> GetAllAsync(int storeID);
        Task<IEnumerable<Medicine>> SearchAsync(int storeID, string searchTerm);
        Task DeleteAsync(int medicineID);
    }
}
