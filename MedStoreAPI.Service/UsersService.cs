using MedStoreAPI.Common;
using MedStoreAPI.Domain;
using MedStoreAPI.Dtos.Users;
using MedStoreAPI.Entities.Repositories;
using MedStoreAPI.Entities.Services;

namespace MedStoreAPI.Service
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Business logic for Users/Auth. Handles password hashing
    /// (via IPasswordHasher), login verification, and JWT issuance (via
    /// IJwtTokenGenerator). PasswordHash never leaves this layer in any
    /// response DTO.
    /// Naming rule: Entity/Table "Users" -> Service class "UsersService".
    /// </summary>
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public UsersService(
            IUsersRepository usersRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ApiResponse<UsersResponseDto>> RegisterAsync(UsersRegisterRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return ApiResponse<UsersResponseDto>.Fail("Username and password are required.");
            }

            var existing = await _usersRepository.GetByUsernameAsync(request.Username);
            if (existing is not null)
            {
                return ApiResponse<UsersResponseDto>.Fail("Username already exists.");
            }

            var domainModel = new User
            {
                StoreId = request.StoreID,
                RoleId = request.RoleID,
                FullName = request.FullName,
                Username = request.Username,
                PasswordHash = _passwordHasher.Hash(request.Password),
                Email = request.Email,
                Mobile = request.Mobile
            };

            var inserted = await _usersRepository.InsertAsync(domainModel);
            var created = await _usersRepository.GetByIDAsync(inserted.UserId);

            return ApiResponse<UsersResponseDto>.Ok(MapToResponseDto(created!), "User registered successfully.");
        }

        public async Task<ApiResponse<UsersLoginResponseDto>> LoginAsync(UsersLoginRequestDto request)
        {
            var user = await _usersRepository.GetByUsernameAsync(request.Username);

            if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                // Deliberately vague message - never reveal whether username exists.
                return ApiResponse<UsersLoginResponseDto>.Fail("Invalid username or password.");
            }

            var (token, expiresAtUtc) = _jwtTokenGenerator.GenerateToken(
                user.UserId, user.Username, user.StoreId, user.RoleName ?? "Unknown");

            var response = new UsersLoginResponseDto
            {
                Token = token,
                ExpiresAtUtc = expiresAtUtc,
                User = MapToResponseDto(user)
            };

            return ApiResponse<UsersLoginResponseDto>.Ok(response, "Login successful.");
        }

        public async Task<ApiResponse<UsersResponseDto>> GetByIDAsync(int userID)
        {
            var user = await _usersRepository.GetByIDAsync(userID);
            if (user is null)
            {
                return ApiResponse<UsersResponseDto>.Fail("User not found.");
            }

            return ApiResponse<UsersResponseDto>.Ok(MapToResponseDto(user));
        }

        public async Task<ApiResponse<IEnumerable<UsersResponseDto>>> GetByStoreAsync(int storeID)
        {
            var users = await _usersRepository.GetByStoreAsync(storeID);
            var response = users.Select(MapToResponseDto);

            return ApiResponse<IEnumerable<UsersResponseDto>>.Ok(response);
        }

        public async Task<ApiResponse<bool>> ChangePasswordAsync(UsersChangePasswordRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.NewPassword) || request.NewPassword.Length < 6)
            {
                return ApiResponse<bool>.Fail("Password must be at least 6 characters.");
            }

            var newHash = _passwordHasher.Hash(request.NewPassword);
            await _usersRepository.UpdatePasswordAsync(request.UserID, newHash);

            return ApiResponse<bool>.Ok(true, "Password updated successfully.");
        }

        public async Task<ApiResponse<bool>> DeactivateAsync(int userID)
        {
            await _usersRepository.DeactivateAsync(userID);
            return ApiResponse<bool>.Ok(true, "User deactivated successfully.");
        }

        private static UsersResponseDto MapToResponseDto(User user)
        {
            return new UsersResponseDto
            {
                UserID = user.UserId,
                UserUID = user.UserUid,
                StoreID = user.StoreId,
                RoleID = user.RoleId,
                RoleName = user.RoleName,
                FullName = user.FullName,
                Username = user.Username,
                Email = user.Email,
                Mobile = user.Mobile,
                CreatedDate = user.CreatedDate
                // PasswordHash intentionally NOT mapped - never returned to client.
            };
        }
    }
}
