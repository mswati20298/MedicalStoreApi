using MedStoreAPI.Common;
using MedStoreAPI.Dtos.Users;

namespace MedStoreAPI.Entities.Services
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Contract for Users/Auth business logic - registration,
    /// login (password verify + JWT issue), password change, user listing.
    /// Implemented by MedStoreAPI.Service.UsersService.
    /// Naming rule: Entity/Table "Users" -> Service class "UsersService".
    /// </summary>
    public interface IUsersService
    {
        Task<ApiResponse<UsersResponseDto>> RegisterAsync(UsersRegisterRequestDto request);
        Task<ApiResponse<UsersLoginResponseDto>> LoginAsync(UsersLoginRequestDto request);
        Task<ApiResponse<UsersResponseDto>> GetByIDAsync(int userID);
        Task<ApiResponse<IEnumerable<UsersResponseDto>>> GetByStoreAsync(int storeID);
        Task<ApiResponse<bool>> ChangePasswordAsync(UsersChangePasswordRequestDto request);
        Task<ApiResponse<bool>> DeactivateAsync(int userID);
    }
}
