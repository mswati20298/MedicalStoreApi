namespace MedStoreAPI.Dtos.Dashboard
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Response DTO for the combined Dashboard summary widget.
    /// </summary>
    public class DashboardSummaryResponseDto
    {
        public decimal TodaySales { get; set; }
        public int LowStockCount { get; set; }
        public int NearExpiryCount { get; set; }
        public decimal TotalPendingCredit { get; set; }
    }
}
