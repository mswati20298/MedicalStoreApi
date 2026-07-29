using MedStoreAPI.Common;
using MedStoreAPI.Domain;
using MedStoreAPI.Dtos.Categories;
using MedStoreAPI.Entities.Repositories;
using MedStoreAPI.Entities.Services;

namespace MedStoreAPI.Service
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Business logic for Categories (master data).
    /// Naming rule: Entity/Table "Categories" -> Service class "CategoriesService".
    /// </summary>
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public CategoriesService(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        public async Task<ApiResponse<CategoriesResponseDto>> AddAsync(CategoriesRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.CategoryName))
            {
                return ApiResponse<CategoriesResponseDto>.Fail("Category name is required.");
            }

            var domainModel = new Category { CategoryName = request.CategoryName, ParentCategoryId = request.ParentCategoryID };
            var inserted = await _categoriesRepository.InsertAsync(domainModel);

            return ApiResponse<CategoriesResponseDto>.Ok(MapToResponseDto(inserted), "Category added successfully.");
        }

        public async Task<ApiResponse<IEnumerable<CategoriesResponseDto>>> GetAllAsync()
        {
            var categories = await _categoriesRepository.GetAllAsync();
            return ApiResponse<IEnumerable<CategoriesResponseDto>>.Ok(categories.Select(MapToResponseDto));
        }

        public async Task<ApiResponse<bool>> UpdateAsync(CategoriesUpdateRequestDto request)
        {
            var domainModel = new Category { CategoryId = request.CategoryID, CategoryName = request.CategoryName, ParentCategoryId = request.ParentCategoryID };
            await _categoriesRepository.UpdateAsync(domainModel);
            return ApiResponse<bool>.Ok(true, "Category updated successfully.");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int categoryID)
        {
            await _categoriesRepository.DeleteAsync(categoryID);
            return ApiResponse<bool>.Ok(true, "Category deleted successfully.");
        }

        private static CategoriesResponseDto MapToResponseDto(Category category)
        {
            return new CategoriesResponseDto
            {
                CategoryID = category.CategoryId,
                CategoryName = category.CategoryName,
                ParentCategoryID = category.ParentCategoryId
            };
        }
    }
}
