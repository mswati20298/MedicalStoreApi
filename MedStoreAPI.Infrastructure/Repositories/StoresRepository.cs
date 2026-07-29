using MedStoreAPI.Common;
using MedStoreAPI.Domain;
using MedStoreAPI.Entities.Repositories;

namespace MedStoreAPI.Infrastructure.Repositories
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Data access implementation for Stores, calls stored
    /// procedures via ISqlDataAccess. Implements IStoresRepository.
    /// </summary>
    public class StoresRepository : IStoresRepository
    {
        private readonly ISqlDataAccess _db;

        public StoresRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<Store> InsertAsync(Store store)
        {
            var parameters = new
            {
                storeName = store.StoreName,
                address = store.Address,
                city = store.City,
                state = store.State,
                pincode = store.Pincode,
                gstin = store.GSTIN,
                drugLicenseNumber = store.DrugLicenseNumber,
                contactNumber = store.ContactNumber,
                email = store.Email
            };

            var storeID = await _db.QuerySingleAsync<int>(StoredProcedureNames.Store.Insert, parameters);
            store.StoreId = storeID;
            return store;
        }

        public async Task<Store?> GetByIDAsync(int storeID)
        {
            var parameters = new { storeID };
            return await _db.QuerySingleAsync<Store>(StoredProcedureNames.Store.GetByID, parameters);
        }

        public async Task<IEnumerable<Store>> GetAllAsync()
        {
            return await _db.QueryAsync<Store>(StoredProcedureNames.Store.GetAll);
        }

        public async Task UpdateAsync(Store store)
        {
            var parameters = new
            {
                storeID = store.StoreId,
                storeName = store.StoreName,
                address = store.Address,
                city = store.City,
                state = store.State,
                pincode = store.Pincode,
                gstin = store.GSTIN,
                drugLicenseNumber = store.DrugLicenseNumber,
                contactNumber = store.ContactNumber,
                email = store.Email,
                logoUrl = store.LogoUrl
            };

            await _db.ExecuteAsync(StoredProcedureNames.Store.Update, parameters);
        }

        public async Task UpdateLogoAsync(int storeID, string logoUrl)
        {
            var parameters = new { storeID, logoUrl };
            await _db.ExecuteAsync(StoredProcedureNames.Store.UpdateLogo, parameters);
        }
    }
}
