using MedStoreAPI.Dtos.Invoices;
using MedStoreAPI.Entities.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedStoreAPI.API.Controllers
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: API endpoints for Invoices/Billing module. StoreID comes
    /// from JWT; CreatedBy is set from the logged-in user's own UserID claim
    /// (not trusted from the client either).
    /// </summary>
    [ApiController]
    [Route("api/invoice")]
    public class InvoicesController : SecureControllerBase
    {
        private readonly IInvoicesService _invoicesService;

        public InvoicesController(IInvoicesService invoicesService)
        {
            _invoicesService = invoicesService;
        }

        private int CurrentUserID => int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InvoicesCreateRequestDto request)
        {
            request.StoreID = CurrentStoreID;
            request.CreatedBy = CurrentUserID;
            var result = await _invoicesService.CreateInvoiceAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{invoiceID:int}")]
        public async Task<IActionResult> GetByID(int invoiceID)
        {
            var result = await _invoicesService.GetInvoiceByIDAsync(invoiceID, CurrentStoreID);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            var request = new InvoicesDateRangeRequestDto { StoreID = CurrentStoreID, FromDate = fromDate, ToDate = toDate };
            var result = await _invoicesService.GetByDateRangeAsync(request);
            return Ok(result);
        }

        [HttpPost("{invoiceID:int}/cancel")]
        public async Task<IActionResult> Cancel(int invoiceID)
        {
            var result = await _invoicesService.CancelInvoiceAsync(invoiceID, CurrentStoreID);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("daily-summary")]
        public async Task<IActionResult> GetDailySummary([FromQuery] DateTime invoiceDate)
        {
            var request = new InvoicesDailySummaryRequestDto { StoreID = CurrentStoreID, InvoiceDate = invoiceDate };
            var result = await _invoicesService.GetDailySummaryAsync(request);
            return Ok(result);
        }
    }
}
