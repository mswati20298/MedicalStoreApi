using MedStoreAPI.Common;
using MedStoreAPI.Domain;
using MedStoreAPI.Entities.Repositories;

namespace MedStoreAPI.Infrastructure.Repositories
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Data access implementation for Suppliers, calls stored
    /// procedures via ISqlDataAccess. Implements ISuppliersRepository.
    /// </summary>
    public class SuppliersRepository : ISuppliersRepository
    {
        private readonly ISqlDataAccess _db;

        public SuppliersRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<Supplier> InsertAsync(Supplier supplier)
        {
            var parameters = new
            {
                storeID = supplier.StoreId,
                supplierName = supplier.SupplierName,
                contactPerson = supplier.ContactPerson,
                contactNumber = supplier.ContactNumber,
                email = supplier.Email,
                address = supplier.Address,
                returnPolicyDays = supplier.ReturnPolicyDays,
                gstin = supplier.GSTIN
            };

            var supplierID = await _db.QuerySingleAsync<int>(StoredProcedureNames.Supplier.Insert, parameters);
            supplier.SupplierId = supplierID;
            return supplier;
        }

        public async Task UpdateAsync(Supplier supplier)
        {
            var parameters = new
            {
                supplierID = supplier.SupplierId,
                supplierName = supplier.SupplierName,
                contactPerson = supplier.ContactPerson,
                contactNumber = supplier.ContactNumber,
                email = supplier.Email,
                address = supplier.Address,
                returnPolicyDays = supplier.ReturnPolicyDays,
                gstin = supplier.GSTIN
            };

            await _db.ExecuteAsync(StoredProcedureNames.Supplier.Update, parameters);
        }

        public async Task<Supplier?> GetByIDAsync(int supplierID)
        {
            var parameters = new { supplierID };
            return await _db.QuerySingleAsync<Supplier>(StoredProcedureNames.Supplier.GetByID, parameters);
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync(int storeID)
        {
            var parameters = new { storeID };
            return await _db.QueryAsync<Supplier>(StoredProcedureNames.Supplier.GetAll, parameters);
        }

        public async Task DeleteAsync(int supplierID)
        {
            var parameters = new { supplierID };
            await _db.ExecuteAsync(StoredProcedureNames.Supplier.Delete, parameters);
        }
    }
}
