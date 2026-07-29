using MedStoreAPI.Common;
using MedStoreAPI.Dtos.Dashboard;
using MedStoreAPI.Entities.Repositories;
using MedStoreAPI.Entities.Services;

namespace MedStoreAPI.Service
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Business logic for the Dashboard summary widget.
    /// </summary>
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<ApiResponse<DashboardSummaryResponseDto>> GetSummaryAsync(int storeID)
        {
            var summary = await _dashboardRepository.GetSummaryAsync(storeID);

            var response = new DashboardSummaryResponseDto
            {
                TodaySales = summary.TodaySales,
                LowStockCount = summary.LowStockCount,
                NearExpiryCount = summary.NearExpiryCount,
                TotalPendingCredit = summary.TotalPendingCredit
            };

            return ApiResponse<DashboardSummaryResponseDto>.Ok(response);
        }
    }
}
