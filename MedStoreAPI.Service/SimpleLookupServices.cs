using MedStoreAPI.Common;
using MedStoreAPI.Domain;
using MedStoreAPI.Dtos.GSTSlabs;
using MedStoreAPI.Dtos.PaymentModes;
using MedStoreAPI.Dtos.Units;
using MedStoreAPI.Entities.Repositories;
using MedStoreAPI.Entities.Services;

namespace MedStoreAPI.Service
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Business logic for Units, GSTSlabs, PaymentModes (master data).
    /// Naming rule: table "Units" -> "UnitsService", "GSTSlabs" -> "GSTSlabsService",
    /// "PaymentModes" -> "PaymentModesService".
    /// </summary>
    public class UnitsService : IUnitsService
    {
        private readonly IUnitsRepository _unitsRepository;
        public UnitsService(IUnitsRepository unitsRepository) => _unitsRepository = unitsRepository;

        public async Task<ApiResponse<UnitsResponseDto>> AddAsync(UnitsRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.UnitName))
            {
                return ApiResponse<UnitsResponseDto>.Fail("Unit name is required.");
            }

            var inserted = await _unitsRepository.InsertAsync(new Unit { UnitName = request.UnitName });
            return ApiResponse<UnitsResponseDto>.Ok(new UnitsResponseDto { UnitID = inserted.UnitId, UnitName = inserted.UnitName }, "Unit added successfully.");
        }

        public async Task<ApiResponse<IEnumerable<UnitsResponseDto>>> GetAllAsync()
        {
            var units = await _unitsRepository.GetAllAsync();
            var response = units.Select(u => new UnitsResponseDto { UnitID = u.UnitId, UnitName = u.UnitName });
            return ApiResponse<IEnumerable<UnitsResponseDto>>.Ok(response);
        }
    }

    public class GSTSlabsService : IGSTSlabsService
    {
        private readonly IGSTSlabsRepository _gstSlabsRepository;
        public GSTSlabsService(IGSTSlabsRepository gstSlabsRepository) => _gstSlabsRepository = gstSlabsRepository;

        public async Task<ApiResponse<GSTSlabsResponseDto>> AddAsync(GSTSlabsRequestDto request)
        {
            if (request.Percentage < 0)
            {
                return ApiResponse<GSTSlabsResponseDto>.Fail("GST percentage cannot be negative.");
            }

            var inserted = await _gstSlabsRepository.InsertAsync(new GSTSlab { Percentage = request.Percentage });
            return ApiResponse<GSTSlabsResponseDto>.Ok(new GSTSlabsResponseDto { GSTSlabID = inserted.GSTSlabId, Percentage = inserted.Percentage }, "GST slab added successfully.");
        }

        public async Task<ApiResponse<IEnumerable<GSTSlabsResponseDto>>> GetAllAsync()
        {
            var slabs = await _gstSlabsRepository.GetAllAsync();
            var response = slabs.Select(s => new GSTSlabsResponseDto { GSTSlabID = s.GSTSlabId, Percentage = s.Percentage });
            return ApiResponse<IEnumerable<GSTSlabsResponseDto>>.Ok(response);
        }
    }

    public class PaymentModesService : IPaymentModesService
    {
        private readonly IPaymentModesRepository _paymentModesRepository;
        public PaymentModesService(IPaymentModesRepository paymentModesRepository) => _paymentModesRepository = paymentModesRepository;

        public async Task<ApiResponse<PaymentModesResponseDto>> AddAsync(PaymentModesRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.ModeName))
            {
                return ApiResponse<PaymentModesResponseDto>.Fail("Payment mode name is required.");
            }

            var inserted = await _paymentModesRepository.InsertAsync(new PaymentMode { ModeName = request.ModeName });
            return ApiResponse<PaymentModesResponseDto>.Ok(new PaymentModesResponseDto { PaymentModeID = inserted.PaymentModeId, ModeName = inserted.ModeName }, "Payment mode added successfully.");
        }

        public async Task<ApiResponse<IEnumerable<PaymentModesResponseDto>>> GetAllAsync()
        {
            var modes = await _paymentModesRepository.GetAllAsync();
            var response = modes.Select(m => new PaymentModesResponseDto { PaymentModeID = m.PaymentModeId, ModeName = m.ModeName });
            return ApiResponse<IEnumerable<PaymentModesResponseDto>>.Ok(response);
        }
    }
}
