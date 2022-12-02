namespace Tenant_Management_System_Server.Models
{
    public class UserTokens
    {
        public string? Token { get; set; }
        public string? UserName { get; set; } = null;
        public string? FullName { get; set; } = null;
        public TimeSpan Validaty { get; set; }
        public string? RefreshToken { get; set; } = null;
        public string? Id { get; set; } 
        public string Email { get; set; } = null!;
        public string? UserType { get; set; } = null;
        public Guid GuidId { get; set; }
        public DateTime ExpiredTime { get; set; }
    }
}
