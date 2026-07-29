using MedStoreAPI.Common;
using MedStoreAPI.Domain;
using MedStoreAPI.Dtos.CustomerCredits;
using MedStoreAPI.Entities.Repositories;
using MedStoreAPI.Entities.Services;

namespace MedStoreAPI.Service
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Business logic for CustomerCredits (Udhaar). Maps between
    /// Request/Response DTOs and Domain models, wraps results in ApiResponse&lt;T&gt;.
    /// Naming rule: Entity/Table "CustomerCredits" -> Service class "CustomerCreditsService".
    /// </summary>
    public class CustomerCreditsService : ICustomerCreditsService
    {
        private readonly ICustomerCreditsRepository _customerCreditsRepository;

        public CustomerCreditsService(ICustomerCreditsRepository customerCreditsRepository)
        {
            _customerCreditsRepository = customerCreditsRepository;
        }

        public async Task<ApiResponse<CustomerCreditsResponseDto>> AddCreditAsync(CustomerCreditsRequestDto request)
        {
            if (request.Amount <= 0)
            {
                return ApiResponse<CustomerCreditsResponseDto>.Fail("Credit amount must be greater than zero.");
            }

            var domainModel = new CustomerCredit
            {
                StoreId = request.StoreID,
                CustomerId = request.CustomerID,
                InvoiceId = request.InvoiceID,
                Amount = request.Amount,
                DueDate = request.DueDate,
                Status = "Pending"
            };

            var inserted = await _customerCreditsRepository.InsertAsync(domainModel);

            return ApiResponse<CustomerCreditsResponseDto>.Ok(MapToResponseDto(inserted), "Credit entry recorded successfully.");
        }

        public async Task<ApiResponse<IEnumerable<CustomerCreditsResponseDto>>> GetPendingAsync(int storeID)
        {
            var credits = await _customerCreditsRepository.GetPendingAsync(storeID);
            var response = credits.Select(MapToResponseDto);

            return ApiResponse<IEnumerable<CustomerCreditsResponseDto>>.Ok(response);
        }

        public async Task<ApiResponse<IEnumerable<CustomerCreditsResponseDto>>> GetByCustomerAsync(int customerID, int storeID)
        {
            var credits = await _customerCreditsRepository.GetByCustomerAsync(customerID);
            // Filter out any credit that doesn't belong to the caller's store
            // (customerID alone doesn't guarantee store ownership).
            var response = credits.Where(c => c.StoreId == storeID).Select(MapToResponseDto);

            return ApiResponse<IEnumerable<CustomerCreditsResponseDto>>.Ok(response);
        }

        public async Task<ApiResponse<bool>> AddPaymentAsync(CustomerCreditsAddPaymentRequestDto request, int storeID)
        {
            if (request.AmountPaid <= 0)
            {
                return ApiResponse<bool>.Fail("Payment amount must be greater than zero.");
            }

            var credit = await _customerCreditsRepository.GetByIDAsync(request.CreditID);
            if (credit is null || credit.StoreId != storeID)
            {
                return ApiResponse<bool>.Fail("Credit entry not found.");
            }

            await _customerCreditsRepository.AddPaymentAsync(request.CreditID, request.AmountPaid, request.PaymentModeID);
            return ApiResponse<bool>.Ok(true, "Payment recorded successfully.");
        }

        private static CustomerCreditsResponseDto MapToResponseDto(CustomerCredit credit)
        {
            return new CustomerCreditsResponseDto
            {
                CreditID = credit.CreditId,
                StoreID = credit.StoreId,
                CustomerID = credit.CustomerId,
                CustomerName = credit.CustomerName,
                Mobile = credit.Mobile,
                InvoiceID = credit.InvoiceId,
                Amount = credit.Amount,
                AmountPaid = credit.AmountPaid,
                DueDate = credit.DueDate,
                Status = credit.Status,
                CreatedDate = credit.CreatedDate
            };
        }
    }
}
