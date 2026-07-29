using MedStoreAPI.Common;
using MedStoreAPI.Domain;
using MedStoreAPI.Entities.Repositories;

namespace MedStoreAPI.Infrastructure.Repositories
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Data access implementation for Customers, calls stored
    /// procedures via ISqlDataAccess. Implements ICustomersRepository from
    /// MedStoreAPI.Entities.
    /// </summary>
    public class CustomersRepository : ICustomersRepository
    {
        private readonly ISqlDataAccess _db;

        public CustomersRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<Customer> InsertAsync(Customer customer)
        {
            var parameters = new
            {
                storeID = customer.StoreId,
                name = customer.Name,
                mobile = customer.Mobile,
                address = customer.Address
            };

            var customerID = await _db.QuerySingleAsync<int>(StoredProcedureNames.Customer.Insert, parameters);

            customer.CustomerId = customerID;
            return customer;
        }

        public async Task<Customer?> GetByMobileAsync(int storeID, string mobile)
        {
            var parameters = new { storeID, mobile };
            return await _db.QuerySingleAsync<Customer>(StoredProcedureNames.Customer.GetByMobile, parameters);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync(int storeID)
        {
            var parameters = new { storeID };
            return await _db.QueryAsync<Customer>(StoredProcedureNames.Customer.GetAll, parameters);
        }
    }
}
