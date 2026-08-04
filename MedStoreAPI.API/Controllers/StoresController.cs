using MedStoreAPI.Dtos.Stores;
using MedStoreAPI.Entities.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace MedStoreAPI.API.Controllers
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: API endpoints for Stores module - each medical store's
    /// own profile (name, address, GSTIN) and logo.
    ///
    /// Note on authorization for this controller specifically:
    /// - Add (creating a brand-new store) and GetAll (public directory) are
    ///   [AllowAnonymous] because a new store has no logged-in user yet -
    ///   this is the onboarding entry point before any Auth/register call
    ///   can happen (register needs an existing StoreID).
    /// - GetByID / Update / UploadLogo operate on ONE specific store, so
    ///   they require login AND verify the token's StoreID matches the
    ///   store being accessed - a Store A user cannot edit Store B's profile.
    /// </summary>
    [ApiController]
    [Route("api/store")]
    public class StoresController : SecureControllerBase
    {
        private readonly IStoresService _storesService;

        public StoresController(IStoresService storesService)
        {
            _storesService = storesService;
        }

        [HttpPost]
        [AllowAnonymous]
        [EnableRateLimiting("AuthPolicy")]
        public async Task<IActionResult> Add([FromBody] StoresRequestDto request)
        {
            var result = await _storesService.AddStoreAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _storesService.GetAllStoresAsync();
            return Ok(result);
        }

        [HttpGet("{storeID:int}")]
        public async Task<IActionResult> GetByID(int storeID)
        {
            if (storeID != CurrentStoreID)
            {
                return Forbid();
            }

            var result = await _storesService.GetStoreByIDAsync(storeID);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPut]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Update([FromBody] StoresUpdateRequestDto request)
        {
            if (request.StoreID != CurrentStoreID)
            {
                return Forbid();
            }

            var result = await _storesService.UpdateStoreAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Uploads/replaces a store's logo. Send as multipart/form-data
        /// with a single file field named "logoFile".
        /// </summary>
        [HttpPost("{storeID:int}/logo")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> UploadLogo(int storeID, IFormFile logoFile)
        {
            if (storeID != CurrentStoreID)
            {
                return Forbid();
            }

            var result = await _storesService.UploadLogoAsync(storeID, logoFile);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
