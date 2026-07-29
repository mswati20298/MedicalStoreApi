namespace MedStoreAPI.Dtos.Stores
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Request/Response DTOs for Stores module. This is the
    /// per-medical-store profile (e.g. "Shiva Medical", "Health Care Pharmacy")
    /// - each store has its own name, address, GSTIN, and logo. Every other
    /// module (Customers, Medicines, Invoices, etc.) already takes a StoreID,
    /// so data is isolated per store - this module manages the store's own
    /// profile/branding.
    /// </summary>
    public class StoresRequestDto
    {
        public string StoreName { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Pincode { get; set; }
        public string? GSTIN { get; set; }
        public string? DrugLicenseNumber { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
    }

    public class StoresUpdateRequestDto : StoresRequestDto
    {
        public int StoreID { get; set; }
    }

    public class StoresResponseDto
    {
        public int StoreID { get; set; }
        public Guid StoreUID { get; set; }
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
        public DateTime CreatedDate { get; set; }
    }
}
