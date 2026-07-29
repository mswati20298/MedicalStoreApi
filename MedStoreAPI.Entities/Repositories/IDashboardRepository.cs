namespace MedStoreAPI.Entities.Repositories
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Contract for the Dashboard summary data access.
    /// SP_DashboardGetSummary returns 4 separate result sets (today's sales,
    /// low stock count, near expiry count, pending credit) - repository reads
    /// them in order using QueryMultipleAsync.
    /// Implemented by MedStoreAPI.Infrastructure.Repositories.DashboardRepository.
    /// </summary>
    public interface IDashboardRepository
    {
        Task<DashboardSummary> GetSummaryAsync(int storeID);
    }

    /// <summary>
    /// Not a table clone - aggregate result of SP_DashboardGetSummary.
    /// </summary>
    public class DashboardSummary
    {
        public decimal TodaySales { get; set; }
        public int LowStockCount { get; set; }
        public int NearExpiryCount { get; set; }
        public decimal TotalPendingCredit { get; set; }
    }
}
