namespace MedStoreAPI.Dtos.Customers
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Request DTOs for Customers module. These are the shapes
    /// the Angular frontend sends TO the API. Kept separate from Domain so
    /// that DB column changes don't automatically break/change the API contract.
    /// </summary>
    public class CustomersRequestDto
    {
        public int StoreID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string? Address { get; set; }
    }

    public class CustomersGetByMobileRequestDto
    {
        public int StoreID { get; set; }
        public string Mobile { get; set; } = string.Empty;
    }
}
