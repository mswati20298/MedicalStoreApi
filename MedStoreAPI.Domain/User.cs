namespace MedStoreAPI.Domain
{
    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Exact clone of the Users table.
    /// </summary>
    public class User
    {
        public int UserId { get; set; }
        public Guid UserUid { get; set; }
        public int StoreId { get; set; }
        public int RoleId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Joined field
        public string? RoleName { get; set; }
    }

    /// <summary>
    /// Author: Mahesh Kumar
    /// Date: 26/07/2026
    /// Description: Exact clone of the AuditLogs table.
    /// </summary>
    public class AuditLog
    {
        public long AuditLogId { get; set; }
        public int StoreId { get; set; }
        public int UserId { get; set; }
        public string EntityName { get; set; } = string.Empty;
        public int EntityId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
