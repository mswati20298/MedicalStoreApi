using MedStoreAPI.Dtos.Medicines;
using MedStoreAPI.Entities.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedStoreAPI.API.Controllers
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: API endpoints for Medicines module. StoreID always comes
    /// from the JWT (via SecureControllerBase.CurrentStoreID), never trusted
    /// from client input.
    /// </summary>
    [ApiController]
    [Route("api/medicine")]
    public class MedicinesController : SecureControllerBase
    {
        private readonly IMedicinesService _medicinesService;

        public MedicinesController(IMedicinesService medicinesService)
        {
            _medicinesService = medicinesService;
        }

        [HttpPost]
        [Authorize(Roles = "Owner,Pharmacist")]
        public async Task<IActionResult> Add([FromBody] MedicinesRequestDto request)
        {
            request.StoreID = CurrentStoreID;
            var result = await _medicinesService.AddMedicineAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        [Authorize(Roles = "Owner,Pharmacist")]
        public async Task<IActionResult> Update([FromBody] MedicinesUpdateRequestDto request)
        {
            request.StoreID = CurrentStoreID;
            var result = await _medicinesService.UpdateMedicineAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{medicineID:int}")]
        public async Task<IActionResult> GetByID(int medicineID)
        {
            var result = await _medicinesService.GetMedicineByIDAsync(medicineID, CurrentStoreID);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _medicinesService.GetAllMedicinesAsync(CurrentStoreID);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string searchTerm)
        {
            var request = new MedicinesSearchRequestDto { StoreID = CurrentStoreID, SearchTerm = searchTerm };
            var result = await _medicinesService.SearchMedicinesAsync(request);
            return Ok(result);
        }

        [HttpDelete("{medicineID:int}")]
        [Authorize(Roles = "Owner,Pharmacist")]
        public async Task<IActionResult> Delete(int medicineID)
        {
            var result = await _medicinesService.DeleteMedicineAsync(medicineID, CurrentStoreID);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
