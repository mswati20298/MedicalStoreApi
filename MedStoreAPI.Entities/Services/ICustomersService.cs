using MedStoreAPI.Common;
using MedStoreAPI.Dtos.Customers;

namespace MedStoreAPI.Entities.Services
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Contract for Customers business logic. Works with Request/Response
    /// DTOs (API-facing shape). Implemented by MedStoreAPI.Service.CustomersService.
    /// Naming rule: Entity/Table name "Customers" -> Service name "CustomersService".
    /// </summary>
    public interface ICustomersService
    {
        Task<ApiResponse<CustomersResponseDto>> AddCustomerAsync(CustomersRequestDto request);
        Task<ApiResponse<CustomersResponseDto>> GetCustomerByMobileAsync(CustomersGetByMobileRequestDto request);
        Task<ApiResponse<IEnumerable<CustomersResponseDto>>> GetAllCustomersAsync(int storeID);
    }
}
