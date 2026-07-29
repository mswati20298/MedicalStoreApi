using MedStoreAPI.Common;
using MedStoreAPI.Dtos.Invoices;

namespace MedStoreAPI.Entities.Services
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Contract for Invoices/Billing business logic. Works with
    /// Request/Response DTOs. Implemented by MedStoreAPI.Service.InvoicesService.
    /// Naming rule: Entity/Table "Invoices" -> Service class "InvoicesService".
    /// </summary>
    public interface IInvoicesService
    {
        Task<ApiResponse<InvoicesResponseDto>> CreateInvoiceAsync(InvoicesCreateRequestDto request);
        Task<ApiResponse<InvoicesResponseDto>> GetInvoiceByIDAsync(int invoiceID, int storeID);
        Task<ApiResponse<IEnumerable<InvoicesResponseDto>>> GetByDateRangeAsync(InvoicesDateRangeRequestDto request);
        Task<ApiResponse<bool>> CancelInvoiceAsync(int invoiceID, int storeID);
        Task<ApiResponse<InvoiceDailySummaryResponseDto>> GetDailySummaryAsync(InvoicesDailySummaryRequestDto request);
    }
}
