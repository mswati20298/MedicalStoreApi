using MedStoreAPI.Domain;

namespace MedStoreAPI.Entities.Repositories
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Contract for CustomerCredits (Udhaar) data access.
    /// Implemented by MedStoreAPI.Infrastructure.Repositories.CustomerCreditsRepository.
    /// </summary>
    public interface ICustomerCreditsRepository
    {
        Task<CustomerCredit> InsertAsync(CustomerCredit credit);
        Task<CustomerCredit?> GetByIDAsync(int creditID);
        Task<IEnumerable<CustomerCredit>> GetPendingAsync(int storeID);
        Task<IEnumerable<CustomerCredit>> GetByCustomerAsync(int customerID);
        Task AddPaymentAsync(int creditID, decimal amountPaid, int paymentModeID);
    }
}
