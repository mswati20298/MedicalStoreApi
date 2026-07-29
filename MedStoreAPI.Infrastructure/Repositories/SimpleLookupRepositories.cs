using MedStoreAPI.Common;
using MedStoreAPI.Domain;
using MedStoreAPI.Entities.Repositories;

namespace MedStoreAPI.Infrastructure.Repositories
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Data access implementations for Units, GSTSlabs, PaymentModes.
    /// </summary>
    public class UnitsRepository : IUnitsRepository
    {
        private readonly ISqlDataAccess _db;
        public UnitsRepository(ISqlDataAccess db) => _db = db;

        public async Task<Unit> InsertAsync(Unit unit)
        {
            var parameters = new { unitName = unit.UnitName };
            unit.UnitId = await _db.QuerySingleAsync<int>(StoredProcedureNames.Unit.Insert, parameters);
            return unit;
        }

        public async Task<IEnumerable<Unit>> GetAllAsync()
            => await _db.QueryAsync<Unit>(StoredProcedureNames.Unit.GetAll);
    }

    public class GSTSlabsRepository : IGSTSlabsRepository
    {
        private readonly ISqlDataAccess _db;
        public GSTSlabsRepository(ISqlDataAccess db) => _db = db;

        public async Task<GSTSlab> InsertAsync(GSTSlab gstSlab)
        {
            var parameters = new { percentage = gstSlab.Percentage };
            gstSlab.GSTSlabId = await _db.QuerySingleAsync<int>(StoredProcedureNames.GSTSlab.Insert, parameters);
            return gstSlab;
        }

        public async Task<IEnumerable<GSTSlab>> GetAllAsync()
            => await _db.QueryAsync<GSTSlab>(StoredProcedureNames.GSTSlab.GetAll);
    }

    public class PaymentModesRepository : IPaymentModesRepository
    {
        private readonly ISqlDataAccess _db;
        public PaymentModesRepository(ISqlDataAccess db) => _db = db;

        public async Task<PaymentMode> InsertAsync(PaymentMode paymentMode)
        {
            var parameters = new { modeName = paymentMode.ModeName };
            paymentMode.PaymentModeId = await _db.QuerySingleAsync<int>(StoredProcedureNames.PaymentMode.Insert, parameters);
            return paymentMode;
        }

        public async Task<IEnumerable<PaymentMode>> GetAllAsync()
            => await _db.QueryAsync<PaymentMode>(StoredProcedureNames.PaymentMode.GetAll);
    }
}
