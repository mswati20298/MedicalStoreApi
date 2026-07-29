using MedStoreAPI.Common;
using MedStoreAPI.Domain;
using MedStoreAPI.Dtos.Customers;
using MedStoreAPI.Entities.Repositories;
using MedStoreAPI.Entities.Services;

namespace MedStoreAPI.Service
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Business logic for Customers. Maps between Request/Response
    /// DTOs (API-facing) and Domain models (DB-facing), and wraps every result
    /// in the common ApiResponse&lt;T&gt; wrapper.
    /// Naming rule: Entity/Table "Customers" -> Service class "CustomersService".
    /// </summary>
    public class CustomersService : ICustomersService
    {
        private readonly ICustomersRepository _customersRepository;

        public CustomersService(ICustomersRepository customersRepository)
        {
            _customersRepository = customersRepository;
        }

        public async Task<ApiResponse<CustomersResponseDto>> AddCustomerAsync(CustomersRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Mobile))
            {
                return ApiResponse<CustomersResponseDto>.Fail("Mobile number is required.");
            }

            var domainModel = new Customer
            {
                StoreId = request.StoreID,
                Name = request.Name,
                Mobile = request.Mobile,
                Address = request.Address
            };

            var inserted = await _customersRepository.InsertAsync(domainModel);

            return ApiResponse<CustomersResponseDto>.Ok(MapToResponseDto(inserted), "Customer saved successfully.");
        }

        public async Task<ApiResponse<CustomersResponseDto>> GetCustomerByMobileAsync(CustomersGetByMobileRequestDto request)
        {
            var customer = await _customersRepository.GetByMobileAsync(request.StoreID, request.Mobile);

            if (customer is null)
            {
                return ApiResponse<CustomersResponseDto>.Fail("Customer not found.");
            }

            return ApiResponse<CustomersResponseDto>.Ok(MapToResponseDto(customer));
        }

        public async Task<ApiResponse<IEnumerable<CustomersResponseDto>>> GetAllCustomersAsync(int storeID)
        {
            var customers = await _customersRepository.GetAllAsync(storeID);
            var response = customers.Select(MapToResponseDto);

            return ApiResponse<IEnumerable<CustomersResponseDto>>.Ok(response);
        }

        private static CustomersResponseDto MapToResponseDto(Customer customer)
        {
            return new CustomersResponseDto
            {
                CustomerID = customer.CustomerId,
                CustomerUID = customer.CustomerUid,
                StoreID = customer.StoreId,
                Name = customer.Name,
                Mobile = customer.Mobile,
                Address = customer.Address,
                CreatedDate = customer.CreatedDate
            };
        }
    }
}
