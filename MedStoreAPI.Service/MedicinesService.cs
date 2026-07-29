using MedStoreAPI.Common;
using MedStoreAPI.Domain;
using MedStoreAPI.Dtos.Medicines;
using MedStoreAPI.Entities.Repositories;
using MedStoreAPI.Entities.Services;

namespace MedStoreAPI.Service
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Business logic for Medicines. Maps between Request/Response
    /// DTOs and Domain models, wraps results in ApiResponse&lt;T&gt;.
    /// Naming rule: Entity/Table "Medicines" -> Service class "MedicinesService".
    /// </summary>
    public class MedicinesService : IMedicinesService
    {
        private readonly IMedicinesRepository _medicinesRepository;

        public MedicinesService(IMedicinesRepository medicinesRepository)
        {
            _medicinesRepository = medicinesRepository;
        }

        public async Task<ApiResponse<MedicinesResponseDto>> AddMedicineAsync(MedicinesRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return ApiResponse<MedicinesResponseDto>.Fail("Medicine name is required.");
            }

            var domainModel = new Medicine
            {
                StoreId = request.StoreID,
                Name = request.Name,
                Composition = request.Composition,
                Manufacturer = request.Manufacturer,
                CategoryId = request.CategoryID,
                UnitId = request.UnitID,
                GSTSlabId = request.GSTSlabID,
                HSNCode = request.HSNCode,
                PrescriptionRequired = request.PrescriptionRequired,
                ReorderPoint = request.ReorderPoint,
                MaxStockLevel = request.MaxStockLevel
            };

            var inserted = await _medicinesRepository.InsertAsync(domainModel);

            return ApiResponse<MedicinesResponseDto>.Ok(MapToResponseDto(inserted), "Medicine saved successfully.");
        }

        public async Task<ApiResponse<MedicinesResponseDto>> UpdateMedicineAsync(MedicinesUpdateRequestDto request)
        {
            var existing = await _medicinesRepository.GetByIDAsync(request.MedicineID);
            if (existing is null)
            {
                return ApiResponse<MedicinesResponseDto>.Fail("Medicine not found.");
            }

            if (existing.StoreId != request.StoreID)
            {
                return ApiResponse<MedicinesResponseDto>.Fail("You do not have permission to modify this medicine.");
            }

            var domainModel = new Medicine
            {
                MedicineId = request.MedicineID,
                Name = request.Name,
                Composition = request.Composition,
                Manufacturer = request.Manufacturer,
                CategoryId = request.CategoryID,
                UnitId = request.UnitID,
                GSTSlabId = request.GSTSlabID,
                HSNCode = request.HSNCode,
                PrescriptionRequired = request.PrescriptionRequired,
                ReorderPoint = request.ReorderPoint,
                MaxStockLevel = request.MaxStockLevel
            };

            await _medicinesRepository.UpdateAsync(domainModel);

            var updated = await _medicinesRepository.GetByIDAsync(request.MedicineID);
            return ApiResponse<MedicinesResponseDto>.Ok(MapToResponseDto(updated!), "Medicine updated successfully.");
        }

        public async Task<ApiResponse<MedicinesResponseDto>> GetMedicineByIDAsync(int medicineID, int storeID)
        {
            var medicine = await _medicinesRepository.GetByIDAsync(medicineID);
            if (medicine is null)
            {
                return ApiResponse<MedicinesResponseDto>.Fail("Medicine not found.");
            }

            if (medicine.StoreId != storeID)
            {
                // Deliberately same message as "not found" - don't reveal that the record
                // exists under a different store.
                return ApiResponse<MedicinesResponseDto>.Fail("Medicine not found.");
            }

            return ApiResponse<MedicinesResponseDto>.Ok(MapToResponseDto(medicine));
        }

        public async Task<ApiResponse<IEnumerable<MedicinesResponseDto>>> GetAllMedicinesAsync(int storeID)
        {
            var medicines = await _medicinesRepository.GetAllAsync(storeID);
            var response = medicines.Select(MapToResponseDto);

            return ApiResponse<IEnumerable<MedicinesResponseDto>>.Ok(response);
        }

        public async Task<ApiResponse<IEnumerable<MedicinesResponseDto>>> SearchMedicinesAsync(MedicinesSearchRequestDto request)
        {
            var medicines = await _medicinesRepository.SearchAsync(request.StoreID, request.SearchTerm);
            var response = medicines.Select(MapToResponseDto);

            return ApiResponse<IEnumerable<MedicinesResponseDto>>.Ok(response);
        }

        public async Task<ApiResponse<bool>> DeleteMedicineAsync(int medicineID, int storeID)
        {
            var existing = await _medicinesRepository.GetByIDAsync(medicineID);
            if (existing is null)
            {
                return ApiResponse<bool>.Fail("Medicine not found.");
            }

            if (existing.StoreId != storeID)
            {
                return ApiResponse<bool>.Fail("You do not have permission to delete this medicine.");
            }

            await _medicinesRepository.DeleteAsync(medicineID);
            return ApiResponse<bool>.Ok(true, "Medicine deleted successfully.");
        }

        private static MedicinesResponseDto MapToResponseDto(Medicine medicine)
        {
            return new MedicinesResponseDto
            {
                MedicineID = medicine.MedicineId,
                MedicineUID = medicine.MedicineUid,
                StoreID = medicine.StoreId,
                Name = medicine.Name,
                Composition = medicine.Composition,
                Manufacturer = medicine.Manufacturer,
                CategoryName = medicine.CategoryName,
                UnitName = medicine.UnitName,
                GSTPercentage = medicine.GSTPercentage,
                HSNCode = medicine.HSNCode,
                PrescriptionRequired = medicine.PrescriptionRequired,
                ReorderPoint = medicine.ReorderPoint,
                MaxStockLevel = medicine.MaxStockLevel,
                CreatedDate = medicine.CreatedDate
            };
        }
    }
}
