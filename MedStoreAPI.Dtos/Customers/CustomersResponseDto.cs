namespace MedStoreAPI.Dtos.Customers
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Response DTO for Customers module. This is the shape
    /// returned FROM the API to the Angular frontend.
    /// </summary>
    public class CustomersResponseDto
    {
        public int CustomerID { get; set; }
        public Guid CustomerUID { get; set; }
        public int StoreID { get; set; }
        public string? Name { get; set; }
        public string? Mobile { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
