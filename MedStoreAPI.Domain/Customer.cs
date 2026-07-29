namespace MedStoreAPI.Domain
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Exact clone of the Customers table. This class represents
    /// the raw DB row - used only between Infrastructure (Dapper mapping) and
    /// Service layer. Controllers/API never expose this directly - they use
    /// CustomersRequestDto / CustomersResponseDto instead.
    /// </summary>
    public class Customer
    {
        public int CustomerId { get; set; }
        public Guid CustomerUid { get; set; }
        public int StoreId { get; set; }
        public string? Name { get; set; }
        public string? Mobile { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
