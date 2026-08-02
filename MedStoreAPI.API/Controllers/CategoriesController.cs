using MedStoreAPI.Dtos.Categories;
using MedStoreAPI.Entities.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedStoreAPI.API.Controllers
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: API endpoints for Categories (master data) module.
    /// Categories are a GLOBAL lookup shared across all stores (no StoreId
    /// column on this table), so this only requires a valid JWT (any logged-in
    /// user of any store) - not scoped to a specific StoreID.
    /// </summary>
    [ApiController]
    [Route("api/category")]
    public class CategoriesController : SecureControllerBase
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Add([FromBody] CategoriesRequestDto request)
        {
            var result = await _categoriesService.AddAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoriesService.GetAllAsync();
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Update([FromBody] CategoriesUpdateRequestDto request)
        {
            var result = await _categoriesService.UpdateAsync(request);
            return Ok(result);
        }

        [HttpDelete("{categoryID:int}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Delete(int categoryID)
        {
            var result = await _categoriesService.DeleteAsync(categoryID);
            return Ok(result);
        }
    }
}
