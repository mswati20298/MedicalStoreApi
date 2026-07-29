using MedStoreAPI.Common;
using MedStoreAPI.Domain;
using MedStoreAPI.Dtos.Invoices;
using MedStoreAPI.Entities.Repositories;
using MedStoreAPI.Entities.Services;

namespace MedStoreAPI.Service
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Business logic for Invoices/Billing. Recalculates GST and
    /// line totals server-side (never trusts client-sent amounts), generates
    /// the invoice number, and wraps the header+items creation in a single
    /// atomic repository call. Maps Domain <-> DTOs and wraps results in
    /// ApiResponse&lt;T&gt;.
    /// Naming rule: Entity/Table "Invoices" -> Service class "InvoicesService".
    /// </summary>
    public class InvoicesService : IInvoicesService
    {
        private readonly IInvoicesRepository _invoicesRepository;

        public InvoicesService(IInvoicesRepository invoicesRepository)
        {
            _invoicesRepository = invoicesRepository;
        }

        public async Task<ApiResponse<InvoicesResponseDto>> CreateInvoiceAsync(InvoicesCreateRequestDto request)
        {
            if (request.Items is null || request.Items.Count == 0)
            {
                return ApiResponse<InvoicesResponseDto>.Fail("Invoice must have at least one item.");
            }

            // Recalculate every line server-side - never trust client-sent GST/LineTotal.
            var domainItems = new List<InvoiceItem>();
            decimal subTotal = 0;
            decimal totalGST = 0;

            foreach (var line in request.Items)
            {
                if (line.Quantity <= 0)
                {
                    return ApiResponse<InvoicesResponseDto>.Fail($"Invalid quantity for BatchID {line.BatchID}.");
                }

                var lineBase = line.Quantity * line.UnitPrice;
                var gstAmount = Math.Round(lineBase * line.GSTPercentage / 100m, 2);
                var lineTotal = lineBase + gstAmount;

                subTotal += lineBase;
                totalGST += gstAmount;

                domainItems.Add(new InvoiceItem
                {
                    BatchId = line.BatchID,
                    MedicineId = line.MedicineID,
                    Quantity = line.Quantity,
                    UnitPrice = line.UnitPrice,
                    GSTPercentage = line.GSTPercentage,
                    GSTAmount = gstAmount,
                    LineTotal = lineTotal
                });
            }

            var totalAmount = subTotal + totalGST - request.DiscountAmount;

            var invoice = new Invoice
            {
                StoreId = request.StoreID,
                InvoiceNumber = GenerateInvoiceNumber(request.StoreID),
                CustomerId = request.CustomerID,
                PaymentModeId = request.PaymentModeID,
                SubTotal = subTotal,
                TotalGST = totalGST,
                DiscountAmount = request.DiscountAmount,
                TotalAmount = totalAmount,
                CreatedBy = request.CreatedBy
            };

            try
            {
                var created = await _invoicesRepository.CreateWithItemsAsync(invoice, domainItems);

                var (header, items) = await _invoicesRepository.GetByIDAsync(created.InvoiceId);
                return ApiResponse<InvoicesResponseDto>.Ok(MapToResponseDto(header!, items), "Invoice created successfully.");
            }
            catch (InvalidOperationException ex)
            {
                // Thrown by repository when a batch has insufficient stock - transaction already rolled back.
                return ApiResponse<InvoicesResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<InvoicesResponseDto>> GetInvoiceByIDAsync(int invoiceID, int storeID)
        {
            var (header, items) = await _invoicesRepository.GetByIDAsync(invoiceID);

            if (header is null || header.StoreId != storeID)
            {
                return ApiResponse<InvoicesResponseDto>.Fail("Invoice not found.");
            }

            return ApiResponse<InvoicesResponseDto>.Ok(MapToResponseDto(header, items));
        }

        public async Task<ApiResponse<IEnumerable<InvoicesResponseDto>>> GetByDateRangeAsync(InvoicesDateRangeRequestDto request)
        {
            var invoices = await _invoicesRepository.GetByDateRangeAsync(request.StoreID, request.FromDate, request.ToDate);
            var response = invoices.Select(inv => MapToResponseDto(inv, Enumerable.Empty<InvoiceItem>()));

            return ApiResponse<IEnumerable<InvoicesResponseDto>>.Ok(response);
        }

        public async Task<ApiResponse<bool>> CancelInvoiceAsync(int invoiceID, int storeID)
        {
            var (header, _) = await _invoicesRepository.GetByIDAsync(invoiceID);
            if (header is null || header.StoreId != storeID)
            {
                return ApiResponse<bool>.Fail("Invoice not found.");
            }

            await _invoicesRepository.CancelAsync(invoiceID);
            return ApiResponse<bool>.Ok(true, "Invoice cancelled and stock restored successfully.");
        }

        public async Task<ApiResponse<InvoiceDailySummaryResponseDto>> GetDailySummaryAsync(InvoicesDailySummaryRequestDto request)
        {
            var summary = await _invoicesRepository.GetDailySummaryAsync(request.StoreID, request.InvoiceDate);

            var response = new InvoiceDailySummaryResponseDto
            {
                TotalInvoices = summary.TotalInvoices,
                TotalSubTotal = summary.TotalSubTotal,
                TotalGSTCollected = summary.TotalGSTCollected,
                TotalDiscount = summary.TotalDiscount,
                TotalSales = summary.TotalSales
            };

            return ApiResponse<InvoiceDailySummaryResponseDto>.Ok(response);
        }

        /// <summary>
        /// Simple invoice number generator: INV-{StoreID}-{timestamp}.
        /// Replace with a proper per-store sequential counter later if needed.
        /// </summary>
        private static string GenerateInvoiceNumber(int storeID)
        {
            return $"INV-{storeID}-{DateTime.Now:yyyyMMddHHmmssfff}";
        }

        private static InvoicesResponseDto MapToResponseDto(Invoice invoice, IEnumerable<InvoiceItem> items)
        {
            return new InvoicesResponseDto
            {
                InvoiceID = invoice.InvoiceId,
                InvoiceUID = invoice.InvoiceUid,
                StoreID = invoice.StoreId,
                InvoiceNumber = invoice.InvoiceNumber,
                CustomerID = invoice.CustomerId,
                CustomerName = invoice.CustomerName,
                CustomerMobile = invoice.CustomerMobile,
                ModeName = invoice.ModeName,
                SubTotal = invoice.SubTotal,
                TotalGST = invoice.TotalGST,
                DiscountAmount = invoice.DiscountAmount,
                TotalAmount = invoice.TotalAmount,
                InvoiceDate = invoice.InvoiceDate,
                Items = items.Select(i => new InvoiceItemResponseDto
                {
                    InvoiceItemID = i.InvoiceItemId,
                    BatchID = i.BatchId,
                    BatchNumber = i.BatchNumber,
                    ExpiryDate = i.ExpiryDate,
                    MedicineID = i.MedicineId,
                    MedicineName = i.MedicineName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    GSTPercentage = i.GSTPercentage,
                    GSTAmount = i.GSTAmount,
                    LineTotal = i.LineTotal
                }).ToList()
            };
        }
    }
}
