using MedStoreAPI.Common;
using MedStoreAPI.Domain;
using MedStoreAPI.Entities.Repositories;

namespace MedStoreAPI.Infrastructure.Repositories
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Data access implementation for Medicines, calls stored
    /// procedures via ISqlDataAccess. Implements IMedicinesRepository from
    /// MedStoreAPI.Entities.
    /// </summary>
    public class MedicinesRepository : IMedicinesRepository
    {
        private readonly ISqlDataAccess _db;

        public MedicinesRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<Medicine> InsertAsync(Medicine medicine)
        {
            var parameters = new
            {
                storeID = medicine.StoreId,
                name = medicine.Name,
                composition = medicine.Composition,
                manufacturer = medicine.Manufacturer,
                categoryID = medicine.CategoryId,
                unitID = medicine.UnitId,
                gstSlabID = medicine.GSTSlabId,
                hsnCode = medicine.HSNCode,
                prescriptionRequired = medicine.PrescriptionRequired,
                reorderPoint = medicine.ReorderPoint,
                maxStockLevel = medicine.MaxStockLevel
            };

            var medicineID = await _db.QuerySingleAsync<int>(StoredProcedureNames.Medicine.Insert, parameters);
            medicine.MedicineId = medicineID;
            return medicine;
        }

        public async Task UpdateAsync(Medicine medicine)
        {
            var parameters = new
            {
                medicineID = medicine.MedicineId,
                name = medicine.Name,
                composition = medicine.Composition,
                manufacturer = medicine.Manufacturer,
                categoryID = medicine.CategoryId,
                unitID = medicine.UnitId,
                gstSlabID = medicine.GSTSlabId,
                hsnCode = medicine.HSNCode,
                prescriptionRequired = medicine.PrescriptionRequired,
                reorderPoint = medicine.ReorderPoint,
                maxStockLevel = medicine.MaxStockLevel
            };

            await _db.ExecuteAsync(StoredProcedureNames.Medicine.Update, parameters);
        }

        public async Task<Medicine?> GetByIDAsync(int medicineID)
        {
            var parameters = new { medicineID };
            return await _db.QuerySingleAsync<Medicine>(StoredProcedureNames.Medicine.GetByID, parameters);
        }

        public async Task<IEnumerable<Medicine>> GetAllAsync(int storeID)
        {
            var parameters = new { storeID };
            return await _db.QueryAsync<Medicine>(StoredProcedureNames.Medicine.GetAll, parameters);
        }

        public async Task<IEnumerable<Medicine>> SearchAsync(int storeID, string searchTerm)
        {
            var parameters = new { storeID, searchTerm };
            return await _db.QueryAsync<Medicine>(StoredProcedureNames.Medicine.Search, parameters);
        }

        public async Task DeleteAsync(int medicineID)
        {
            var parameters = new { medicineID };
            await _db.ExecuteAsync(StoredProcedureNames.Medicine.Delete, parameters);
        }
    }
}
