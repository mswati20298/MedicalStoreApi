namespace MedStoreAPI.Dtos.Categories
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Request/Response DTOs for Categories (master data) module.
    /// </summary>
    public class CategoriesRequestDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public int? ParentCategoryID { get; set; }
    }

    public class CategoriesUpdateRequestDto : CategoriesRequestDto
    {
        public int CategoryID { get; set; }
    }

    public class CategoriesResponseDto
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int? ParentCategoryID { get; set; }
    }
}
