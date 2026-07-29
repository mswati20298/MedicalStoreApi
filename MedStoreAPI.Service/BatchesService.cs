using MedStoreAPI.Common;
using MedStoreAPI.Domain;
using MedStoreAPI.Dtos.Batches;
using MedStoreAPI.Entities.Repositories;
using MedStoreAPI.Entities.Services;

namespace MedStoreAPI.Service
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Business logic for Batches (stock + expiry management).
    /// Maps between Request/Response DTOs and Domain models, wraps results in
    /// ApiResponse&lt;T&gt;.
    /// Naming rule: Entity/Table "Batches" -> Service class "BatchesService".
    /// </summary>
    public class BatchesService : IBatchesService
    {
        private readonly IBatchesRepository _batchesRepository;

        public BatchesService(IBatchesRepository batchesRepository)
        {
            _batchesRepository = batchesRepository;
        }

        public async Task<ApiResponse<BatchesResponseDto>> AddBatchAsync(BatchesRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.BatchNumber))
            {
                return ApiResponse<BatchesResponseDto>.Fail("Batch number is required.");
            }

            if (request.ExpiryDate <= request.DateReceived)
            {
                return ApiResponse<BatchesResponseDto>.Fail("Expiry date must be after the date received.");
            }

            var domainModel = new Batch
            {
                StoreId = request.StoreID,
                MedicineId = request.MedicineID,
                SupplierId = request.SupplierID,
                BatchNumber = request.BatchNumber,
                ExpiryDate = request.ExpiryDate,
                ManufactureDate = request.ManufactureDate,
                QuantityReceived = request.QuantityReceived,
                PurchasePrice = request.PurchasePrice,
                MRP = request.MRP,
                DateReceived = request.DateReceived
            };

            var inserted = await _batchesRepository.InsertAsync(domainModel);

            return ApiResponse<BatchesResponseDto>.Ok(MapToResponseDto(inserted), "Batch added successfully.");
        }

        public async Task<ApiResponse<IEnumerable<BatchesResponseDto>>> GetByMedicineAsync(int medicineID)
        {
            // FEFO order - repository already sorts by ExpiryDate ASC
            var batches = await _batchesRepository.GetByMedicineAsync(medicineID);
            var response = batches.Select(MapToResponseDto);

            return ApiResponse<IEnumerable<BatchesResponseDto>>.Ok(response);
        }

        public async Task<ApiResponse<IEnumerable<BatchesExpiryStatusResponseDto>>> GetExpiryStatusAsync(BatchesExpiryStatusRequestDto request)
        {
            var batches = await _batchesRepository.GetExpiryStatusAsync(request.StoreID, request.RedDays, request.YellowDays);
            var response = batches.Select(MapToExpiryStatusDto);

            return ApiResponse<IEnumerable<BatchesExpiryStatusResponseDto>>.Ok(response);
        }

        public async Task<ApiResponse<IEnumerable<BatchesResponseDto>>> GetExpiringAsync(BatchesExpiringRequestDto request)
        {
            var batches = await _batchesRepository.GetExpiringAsync(request.StoreID, request.Days);
            var response = batches.Select(MapToResponseDto);

            return ApiResponse<IEnumerable<BatchesResponseDto>>.Ok(response);
        }

        public async Task<ApiResponse<IEnumerable<BatchesLowStockResponseDto>>> GetLowStockAsync(int storeID)
        {
            var lowStock = await _batchesRepository.GetLowStockAsync(storeID);
            return ApiResponse<IEnumerable<BatchesLowStockResponseDto>>.Ok(lowStock);
        }

        public async Task<ApiResponse<bool>> DeleteBatchAsync(int batchID, int storeID)
        {
            var existing = await _batchesRepository.GetByIDAsync(batchID);
            if (existing is null)
            {
                return ApiResponse<bool>.Fail("Batch not found.");
            }

            if (existing.StoreId != storeID)
            {
                return ApiResponse<bool>.Fail("You do not have permission to delete this batch.");
            }

            await _batchesRepository.DeleteAsync(batchID);
            return ApiResponse<bool>.Ok(true, "Batch deleted successfully.");
        }

        private static BatchesResponseDto MapToResponseDto(Batch batch)
        {
            return new BatchesResponseDto
            {
                BatchID = batch.BatchId,
                BatchUID = batch.BatchUid,
                StoreID = batch.StoreId,
                MedicineID = batch.MedicineId,
                MedicineName = batch.MedicineName,
                SupplierID = batch.SupplierId,
                SupplierName = batch.SupplierName,
                BatchNumber = batch.BatchNumber,
                ExpiryDate = batch.ExpiryDate,
                ManufactureDate = batch.ManufactureDate,
                QuantityReceived = batch.QuantityReceived,
                QuantityRemaining = batch.QuantityRemaining,
                PurchasePrice = batch.PurchasePrice,
                MRP = batch.MRP,
                DateReceived = batch.DateReceived
            };
        }

        private static BatchesExpiryStatusResponseDto MapToExpiryStatusDto(Batch batch)
        {
            return new BatchesExpiryStatusResponseDto
            {
                BatchID = batch.BatchId,
                BatchNumber = batch.BatchNumber,
                ExpiryDate = batch.ExpiryDate,
                QuantityRemaining = batch.QuantityRemaining,
                MedicineID = batch.MedicineId,
                MedicineName = batch.MedicineName,
                SupplierName = batch.SupplierName,
                ReturnPolicyDays = batch.ReturnPolicyDays,
                DaysToExpiry = batch.DaysToExpiry ?? 0,
                ExpiryStatus = batch.ExpiryStatus ?? string.Empty
            };
        }
    }
}
