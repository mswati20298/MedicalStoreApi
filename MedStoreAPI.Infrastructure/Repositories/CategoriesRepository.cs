using MedStoreAPI.Common;
using MedStoreAPI.Domain;
using MedStoreAPI.Entities.Repositories;

namespace MedStoreAPI.Infrastructure.Repositories
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Data access implementation for Categories.
    /// </summary>
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly ISqlDataAccess _db;

        public CategoriesRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<Category> InsertAsync(Category category)
        {
            var parameters = new { categoryName = category.CategoryName, parentCategoryID = category.ParentCategoryId };
            var categoryID = await _db.QuerySingleAsync<int>(StoredProcedureNames.Category.Insert, parameters);
            category.CategoryId = categoryID;
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _db.QueryAsync<Category>(StoredProcedureNames.Category.GetAll);
        }

        public async Task UpdateAsync(Category category)
        {
            var parameters = new { categoryID = category.CategoryId, categoryName = category.CategoryName, parentCategoryID = category.ParentCategoryId };
            await _db.ExecuteAsync(StoredProcedureNames.Category.Update, parameters);
        }

        public async Task DeleteAsync(int categoryID)
        {
            var parameters = new { categoryID };
            await _db.ExecuteAsync(StoredProcedureNames.Category.Delete, parameters);
        }
    }
}
