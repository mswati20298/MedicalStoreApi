using MedStoreAPI.Common;
using MedStoreAPI.Dtos.CustomerCredits;

namespace MedStoreAPI.Entities.Services
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Contract for CustomerCredits (Udhaar) business logic.
    /// Implemented by MedStoreAPI.Service.CustomerCreditsService.
    /// Naming rule: Entity/Table "CustomerCredits" -> Service class "CustomerCreditsService".
    /// </summary>
    public interface ICustomerCreditsService
    {
        Task<ApiResponse<CustomerCreditsResponseDto>> AddCreditAsync(CustomerCreditsRequestDto request);
        Task<ApiResponse<IEnumerable<CustomerCreditsResponseDto>>> GetPendingAsync(int storeID);
        Task<ApiResponse<IEnumerable<CustomerCreditsResponseDto>>> GetByCustomerAsync(int customerID, int storeID);
        Task<ApiResponse<bool>> AddPaymentAsync(CustomerCreditsAddPaymentRequestDto request, int storeID);
    }
}
