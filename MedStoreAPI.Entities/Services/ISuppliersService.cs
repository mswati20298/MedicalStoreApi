using MedStoreAPI.Common;
using MedStoreAPI.Dtos.Suppliers;

namespace MedStoreAPI.Entities.Services
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Contract for Suppliers business logic. Works with Request/Response DTOs.
    /// Implemented by MedStoreAPI.Service.SuppliersService.
    /// Naming rule: Entity/Table "Suppliers" -> Service class "SuppliersService".
    /// </summary>
    public interface ISuppliersService
    {
        Task<ApiResponse<SuppliersResponseDto>> AddSupplierAsync(SuppliersRequestDto request);
        Task<ApiResponse<SuppliersResponseDto>> UpdateSupplierAsync(SuppliersUpdateRequestDto request);
        Task<ApiResponse<SuppliersResponseDto>> GetSupplierByIDAsync(int supplierID, int storeID);
        Task<ApiResponse<IEnumerable<SuppliersResponseDto>>> GetAllSuppliersAsync(int storeID);
        Task<ApiResponse<bool>> DeleteSupplierAsync(int supplierID, int storeID);
    }
}
