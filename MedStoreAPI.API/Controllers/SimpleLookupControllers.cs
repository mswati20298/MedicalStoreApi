using MedStoreAPI.Dtos.GSTSlabs;
using MedStoreAPI.Dtos.PaymentModes;
using MedStoreAPI.Dtos.Units;
using MedStoreAPI.Entities.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedStoreAPI.API.Controllers
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: API endpoints for Units, GSTSlabs, PaymentModes (master data).
    /// </summary>
    /// Requires a valid JWT (any logged-in user) - these are GLOBAL lookups,
    /// not scoped to a specific store (no StoreId column on these tables).
    [ApiController]
    [Route("api/unit")]
    public class UnitsController : SecureControllerBase
    {
        private readonly IUnitsService _unitsService;
        public UnitsController(IUnitsService unitsService) => _unitsService = unitsService;

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Add([FromBody] UnitsRequestDto request)
        {
            var result = await _unitsService.AddAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _unitsService.GetAllAsync());
    }

    [ApiController]
    [Route("api/gstslab")]
    public class GSTSlabsController : SecureControllerBase
    {
        private readonly IGSTSlabsService _gstSlabsService;
        public GSTSlabsController(IGSTSlabsService gstSlabsService) => _gstSlabsService = gstSlabsService;

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Add([FromBody] GSTSlabsRequestDto request)
        {
            var result = await _gstSlabsService.AddAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _gstSlabsService.GetAllAsync());
    }

    [ApiController]
    [Route("api/paymentmode")]
    public class PaymentModesController : SecureControllerBase
    {
        private readonly IPaymentModesService _paymentModesService;
        public PaymentModesController(IPaymentModesService paymentModesService) => _paymentModesService = paymentModesService;

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Add([FromBody] PaymentModesRequestDto request)
        {
            var result = await _paymentModesService.AddAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _paymentModesService.GetAllAsync());
    }
}
