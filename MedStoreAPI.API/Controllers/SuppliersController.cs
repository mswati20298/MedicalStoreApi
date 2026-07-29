using MedStoreAPI.Dtos.Suppliers;
using MedStoreAPI.Entities.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedStoreAPI.API.Controllers
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: API endpoints for Suppliers module. StoreID comes from JWT.
    /// </summary>
    [ApiController]
    [Route("api/supplier")]
    public class SuppliersController : SecureControllerBase
    {
        private readonly ISuppliersService _suppliersService;

        public SuppliersController(ISuppliersService suppliersService)
        {
            _suppliersService = suppliersService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] SuppliersRequestDto request)
        {
            request.StoreID = CurrentStoreID;
            var result = await _suppliersService.AddSupplierAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SuppliersUpdateRequestDto request)
        {
            request.StoreID = CurrentStoreID;
            var result = await _suppliersService.UpdateSupplierAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{supplierID:int}")]
        public async Task<IActionResult> GetByID(int supplierID)
        {
            var result = await _suppliersService.GetSupplierByIDAsync(supplierID, CurrentStoreID);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _suppliersService.GetAllSuppliersAsync(CurrentStoreID);
            return Ok(result);
        }

        [HttpDelete("{supplierID:int}")]
        public async Task<IActionResult> Delete(int supplierID)
        {
            var result = await _suppliersService.DeleteSupplierAsync(supplierID, CurrentStoreID);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
