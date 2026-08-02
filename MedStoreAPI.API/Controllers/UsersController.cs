using MedStoreAPI.Dtos.Users;
using MedStoreAPI.Entities.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedStoreAPI.API.Controllers
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: User management endpoints - all require a valid JWT.
    /// StoreID comes from the token, so a Store A user can only ever list/
    /// manage Store A's users. Login/Register live in AuthController.
    /// </summary>
    [ApiController]
    [Route("api/user")]
    [Authorize(Roles = "Owner")]
    public class UsersController : SecureControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet("{userID:int}")]
        public async Task<IActionResult> GetByID(int userID)
        {
            var result = await _usersService.GetByIDAsync(userID);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStore()
        {
            var result = await _usersService.GetByStoreAsync(CurrentStoreID);
            return Ok(result);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] UsersChangePasswordRequestDto request)
        {
            var result = await _usersService.ChangePasswordAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{userID:int}/deactivate")]
        public async Task<IActionResult> Deactivate(int userID)
        {
            var result = await _usersService.DeactivateAsync(userID);
            return Ok(result);
        }
    }
}
