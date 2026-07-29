namespace MedStoreAPI.Dtos.Users
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Request DTOs for Users/Auth module.
    /// </summary>
    public class UsersRegisterRequestDto
    {
        public int StoreID { get; set; }
        public int RoleID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Mobile { get; set; }
    }

    public class UsersLoginRequestDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class UsersChangePasswordRequestDto
    {
        public int UserID { get; set; }
        public string NewPassword { get; set; } = string.Empty;
    }
}
