using MedStoreAPI.Common;
using MedStoreAPI.Domain;
using MedStoreAPI.Dtos.GSTSlabs;
using MedStoreAPI.Dtos.PaymentModes;
using MedStoreAPI.Dtos.Units;

namespace MedStoreAPI.Entities.Repositories
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Contracts for Units, GSTSlabs, PaymentModes data access -
    /// simple lookup tables, Insert + GetAll only (matching available SPs).
    /// </summary>
    public interface IUnitsRepository
    {
        Task<Unit> InsertAsync(Unit unit);
        Task<IEnumerable<Unit>> GetAllAsync();
    }

    public interface IGSTSlabsRepository
    {
        Task<GSTSlab> InsertAsync(GSTSlab gstSlab);
        Task<IEnumerable<GSTSlab>> GetAllAsync();
    }

    public interface IPaymentModesRepository
    {
        Task<PaymentMode> InsertAsync(PaymentMode paymentMode);
        Task<IEnumerable<PaymentMode>> GetAllAsync();
    }
}

namespace MedStoreAPI.Entities.Services
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Contracts for Units, GSTSlabs, PaymentModes business logic.
    /// Naming rule: table "Units" -> "UnitsService", "GSTSlabs" -> "GSTSlabsService",
    /// "PaymentModes" -> "PaymentModesService".
    /// </summary>
    public interface IUnitsService
    {
        Task<ApiResponse<UnitsResponseDto>> AddAsync(UnitsRequestDto request);
        Task<ApiResponse<IEnumerable<UnitsResponseDto>>> GetAllAsync();
    }

    public interface IGSTSlabsService
    {
        Task<ApiResponse<GSTSlabsResponseDto>> AddAsync(GSTSlabsRequestDto request);
        Task<ApiResponse<IEnumerable<GSTSlabsResponseDto>>> GetAllAsync();
    }

    public interface IPaymentModesService
    {
        Task<ApiResponse<PaymentModesResponseDto>> AddAsync(PaymentModesRequestDto request);
        Task<ApiResponse<IEnumerable<PaymentModesResponseDto>>> GetAllAsync();
    }
}
