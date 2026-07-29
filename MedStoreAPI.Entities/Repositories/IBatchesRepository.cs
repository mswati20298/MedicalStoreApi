using MedStoreAPI.Domain;
using MedStoreAPI.Dtos.Batches;

namespace MedStoreAPI.Entities.Repositories
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Contract for Batches data access. Works with Domain model (Batch)
    /// for standard CRUD, and with BatchesLowStockResponseDto for the low-stock
    /// report since that query aggregates Medicines+Batches and isn't a single
    /// table clone. Implemented by MedStoreAPI.Infrastructure.Repositories.BatchesRepository.
    /// </summary>
    public interface IBatchesRepository
    {
        Task<Batch> InsertAsync(Batch batch);
        Task<Batch?> GetByIDAsync(int batchID);
        Task<IEnumerable<Batch>> GetByMedicineAsync(int medicineID);
        Task<IEnumerable<Batch>> GetExpiryStatusAsync(int storeID, int redDays, int yellowDays);
        Task<IEnumerable<Batch>> GetExpiringAsync(int storeID, int days);
        Task<IEnumerable<BatchesLowStockResponseDto>> GetLowStockAsync(int storeID);
        Task ReduceStockAsync(int batchID, int quantity);
        Task DeleteAsync(int batchID);
    }
}
