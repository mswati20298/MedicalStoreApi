namespace MedStoreAPI.Dtos.Suppliers
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Response DTO for Suppliers module.
    /// </summary>
    public class SuppliersResponseDto
    {
        public int SupplierID { get; set; }
        public Guid SupplierUID { get; set; }
        public int StoreID { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public string? ContactPerson { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public int ReturnPolicyDays { get; set; }
        public string? GSTIN { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
