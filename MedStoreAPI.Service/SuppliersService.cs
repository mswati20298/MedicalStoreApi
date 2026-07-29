using MedStoreAPI.Common;
using MedStoreAPI.Domain;
using MedStoreAPI.Dtos.Suppliers;
using MedStoreAPI.Entities.Repositories;
using MedStoreAPI.Entities.Services;

namespace MedStoreAPI.Service
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Business logic for Suppliers. Maps between Request/Response
    /// DTOs and Domain models, wraps results in ApiResponse&lt;T&gt;.
    /// Naming rule: Entity/Table "Suppliers" -> Service class "SuppliersService".
    /// </summary>
    public class SuppliersService : ISuppliersService
    {
        private readonly ISuppliersRepository _suppliersRepository;

        public SuppliersService(ISuppliersRepository suppliersRepository)
        {
            _suppliersRepository = suppliersRepository;
        }

        public async Task<ApiResponse<SuppliersResponseDto>> AddSupplierAsync(SuppliersRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.SupplierName))
            {
                return ApiResponse<SuppliersResponseDto>.Fail("Supplier name is required.");
            }

            var domainModel = new Supplier
            {
                StoreId = request.StoreID,
                SupplierName = request.SupplierName,
                ContactPerson = request.ContactPerson,
                ContactNumber = request.ContactNumber,
                Email = request.Email,
                Address = request.Address,
                ReturnPolicyDays = request.ReturnPolicyDays,
                GSTIN = request.GSTIN
            };

            var inserted = await _suppliersRepository.InsertAsync(domainModel);

            return ApiResponse<SuppliersResponseDto>.Ok(MapToResponseDto(inserted), "Supplier saved successfully.");
        }

        public async Task<ApiResponse<SuppliersResponseDto>> UpdateSupplierAsync(SuppliersUpdateRequestDto request)
        {
            var existing = await _suppliersRepository.GetByIDAsync(request.SupplierID);
            if (existing is null)
            {
                return ApiResponse<SuppliersResponseDto>.Fail("Supplier not found.");
            }

            if (existing.StoreId != request.StoreID)
            {
                return ApiResponse<SuppliersResponseDto>.Fail("You do not have permission to modify this supplier.");
            }

            var domainModel = new Supplier
            {
                SupplierId = request.SupplierID,
                SupplierName = request.SupplierName,
                ContactPerson = request.ContactPerson,
                ContactNumber = request.ContactNumber,
                Email = request.Email,
                Address = request.Address,
                ReturnPolicyDays = request.ReturnPolicyDays,
                GSTIN = request.GSTIN
            };

            await _suppliersRepository.UpdateAsync(domainModel);

            var updated = await _suppliersRepository.GetByIDAsync(request.SupplierID);
            return ApiResponse<SuppliersResponseDto>.Ok(MapToResponseDto(updated!), "Supplier updated successfully.");
        }

        public async Task<ApiResponse<SuppliersResponseDto>> GetSupplierByIDAsync(int supplierID, int storeID)
        {
            var supplier = await _suppliersRepository.GetByIDAsync(supplierID);
            if (supplier is null || supplier.StoreId != storeID)
            {
                return ApiResponse<SuppliersResponseDto>.Fail("Supplier not found.");
            }

            return ApiResponse<SuppliersResponseDto>.Ok(MapToResponseDto(supplier));
        }

        public async Task<ApiResponse<IEnumerable<SuppliersResponseDto>>> GetAllSuppliersAsync(int storeID)
        {
            var suppliers = await _suppliersRepository.GetAllAsync(storeID);
            var response = suppliers.Select(MapToResponseDto);

            return ApiResponse<IEnumerable<SuppliersResponseDto>>.Ok(response);
        }

        public async Task<ApiResponse<bool>> DeleteSupplierAsync(int supplierID, int storeID)
        {
            var existing = await _suppliersRepository.GetByIDAsync(supplierID);
            if (existing is null)
            {
                return ApiResponse<bool>.Fail("Supplier not found.");
            }

            if (existing.StoreId != storeID)
            {
                return ApiResponse<bool>.Fail("You do not have permission to delete this supplier.");
            }

            await _suppliersRepository.DeleteAsync(supplierID);
            return ApiResponse<bool>.Ok(true, "Supplier deleted successfully.");
        }

        private static SuppliersResponseDto MapToResponseDto(Supplier supplier)
        {
            return new SuppliersResponseDto
            {
                SupplierID = supplier.SupplierId,
                SupplierUID = supplier.SupplierUid,
                StoreID = supplier.StoreId,
                SupplierName = supplier.SupplierName,
                ContactPerson = supplier.ContactPerson,
                ContactNumber = supplier.ContactNumber,
                Email = supplier.Email,
                Address = supplier.Address,
                ReturnPolicyDays = supplier.ReturnPolicyDays,
                GSTIN = supplier.GSTIN,
                CreatedDate = supplier.CreatedDate
            };
        }
    }
}
