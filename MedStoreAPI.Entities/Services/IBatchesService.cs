using MedStoreAPI.Common;
using MedStoreAPI.Dtos.Batches;

namespace MedStoreAPI.Entities.Services
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Contract for Batches business logic. Works with Request/Response DTOs.
    /// Implemented by MedStoreAPI.Service.BatchesService.
    /// Naming rule: Entity/Table "Batches" -> Service class "BatchesService".
    /// </summary>
    public interface IBatchesService
    {
        Task<ApiResponse<BatchesResponseDto>> AddBatchAsync(BatchesRequestDto request);
        Task<ApiResponse<IEnumerable<BatchesResponseDto>>> GetByMedicineAsync(int medicineID);
        Task<ApiResponse<IEnumerable<BatchesExpiryStatusResponseDto>>> GetExpiryStatusAsync(BatchesExpiryStatusRequestDto request);
        Task<ApiResponse<IEnumerable<BatchesResponseDto>>> GetExpiringAsync(BatchesExpiringRequestDto request);
        Task<ApiResponse<IEnumerable<BatchesLowStockResponseDto>>> GetLowStockAsync(int storeID);
        Task<ApiResponse<bool>> DeleteBatchAsync(int batchID, int storeID);
    }
}
