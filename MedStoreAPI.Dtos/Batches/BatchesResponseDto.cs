namespace MedStoreAPI.Dtos.Batches
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Response DTOs for Batches module.
    /// </summary>
    public class BatchesResponseDto
    {
        public int BatchID { get; set; }
        public Guid BatchUID { get; set; }
        public int StoreID { get; set; }
        public int MedicineID { get; set; }
        public string? MedicineName { get; set; }
        public int? SupplierID { get; set; }
        public string? SupplierName { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public int QuantityReceived { get; set; }
        public int QuantityRemaining { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal MRP { get; set; }
        public DateTime DateReceived { get; set; }
    }

    /// <summary>
    /// Used for SP_BatchGetExpiryStatus - includes Red/Yellow/Green classification.
    /// </summary>
    public class BatchesExpiryStatusResponseDto
    {
        public int BatchID { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public int QuantityRemaining { get; set; }
        public int MedicineID { get; set; }
        public string? MedicineName { get; set; }
        public string? SupplierName { get; set; }
        public int? ReturnPolicyDays { get; set; }
        public int DaysToExpiry { get; set; }
        public string ExpiryStatus { get; set; } = string.Empty; // RED / YELLOW / GREEN
    }

    /// <summary>
    /// Used for SP_BatchGetLowStock - medicines below reorder point.
    /// </summary>
    public class BatchesLowStockResponseDto
    {
        public int MedicineID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ReorderPoint { get; set; }
        public int MaxStockLevel { get; set; }
        public int CurrentStock { get; set; }
    }
}
