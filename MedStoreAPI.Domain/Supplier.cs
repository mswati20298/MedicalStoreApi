namespace MedStoreAPI.Domain
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Exact clone of the Suppliers table.
    /// </summary>
    public class Supplier
    {
        public int SupplierId { get; set; }
        public Guid SupplierUid { get; set; }
        public int StoreId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public string? ContactPerson { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public int ReturnPolicyDays { get; set; }
        public string? GSTIN { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
