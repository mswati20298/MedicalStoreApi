using MedStoreAPI.Domain;

namespace MedStoreAPI.Entities.Repositories
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Contract for Invoices data access. CreateWithItemsAsync wraps
    /// the header insert + all item inserts + stock reduction in a single DB
    /// transaction - if any item has insufficient stock, everything rolls back.
    /// Implemented by MedStoreAPI.Infrastructure.Repositories.InvoicesRepository.
    /// </summary>
    public interface IInvoicesRepository
    {
        /// <summary>
        /// Creates invoice header + all items atomically. Throws InvalidOperationException
        /// (with the failing batch/message) if any item's batch has insufficient stock -
        /// the whole transaction is rolled back in that case.
        /// </summary>
        Task<Invoice> CreateWithItemsAsync(Invoice invoice, List<InvoiceItem> items);

        Task<(Invoice? Header, IEnumerable<InvoiceItem> Items)> GetByIDAsync(int invoiceID);
        Task<IEnumerable<Invoice>> GetByDateRangeAsync(int storeID, DateTime fromDate, DateTime toDate);
        Task CancelAsync(int invoiceID);
        Task<InvoiceDailySummary> GetDailySummaryAsync(int storeID, DateTime invoiceDate);
    }
}

