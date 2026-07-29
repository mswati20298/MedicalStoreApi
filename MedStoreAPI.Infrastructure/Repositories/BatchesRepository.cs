using MedStoreAPI.Common;
using MedStoreAPI.Domain;
using MedStoreAPI.Dtos.Batches;
using MedStoreAPI.Entities.Repositories;

namespace MedStoreAPI.Infrastructure.Repositories
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Data access implementation for Batches, calls stored
    /// procedures via ISqlDataAccess. Implements IBatchesRepository.
    /// </summary>
    public class BatchesRepository : IBatchesRepository
    {
        private readonly ISqlDataAccess _db;

        public BatchesRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<Batch> InsertAsync(Batch batch)
        {
            var parameters = new
            {
                storeID = batch.StoreId,
                medicineID = batch.MedicineId,
                supplierID = batch.SupplierId,
                batchNumber = batch.BatchNumber,
                expiryDate = batch.ExpiryDate,
                manufactureDate = batch.ManufactureDate,
                quantityReceived = batch.QuantityReceived,
                purchasePrice = batch.PurchasePrice,
                mrp = batch.MRP,
                dateReceived = batch.DateReceived
            };

            var batchID = await _db.QuerySingleAsync<int>(StoredProcedureNames.Batch.Insert, parameters);
            batch.BatchId = batchID;
            batch.QuantityRemaining = batch.QuantityReceived;
            return batch;
        }

        public async Task<Batch?> GetByIDAsync(int batchID)
        {
            var parameters = new { batchID };
            return await _db.QuerySingleAsync<Batch>(StoredProcedureNames.Batch.GetByID, parameters);
        }

        public async Task<IEnumerable<Batch>> GetByMedicineAsync(int medicineID)
        {
            var parameters = new { medicineID };
            return await _db.QueryAsync<Batch>(StoredProcedureNames.Batch.GetByMedicine, parameters);
        }

        public async Task<IEnumerable<Batch>> GetExpiryStatusAsync(int storeID, int redDays, int yellowDays)
        {
            var parameters = new { storeID, redDays, yellowDays };
            return await _db.QueryAsync<Batch>(StoredProcedureNames.Batch.GetExpiryStatus, parameters);
        }

        public async Task<IEnumerable<Batch>> GetExpiringAsync(int storeID, int days)
        {
            var parameters = new { storeID, days };
            return await _db.QueryAsync<Batch>(StoredProcedureNames.Batch.GetExpiring, parameters);
        }

        public async Task<IEnumerable<BatchesLowStockResponseDto>> GetLowStockAsync(int storeID)
        {
            // Note: this SP aggregates Medicines + Batches (reorder point check),
            // so it maps directly to a reporting DTO rather than the Batch domain model.
            var parameters = new { storeID };
            return await _db.QueryAsync<BatchesLowStockResponseDto>(StoredProcedureNames.Batch.GetLowStock, parameters);
        }

        public async Task ReduceStockAsync(int batchID, int quantity)
        {
            var parameters = new { batchID, quantity };
            await _db.ExecuteAsync(StoredProcedureNames.Batch.ReduceStock, parameters);
        }

        public async Task DeleteAsync(int batchID)
        {
            var parameters = new { batchID };
            await _db.ExecuteAsync(StoredProcedureNames.Batch.Delete, parameters);
        }
    }
}
