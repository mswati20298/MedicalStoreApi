using System.Data;
using Dapper;
using MedStoreAPI.Common;
using MedStoreAPI.Domain;
using MedStoreAPI.Entities.Repositories;

namespace MedStoreAPI.Infrastructure.Repositories
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Data access implementation for Invoices/Billing.
    /// CreateWithItemsAsync manages its own connection + transaction directly
    /// (instead of using ISqlDataAccess) because header insert + all item
    /// inserts + stock reduction must succeed or fail together atomically.
    /// If any item's batch has insufficient stock, the whole transaction
    /// rolls back and nothing is committed.
    /// </summary>
    public class InvoicesRepository : IInvoicesRepository
    {
        private readonly ISqlDataAccess _db;
        private readonly IDbConnectionFactory _connectionFactory;

        public InvoicesRepository(ISqlDataAccess db, IDbConnectionFactory connectionFactory)
        {
            _db = db;
            _connectionFactory = connectionFactory;
        }

        public async Task<Invoice> CreateWithItemsAsync(Invoice invoice, List<InvoiceItem> items)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                var invoiceParameters = new
                {
                    storeID = invoice.StoreId,
                    invoiceNumber = invoice.InvoiceNumber,
                    customerID = invoice.CustomerId,
                    paymentModeID = invoice.PaymentModeId,
                    subTotal = invoice.SubTotal,
                    totalGST = invoice.TotalGST,
                    discountAmount = invoice.DiscountAmount,
                    totalAmount = invoice.TotalAmount,
                    createdBy = invoice.CreatedBy
                };

                var invoiceID = await connection.QuerySingleAsync<int>(
                    StoredProcedureNames.Invoice.Create,
                    invoiceParameters,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);

                invoice.InvoiceId = invoiceID;

                foreach (var item in items)
                {
                    var itemParameters = new
                    {
                        invoiceID = invoiceID,
                        batchID = item.BatchId,
                        medicineID = item.MedicineId,
                        quantity = item.Quantity,
                        unitPrice = item.UnitPrice,
                        gstPercentage = item.GSTPercentage,
                        gstAmount = item.GSTAmount,
                        lineTotal = item.LineTotal
                    };

                    var result = await connection.QuerySingleAsync<InvoiceItemStockResult>(
                        StoredProcedureNames.Invoice.ItemInsertAndReduceStock,
                        itemParameters,
                        transaction: transaction,
                        commandType: CommandType.StoredProcedure);

                    if (result.Success == 0)
                    {
                        transaction.Rollback();
                        throw new InvalidOperationException(
                            $"Insufficient stock for BatchID {item.BatchId} (MedicineID {item.MedicineId}): {result.Message}");
                    }
                }

                transaction.Commit();
                return invoice;
            }
            catch
            {
                if (transaction.Connection != null)
                {
                    transaction.Rollback();
                }
                throw;
            }
        }

        public async Task<(Invoice? Header, IEnumerable<InvoiceItem> Items)> GetByIDAsync(int invoiceID)
        {
            var parameters = new { invoiceID };
            using var multi = await _db.QueryMultipleAsync(StoredProcedureNames.Invoice.GetByID, parameters);

            var header = await multi.ReadSingleOrDefaultAsync<Invoice>();
            var items = await multi.ReadAsync<InvoiceItem>();

            return (header, items);
        }

        public async Task<IEnumerable<Invoice>> GetByDateRangeAsync(int storeID, DateTime fromDate, DateTime toDate)
        {
            var parameters = new { storeID, fromDate, toDate };
            return await _db.QueryAsync<Invoice>(StoredProcedureNames.Invoice.GetByDateRange, parameters);
        }

        public async Task CancelAsync(int invoiceID)
        {
            var parameters = new { invoiceID };
            await _db.ExecuteAsync(StoredProcedureNames.Invoice.Cancel, parameters);
        }

        public async Task<InvoiceDailySummary> GetDailySummaryAsync(int storeID, DateTime invoiceDate)
        {
            var parameters = new { storeID, invoiceDate };
            var result = await _db.QuerySingleAsync<InvoiceDailySummary>(StoredProcedureNames.Invoice.GetDailySummary, parameters);
            return result ?? new InvoiceDailySummary();
        }
    }
}
