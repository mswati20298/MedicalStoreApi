namespace MedStoreAPI.Dtos.Invoices
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Response DTOs for Invoices/Billing module.
    /// </summary>
    public class InvoicesResponseDto
    {
        public int InvoiceID { get; set; }
        public Guid InvoiceUID { get; set; }
        public int StoreID { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int? CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerMobile { get; set; }
        public string? ModeName { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalGST { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime InvoiceDate { get; set; }
        public List<InvoiceItemResponseDto> Items { get; set; } = new();
    }

    public class InvoiceItemResponseDto
    {
        public int InvoiceItemID { get; set; }
        public int BatchID { get; set; }
        public string? BatchNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int MedicineID { get; set; }
        public string? MedicineName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal GSTPercentage { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal LineTotal { get; set; }
    }

    /// <summary>
    /// Used for SP_InvoiceGetDailySummary - an aggregate report, not a table clone.
    /// </summary>
    public class InvoiceDailySummaryResponseDto
    {
        public int TotalInvoices { get; set; }
        public decimal TotalSubTotal { get; set; }
        public decimal TotalGSTCollected { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalSales { get; set; }
    }
}
