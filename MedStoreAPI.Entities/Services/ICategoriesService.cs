using MedStoreAPI.Common;
using MedStoreAPI.Dtos.Categories;

namespace MedStoreAPI.Entities.Services
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Contract for Categories (master data) business logic.
    /// Naming rule: Entity/Table "Categories" -> Service class "CategoriesService".
    /// </summary>
    public interface ICategoriesService
    {
        Task<ApiResponse<CategoriesResponseDto>> AddAsync(CategoriesRequestDto request);
        Task<ApiResponse<IEnumerable<CategoriesResponseDto>>> GetAllAsync();
        Task<ApiResponse<bool>> UpdateAsync(CategoriesUpdateRequestDto request);
        Task<ApiResponse<bool>> DeleteAsync(int categoryID);
    }
}
