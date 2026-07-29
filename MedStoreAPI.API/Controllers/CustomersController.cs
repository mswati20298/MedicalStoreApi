using MedStoreAPI.Dtos.Customers;
using MedStoreAPI.Entities.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedStoreAPI.API.Controllers
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: API endpoints for Customers module. Thin layer - only
    /// receives request, calls ICustomersService, returns ApiResponse&lt;T&gt;.
    /// No business logic lives here. Inherits SecureControllerBase - requires
    /// a valid JWT, and StoreID always comes from the token, never the client.
    /// </summary>
    [ApiController]
    [Route("api/customer")]
    public class CustomersController : SecureControllerBase
    {
        private readonly ICustomersService _customersService;

        public CustomersController(ICustomersService customersService)
        {
            _customersService = customersService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CustomersRequestDto request)
        {
            request.StoreID = CurrentStoreID; // ignore any client-supplied StoreID
            var result = await _customersService.AddCustomerAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("by-mobile")]
        public async Task<IActionResult> GetByMobile([FromQuery] string mobile)
        {
            var request = new CustomersGetByMobileRequestDto { StoreID = CurrentStoreID, Mobile = mobile };
            var result = await _customersService.GetCustomerByMobileAsync(request);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _customersService.GetAllCustomersAsync(CurrentStoreID);
            return Ok(result);
        }
    }
}
