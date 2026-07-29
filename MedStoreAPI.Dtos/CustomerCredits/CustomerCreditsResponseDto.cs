namespace MedStoreAPI.Dtos.CustomerCredits
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Response DTO for CustomerCredits (Udhaar) module.
    /// </summary>
    public class CustomerCreditsResponseDto
    {
        public int CreditID { get; set; }
        public int StoreID { get; set; }
        public int CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public string? Mobile { get; set; }
        public int? InvoiceID { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Balance => Amount - AmountPaid;
        public DateTime? DueDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}
