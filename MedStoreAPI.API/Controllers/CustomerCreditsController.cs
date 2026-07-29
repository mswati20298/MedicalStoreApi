using MedStoreAPI.Dtos.CustomerCredits;
using MedStoreAPI.Entities.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedStoreAPI.API.Controllers
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: API endpoints for CustomerCredits (Udhaar) module.
    /// StoreID comes from JWT.
    /// </summary>
    [ApiController]
    [Route("api/customercredit")]
    public class CustomerCreditsController : SecureControllerBase
    {
        private readonly ICustomerCreditsService _customerCreditsService;

        public CustomerCreditsController(ICustomerCreditsService customerCreditsService)
        {
            _customerCreditsService = customerCreditsService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CustomerCreditsRequestDto request)
        {
            request.StoreID = CurrentStoreID;
            var result = await _customerCreditsService.AddCreditAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var result = await _customerCreditsService.GetPendingAsync(CurrentStoreID);
            return Ok(result);
        }

        [HttpGet("by-customer/{customerID:int}")]
        public async Task<IActionResult> GetByCustomer(int customerID)
        {
            var result = await _customerCreditsService.GetByCustomerAsync(customerID, CurrentStoreID);
            return Ok(result);
        }

        [HttpPost("add-payment")]
        public async Task<IActionResult> AddPayment([FromBody] CustomerCreditsAddPaymentRequestDto request)
        {
            var result = await _customerCreditsService.AddPaymentAsync(request, CurrentStoreID);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
