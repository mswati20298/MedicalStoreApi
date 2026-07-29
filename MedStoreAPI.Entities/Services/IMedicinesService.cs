using MedStoreAPI.Common;
using MedStoreAPI.Dtos.Medicines;

namespace MedStoreAPI.Entities.Services
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Contract for Medicines business logic. Works with Request/Response DTOs.
    /// Implemented by MedStoreAPI.Service.MedicinesService.
    /// Naming rule: Entity/Table name "Medicines" -> Service name "MedicinesService".
    /// </summary>
    public interface IMedicinesService
    {
        Task<ApiResponse<MedicinesResponseDto>> AddMedicineAsync(MedicinesRequestDto request);
        Task<ApiResponse<MedicinesResponseDto>> UpdateMedicineAsync(MedicinesUpdateRequestDto request);
        Task<ApiResponse<MedicinesResponseDto>> GetMedicineByIDAsync(int medicineID, int storeID);
        Task<ApiResponse<IEnumerable<MedicinesResponseDto>>> GetAllMedicinesAsync(int storeID);
        Task<ApiResponse<IEnumerable<MedicinesResponseDto>>> SearchMedicinesAsync(MedicinesSearchRequestDto request);
        Task<ApiResponse<bool>> DeleteMedicineAsync(int medicineID, int storeID);
    }
}
