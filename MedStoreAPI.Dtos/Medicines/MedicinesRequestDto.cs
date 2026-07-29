namespace MedStoreAPI.Dtos.Medicines
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Request DTOs for Medicines module.
    /// </summary>
    public class MedicinesRequestDto
    {
        public int StoreID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Composition { get; set; }
        public string? Manufacturer { get; set; }
        public int? CategoryID { get; set; }
        public int? UnitID { get; set; }
        public int? GSTSlabID { get; set; }
        public string? HSNCode { get; set; }
        public bool PrescriptionRequired { get; set; }
        public int ReorderPoint { get; set; }
        public int MaxStockLevel { get; set; }
    }

    public class MedicinesUpdateRequestDto : MedicinesRequestDto
    {
        public int MedicineID { get; set; }
    }

    public class MedicinesSearchRequestDto
    {
        public int StoreID { get; set; }
        public string SearchTerm { get; set; } = string.Empty;
    }
}
