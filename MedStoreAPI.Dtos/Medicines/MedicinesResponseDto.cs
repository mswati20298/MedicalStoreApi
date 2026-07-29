namespace MedStoreAPI.Dtos.Medicines
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Response DTO for Medicines module.
    /// </summary>
    public class MedicinesResponseDto
    {
        public int MedicineID { get; set; }
        public Guid MedicineUID { get; set; }
        public int StoreID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Composition { get; set; }
        public string? Manufacturer { get; set; }
        public string? CategoryName { get; set; }
        public string? UnitName { get; set; }
        public decimal? GSTPercentage { get; set; }
        public string? HSNCode { get; set; }
        public bool PrescriptionRequired { get; set; }
        public int ReorderPoint { get; set; }
        public int MaxStockLevel { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
