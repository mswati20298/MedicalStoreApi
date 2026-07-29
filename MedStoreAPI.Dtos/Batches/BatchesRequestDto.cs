namespace MedStoreAPI.Dtos.Batches
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Request DTOs for Batches module.
    /// </summary>
    public class BatchesRequestDto
    {
        public int StoreID { get; set; }
        public int MedicineID { get; set; }
        public int? SupplierID { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public int QuantityReceived { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal MRP { get; set; }
        public DateTime DateReceived { get; set; }
    }

    public class BatchesExpiryStatusRequestDto
    {
        public int StoreID { get; set; }
        public int RedDays { get; set; } = 30;
        public int YellowDays { get; set; } = 90;
    }

    public class BatchesExpiringRequestDto
    {
        public int StoreID { get; set; }
        public int Days { get; set; } = 30;
    }
}
