using MedStoreAPI.Common;
using MedStoreAPI.Entities.Repositories;

namespace MedStoreAPI.Infrastructure.Repositories
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Data access implementation for Dashboard summary.
    /// SP_DashboardGetSummary returns 4 result sets in order:
    /// 1) TodaySales, 2) LowStockCount, 3) NearExpiryCount, 4) TotalPendingCredit.
    /// </summary>
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ISqlDataAccess _db;

        public DashboardRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<DashboardSummary> GetSummaryAsync(int storeID)
        {
            var parameters = new { storeID };
            using var multi = await _db.QueryMultipleAsync(StoredProcedureNames.Dashboard.GetSummary, parameters);

            var todaySales = await multi.ReadSingleAsync<decimal>();
            var lowStockCount = await multi.ReadSingleAsync<int>();
            var nearExpiryCount = await multi.ReadSingleAsync<int>();
            var totalPendingCredit = await multi.ReadSingleAsync<decimal>();

            return new DashboardSummary
            {
                TodaySales = todaySales,
                LowStockCount = lowStockCount,
                NearExpiryCount = nearExpiryCount,
                TotalPendingCredit = totalPendingCredit
            };
        }
    }
}
