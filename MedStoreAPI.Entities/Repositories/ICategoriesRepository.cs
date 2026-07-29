using MedStoreAPI.Domain;

namespace MedStoreAPI.Entities.Repositories
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Contract for Categories (master data) data access.
    /// </summary>
    public interface ICategoriesRepository
    {
        Task<Category> InsertAsync(Category category);
        Task<IEnumerable<Category>> GetAllAsync();
        Task UpdateAsync(Category category);
        Task DeleteAsync(int categoryID);
    }
}
