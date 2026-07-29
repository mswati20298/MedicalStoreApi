namespace MedStoreAPI.Dtos.Users
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Response DTOs for Users/Auth module. Note: PasswordHash is
    /// NEVER included in any response DTO - it must never leave the server.
    /// </summary>
    public class UsersResponseDto
    {
        public int UserID { get; set; }
        public Guid UserUID { get; set; }
        public int StoreID { get; set; }
        public int RoleID { get; set; }
        public string? RoleName { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    /// <summary>
    /// Returned by POST /api/Auth/login - contains the JWT to be sent as
    /// "Authorization: Bearer {token}" on subsequent requests.
    /// </summary>
    public class UsersLoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAtUtc { get; set; }
        public UsersResponseDto User { get; set; } = new();
    }
}
