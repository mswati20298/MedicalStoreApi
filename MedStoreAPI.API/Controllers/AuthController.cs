using MedStoreAPI.Dtos.Users;
using MedStoreAPI.Entities.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedStoreAPI.API.Controllers
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Public authentication endpoints (no JWT required to reach
    /// these - [AllowAnonymous] overrides any global auth policy).
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public AuthController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UsersRegisterRequestDto request)
        {
            var result = await _usersService.RegisterAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsersLoginRequestDto request)
        {
            var result = await _usersService.LoginAsync(request);
            return result.Success ? Ok(result) : Unauthorized(result);
        }
    }
}
