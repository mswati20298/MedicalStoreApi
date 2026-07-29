namespace MedStoreAPI.Domain
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Exact clones of master/lookup tables - Stores, Roles,
    /// Categories, Units, GSTSlabs, PaymentModes. Grouped in one file since
    /// these are small, simple lookup entities.
    /// </summary>
    public class Store
    {
        public int StoreId { get; set; }
        public Guid StoreUid { get; set; }
        public string StoreName { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Pincode { get; set; }
        public string? GSTIN { get; set; }
        public string? DrugLicenseNumber { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? LogoUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int? ParentCategoryId { get; set; }
        public bool IsActive { get; set; }
    }

    public class Unit
    {
        public int UnitId { get; set; }
        public string UnitName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class GSTSlab
    {
        public int GSTSlabId { get; set; }
        public decimal Percentage { get; set; }
        public bool IsActive { get; set; }
    }

    public class PaymentMode
    {
        public int PaymentModeId { get; set; }
        public string ModeName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
