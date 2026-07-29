namespace MedStoreAPI.Dtos.CustomerCredits
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Request DTOs for CustomerCredits (Udhaar) module.
    /// </summary>
    public class CustomerCreditsRequestDto
    {
        public int StoreID { get; set; }
        public int CustomerID { get; set; }
        public int? InvoiceID { get; set; }
        public decimal Amount { get; set; }
        public DateTime? DueDate { get; set; }
    }

    public class CustomerCreditsAddPaymentRequestDto
    {
        public int CreditID { get; set; }
        public decimal AmountPaid { get; set; }
        public int PaymentModeID { get; set; }
    }
}
