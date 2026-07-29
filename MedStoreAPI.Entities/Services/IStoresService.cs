using MedStoreAPI.Common;
using MedStoreAPI.Dtos.Stores;
using Microsoft.AspNetCore.Http;

namespace MedStoreAPI.Entities.Services
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Contract for Stores business logic - profile management
    /// and logo upload for each medical store. Implemented by
    /// MedStoreAPI.Service.StoresService.
    /// Naming rule: Entity/Table "Stores" -> Service class "StoresService".
    /// </summary>
    public interface IStoresService
    {
        Task<ApiResponse<StoresResponseDto>> AddStoreAsync(StoresRequestDto request);
        Task<ApiResponse<StoresResponseDto>> GetStoreByIDAsync(int storeID);
        Task<ApiResponse<IEnumerable<StoresResponseDto>>> GetAllStoresAsync();
        Task<ApiResponse<StoresResponseDto>> UpdateStoreAsync(StoresUpdateRequestDto request);
        Task<ApiResponse<string>> UploadLogoAsync(int storeID, IFormFile logoFile);
    }
}
