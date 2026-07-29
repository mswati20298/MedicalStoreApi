namespace MedStoreAPI.Domain
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Exact clone of the Batches table.
    /// </summary>
    public class Batch
    {
        public int BatchId { get; set; }
        public Guid BatchUid { get; set; }
        public int StoreId { get; set; }
        public int MedicineId { get; set; }
        public int? SupplierId { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public int QuantityReceived { get; set; }
        public int QuantityRemaining { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal MRP { get; set; }
        public DateTime DateReceived { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Joined fields
        public string? MedicineName { get; set; }
        public string? SupplierName { get; set; }
        public int? ReturnPolicyDays { get; set; }
        public int? DaysToExpiry { get; set; }
        public string? ExpiryStatus { get; set; }
    }
}
