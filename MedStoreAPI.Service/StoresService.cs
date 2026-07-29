using MedStoreAPI.Common;
using MedStoreAPI.Domain;
using MedStoreAPI.Dtos.Stores;
using MedStoreAPI.Entities.Repositories;
using MedStoreAPI.Entities.Services;
using Microsoft.AspNetCore.Http;

namespace MedStoreAPI.Service
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Business logic for Stores - profile management for each
    /// medical store (e.g. "Shiva Medical", "Health Care Pharmacy") and logo
    /// upload. Logos are saved to wwwroot/uploads/logos/{storeID}_{guid}.{ext}
    /// and the relative URL is stored in Stores.LogoUrl.
    /// Naming rule: Entity/Table "Stores" -> Service class "StoresService".
    /// </summary>
    public class StoresService : IStoresService
    {
        private readonly IStoresRepository _storesRepository;
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxLogoSizeBytes = 2 * 1024 * 1024; // 2 MB

        public StoresService(IStoresRepository storesRepository)
        {
            _storesRepository = storesRepository;
        }

        public async Task<ApiResponse<StoresResponseDto>> AddStoreAsync(StoresRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.StoreName))
            {
                return ApiResponse<StoresResponseDto>.Fail("Store name is required.");
            }

            var domainModel = new Store
            {
                StoreName = request.StoreName,
                Address = request.Address,
                City = request.City,
                State = request.State,
                Pincode = request.Pincode,
                GSTIN = request.GSTIN,
                DrugLicenseNumber = request.DrugLicenseNumber,
                ContactNumber = request.ContactNumber,
                Email = request.Email
            };

            var inserted = await _storesRepository.InsertAsync(domainModel);

            return ApiResponse<StoresResponseDto>.Ok(MapToResponseDto(inserted), "Store created successfully.");
        }

        public async Task<ApiResponse<StoresResponseDto>> GetStoreByIDAsync(int storeID)
        {
            var store = await _storesRepository.GetByIDAsync(storeID);
            if (store is null)
            {
                return ApiResponse<StoresResponseDto>.Fail("Store not found.");
            }

            return ApiResponse<StoresResponseDto>.Ok(MapToResponseDto(store));
        }

        public async Task<ApiResponse<IEnumerable<StoresResponseDto>>> GetAllStoresAsync()
        {
            var stores = await _storesRepository.GetAllAsync();
            var response = stores.Select(MapToResponseDto);

            return ApiResponse<IEnumerable<StoresResponseDto>>.Ok(response);
        }

        public async Task<ApiResponse<StoresResponseDto>> UpdateStoreAsync(StoresUpdateRequestDto request)
        {
            var existing = await _storesRepository.GetByIDAsync(request.StoreID);
            if (existing is null)
            {
                return ApiResponse<StoresResponseDto>.Fail("Store not found.");
            }

            var domainModel = new Store
            {
                StoreId = request.StoreID,
                StoreName = request.StoreName,
                Address = request.Address,
                City = request.City,
                State = request.State,
                Pincode = request.Pincode,
                GSTIN = request.GSTIN,
                DrugLicenseNumber = request.DrugLicenseNumber,
                ContactNumber = request.ContactNumber,
                Email = request.Email,
                LogoUrl = existing.LogoUrl // logo is updated separately via UploadLogoAsync
            };

            await _storesRepository.UpdateAsync(domainModel);

            var updated = await _storesRepository.GetByIDAsync(request.StoreID);
            return ApiResponse<StoresResponseDto>.Ok(MapToResponseDto(updated!), "Store updated successfully.");
        }

        public async Task<ApiResponse<string>> UploadLogoAsync(int storeID, IFormFile logoFile)
        {
            var existing = await _storesRepository.GetByIDAsync(storeID);
            if (existing is null)
            {
                return ApiResponse<string>.Fail("Store not found.");
            }

            if (logoFile is null || logoFile.Length == 0)
            {
                return ApiResponse<string>.Fail("Logo file is required.");
            }

            if (logoFile.Length > MaxLogoSizeBytes)
            {
                return ApiResponse<string>.Fail("Logo file must be under 2 MB.");
            }

            var extension = Path.GetExtension(logoFile.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
            {
                return ApiResponse<string>.Fail("Only .jpg, .jpeg, .png, .webp files are allowed.");
            }

            var uploadsFolder = Path.Combine(AppContext.BaseDirectory, "wwwroot", "uploads", "logos");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{storeID}_{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await logoFile.CopyToAsync(stream);
            }

            var logoUrl = $"/uploads/logos/{fileName}";
            await _storesRepository.UpdateLogoAsync(storeID, logoUrl);

            return ApiResponse<string>.Ok(logoUrl, "Logo uploaded successfully.");
        }

        private static StoresResponseDto MapToResponseDto(Store store)
        {
            return new StoresResponseDto
            {
                StoreID = store.StoreId,
                StoreUID = store.StoreUid,
                StoreName = store.StoreName,
                Address = store.Address,
                City = store.City,
                State = store.State,
                Pincode = store.Pincode,
                GSTIN = store.GSTIN,
                DrugLicenseNumber = store.DrugLicenseNumber,
                ContactNumber = store.ContactNumber,
                Email = store.Email,
                LogoUrl = store.LogoUrl,
                CreatedDate = store.CreatedDate
            };
        }
    }
}
