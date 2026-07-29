using MedStoreAPI.Entities.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedStoreAPI.API.Controllers
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: API endpoint for the Dashboard summary widget.
    /// StoreID comes from JWT.
    /// </summary>
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : SecureControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var result = await _dashboardService.GetSummaryAsync(CurrentStoreID);
            return Ok(result);
        }
    }
}
