namespace MedStoreAPI.Domain
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Exact clone of the Invoices table.
    /// </summary>
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public Guid InvoiceUid { get; set; }
        public int StoreId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int? CustomerId { get; set; }
        public int PaymentModeId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalGST { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        // Joined fields
        public string? CustomerName { get; set; }
        public string? CustomerMobile { get; set; }
        public string? ModeName { get; set; }
    }

    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Exact clone of the InvoiceItems table.
    /// </summary>
    public class InvoiceItem
    {
        public int InvoiceItemId { get; set; }
        public int InvoiceId { get; set; }
        public int BatchId { get; set; }
        public int MedicineId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal GSTPercentage { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal LineTotal { get; set; }

        // Joined fields
        public string? MedicineName { get; set; }
        public string? BatchNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Not a table clone - this maps the (Success, Message) result
    /// returned by SP_InvoiceItemInsertAndReduceStock, used internally by
    /// InvoicesRepository to detect insufficient-stock failures inside a transaction.
    /// </summary>
    public class InvoiceItemStockResult
    {
        public int Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Not a table clone - aggregate result of SP_InvoiceGetDailySummary.
    /// </summary>
    public class InvoiceDailySummary
    {
        public int TotalInvoices { get; set; }
        public decimal TotalSubTotal { get; set; }
        public decimal TotalGSTCollected { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalSales { get; set; }
    }
}
