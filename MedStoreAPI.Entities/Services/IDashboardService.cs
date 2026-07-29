using MedStoreAPI.Common;
using MedStoreAPI.Dtos.Dashboard;

namespace MedStoreAPI.Entities.Services
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Contract for Dashboard business logic.
    /// Implemented by MedStoreAPI.Service.DashboardService.
    /// </summary>
    public interface IDashboardService
    {
        Task<ApiResponse<DashboardSummaryResponseDto>> GetSummaryAsync(int storeID);
    }
}
