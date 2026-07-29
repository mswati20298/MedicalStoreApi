using MedStoreAPI.Dtos.Batches;
using MedStoreAPI.Entities.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedStoreAPI.API.Controllers
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: API endpoints for Batches module (stock + expiry management).
    /// StoreID comes from JWT.
    /// </summary>
    [ApiController]
    [Route("api/batch")]
    public class BatchesController : SecureControllerBase
    {
        private readonly IBatchesService _batchesService;

        public BatchesController(IBatchesService batchesService)
        {
            _batchesService = batchesService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BatchesRequestDto request)
        {
            request.StoreID = CurrentStoreID;
            var result = await _batchesService.AddBatchAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("by-medicine/{medicineID:int}")]
        public async Task<IActionResult> GetByMedicine(int medicineID)
        {
            var result = await _batchesService.GetByMedicineAsync(medicineID);
            return Ok(result);
        }

        [HttpGet("expiry-status")]
        public async Task<IActionResult> GetExpiryStatus([FromQuery] int redDays = 30, [FromQuery] int yellowDays = 90)
        {
            var request = new BatchesExpiryStatusRequestDto { StoreID = CurrentStoreID, RedDays = redDays, YellowDays = yellowDays };
            var result = await _batchesService.GetExpiryStatusAsync(request);
            return Ok(result);
        }

        [HttpGet("expiring")]
        public async Task<IActionResult> GetExpiring([FromQuery] int days = 30)
        {
            var request = new BatchesExpiringRequestDto { StoreID = CurrentStoreID, Days = days };
            var result = await _batchesService.GetExpiringAsync(request);
            return Ok(result);
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStock()
        {
            var result = await _batchesService.GetLowStockAsync(CurrentStoreID);
            return Ok(result);
        }

        [HttpDelete("{batchID:int}")]
        public async Task<IActionResult> Delete(int batchID)
        {
            var result = await _batchesService.DeleteBatchAsync(batchID, CurrentStoreID);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
