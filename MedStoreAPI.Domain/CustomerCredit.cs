namespace MedStoreAPI.Domain
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Exact clone of the CustomerCredits table (Udhaar tracking).
    /// </summary>
    public class CustomerCredit
    {
        public int CreditId { get; set; }
        public int StoreId { get; set; }
        public int CustomerId { get; set; }
        public int? InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Joined fields
        public string? CustomerName { get; set; }
        public string? Mobile { get; set; }
    }

    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Exact clone of the CreditPayments table.
    /// </summary>
    public class CreditPayment
    {
        public int CreditPaymentId { get; set; }
        public int CreditId { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public int? PaymentModeId { get; set; }
    }
}
