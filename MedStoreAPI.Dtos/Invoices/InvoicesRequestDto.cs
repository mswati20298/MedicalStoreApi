namespace MedStoreAPI.Dtos.Invoices
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Request DTOs for Invoices/Billing module.
    /// Note: GSTAmount and LineTotal are NOT accepted from the client - the
    /// Service layer recalculates them server-side from Quantity/UnitPrice/GSTPercentage
    /// to prevent tampering with billing amounts.
    /// </summary>
    public class InvoicesCreateRequestDto
    {
        public int StoreID { get; set; }
        public int? CustomerID { get; set; }
        public int PaymentModeID { get; set; }
        public decimal DiscountAmount { get; set; } = 0;
        public int CreatedBy { get; set; }
        public List<InvoiceItemRequestDto> Items { get; set; } = new();
    }

    public class InvoiceItemRequestDto
    {
        public int BatchID { get; set; }
        public int MedicineID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal GSTPercentage { get; set; }
    }

    public class InvoicesDateRangeRequestDto
    {
        public int StoreID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class InvoicesDailySummaryRequestDto
    {
        public int StoreID { get; set; }
        public DateTime InvoiceDate { get; set; }
    }
}
