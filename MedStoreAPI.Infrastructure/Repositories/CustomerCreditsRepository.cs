using MedStoreAPI.Common;
using MedStoreAPI.Domain;
using MedStoreAPI.Entities.Repositories;

namespace MedStoreAPI.Infrastructure.Repositories
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Data access implementation for CustomerCredits (Udhaar),
    /// calls stored procedures via ISqlDataAccess. Implements ICustomerCreditsRepository.
    /// </summary>
    public class CustomerCreditsRepository : ICustomerCreditsRepository
    {
        private readonly ISqlDataAccess _db;

        public CustomerCreditsRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<CustomerCredit> InsertAsync(CustomerCredit credit)
        {
            var parameters = new
            {
                storeID = credit.StoreId,
                customerID = credit.CustomerId,
                invoiceID = credit.InvoiceId,
                amount = credit.Amount,
                dueDate = credit.DueDate
            };

            var creditID = await _db.QuerySingleAsync<int>(StoredProcedureNames.CustomerCredit.Insert, parameters);
            credit.CreditId = creditID;
            return credit;
        }

        public async Task<CustomerCredit?> GetByIDAsync(int creditID)
        {
            var parameters = new { creditID };
            return await _db.QuerySingleAsync<CustomerCredit>(StoredProcedureNames.CustomerCredit.GetByID, parameters);
        }

        public async Task<IEnumerable<CustomerCredit>> GetPendingAsync(int storeID)
        {
            var parameters = new { storeID };
            return await _db.QueryAsync<CustomerCredit>(StoredProcedureNames.CustomerCredit.GetPending, parameters);
        }

        public async Task<IEnumerable<CustomerCredit>> GetByCustomerAsync(int customerID)
        {
            var parameters = new { customerID };
            return await _db.QueryAsync<CustomerCredit>(StoredProcedureNames.CustomerCredit.GetByCustomer, parameters);
        }

        public async Task AddPaymentAsync(int creditID, decimal amountPaid, int paymentModeID)
        {
            var parameters = new { creditID, amountPaid, paymentModeID };
            await _db.ExecuteAsync(StoredProcedureNames.CustomerCredit.AddPayment, parameters);
        }
    }
}
