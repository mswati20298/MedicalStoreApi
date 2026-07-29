namespace MedStoreAPI.Domain
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Exact clone of the Medicines table.
    /// </summary>
    public class Medicine
    {
        public int MedicineId { get; set; }
        public Guid MedicineUid { get; set; }
        public int StoreId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Composition { get; set; }
        public string? Manufacturer { get; set; }
        public int? CategoryId { get; set; }
        public int? UnitId { get; set; }
        public int? GSTSlabId { get; set; }
        public string? HSNCode { get; set; }
        public bool PrescriptionRequired { get; set; }
        public int ReorderPoint { get; set; }
        public int MaxStockLevel { get; set; }
        public string? ExtraAttributes { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Joined/lookup fields returned by SP_MedicineGetAll / SP_MedicineGetByID
        public string? CategoryName { get; set; }
        public string? UnitName { get; set; }
        public decimal? GSTPercentage { get; set; }
    }
}
