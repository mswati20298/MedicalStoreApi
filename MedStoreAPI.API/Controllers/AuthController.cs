using MedStoreAPI.Dtos.Users;
using MedStoreAPI.Entities.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace MedStoreAPI.API.Controllers
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Public authentication endpoints (no JWT required to reach
    /// these - [AllowAnonymous] overrides any global auth policy).
    /// Rate-limited via "AuthPolicy" (max 5 requests/minute per IP, see
    /// Program.cs) - login/register/forgot-password are exactly the
    /// endpoints someone would try to brute-force or spam, so every action
    /// in this controller is covered.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    [AllowAnonymous]
    [EnableRateLimiting("AuthPolicy")]
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

        /// <summary>
        /// Step 1 of self-service password recovery - verifies username +
        /// registered mobile number match before letting the frontend show
        /// the "set new password" form.
        /// </summary>
        [HttpPost("forgot-password/verify")]
        public async Task<IActionResult> ForgotPasswordVerify([FromBody] ForgotPasswordVerifyRequestDto request)
        {
            var result = await _usersService.ForgotPasswordVerifyAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Step 2 - re-verifies username + mobile, then sets the new password.
        /// </summary>
        [HttpPost("forgot-password/reset")]
        public async Task<IActionResult> ForgotPasswordReset([FromBody] ForgotPasswordResetRequestDto request)
        {
            var result = await _usersService.ForgotPasswordResetAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
